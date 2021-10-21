using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Brain : MonoBehaviour
{
	protected List<Transform> enemies = new List<Transform>();
	protected float timeStamp;
	[SerializeField]
	private LayerMask enemyMask;

	protected EnemyInput Owner { get; set; }
	//protected LayerMask EnemyMask { get; set; }
	protected BrainModifiers Modifiers { get; set; }
	protected bool IsActive { get; set; }

	private void Awake()
	{
		Owner = GetComponent<EnemyInput>();
		IsActive = true;
	}

	public void StartBrain()
	{
		// Assign the brain to the Owner
		Owner.onUpdate += Execute;
		Owner.onBeginOverlap += Detect;
		Owner.onEndOverlap += StopDetectings;
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual
	/// Order all enemies by range
	/// </summary>
	public virtual void Execute()
	{
		// Lambda ordering of the Square Magnitude (tiny optimization instead of using regular magnitude)
		enemies = enemies.OrderBy(x => Vector3.SqrMagnitude(x.transform.position - Owner.transform.position)).ToList();
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual
	/// Check if the object is in range
	/// </summary>
	/// <param name="other"></param>
	/// <returns>Can this object attack</returns>
	public virtual bool CanAttack(Transform other)
	{
		// Using squared distance is a tiny optimization, but sometimes worth it
		bool hasEnemies = enemies.Count > 0;
		// We're using distance squared, is a tiny optimisation
		float enemySqrDistance = (other.position - Owner.transform.position).sqrMagnitude;
		float possibleAttackRange = Mathf.Pow(Modifiers.attackRange * Modifiers.attackRangeModifier, 2);
		bool inRange = enemySqrDistance < possibleAttackRange;
		bool noCooldown = timeStamp + Modifiers.attackTime < Time.time;

		return hasEnemies && inRange && noCooldown;
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual
	/// </summary>
	public virtual void Move() 
	{
		// This probably needs a timer to prevent it from being jerky on update
		Vector3 direction = Vector3.zero;
		direction.Randomize(1);
		// Normalize direction, otherwise we'll move quicker when going diagonal
		Owner.Move(direction.normalized);
	}

	public virtual void Detect(Collider2D other) 
	{
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators
		if (enemyMask == (enemyMask | (1 << other.gameObject.layer)))
			enemies.Add(other.transform);
	}

	public virtual void StopDetectings(Collider2D other)
	{
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators
		if (enemyMask == (enemyMask | (1 << other.gameObject.layer)))
			enemies.Remove(other.transform);
	}

	public virtual void SetBrainModifiers(BrainModifiers modifiers)
	{
		// Currently the on spawn bool is not used, change that if you need it
		Owner.ApplyModifiers(modifiers);
		Modifiers = modifiers;
	}

	public virtual void DoDamage(Transform enemy)
	{
		Health enemyHealth = enemy.GetComponent<Health>();

		if (!enemyHealth)
			return;

		enemyHealth.AdjustHealth(-Modifiers.damage, Modifiers.damageModifier);
	}

	public virtual void Stop() 
	{
		IsActive = false;
		Owner.onUpdate -= Execute;
		Owner.onBeginOverlap -= Detect;
		Owner.onEndOverlap -= StopDetectings;
	}

	public virtual bool Attack() 
	{ 
		return false; 
	}
}

/// <summary>
/// Settings for the brain
/// </summary>
[System.Serializable]
public struct BrainSettings
{
	public EnemyInput enemyInput;
	public BrainModifiers basicSettings;
	public string brainName;
	public float weight;
	public int weaknessesCount;
	public int strengthsCount;
}

/// <summary>
/// What modifiers to apply to the brain
/// </summary>
[System.Serializable]
public struct BrainLinkModifier
{
	public string name;
	public float weight;
	public BrainModifiers modifier;
}

/// <summary>
/// The actual modifier that contains all the settings for an enemy
/// </summary>
[System.Serializable]
public struct BrainModifiers
{
	public bool applyOnSpawn;

	public float attackRange;
	[Range(0.0F, 2.0F), Tooltip("Set to 1 if no change is desired")]
	public float attackRangeModifier;

	public float attackTime;
	[Range(0.0F, 2.0F), Tooltip("Set to 1 if no change is desired")]
	public float attackTimeModifier;

	public float damage;
	[Range(0.0F, 2.0F), Tooltip("Set to 1 if no change is desired")]
	public float damageModifier;

	public float health;
	[Range(0.0F, 2.0F), Tooltip("Set to 1 if no change is desired")]
	public float healthModifier;

	public float speed;
	[Range(0.0F, 2.0F), Tooltip("Set to 1 if no change is desired")]
	public float speedModifier;

	/// <summary>
	/// This is to make sure that modifiers don't get out of hand. 
	/// </summary>
	/// <param name="first">First data set</param>
	/// <param name="second">Second data</param>
	/// <returns></returns>
	public static float AdjustModifier(float first, float second)
	{
		if (second > 1)
			second -= 1;
		else
			second = 1 - second;

		return first + second;
	}

	public static BrainModifiers operator +(BrainModifiers a, BrainModifiers b)
	{
		return new BrainModifiers()
		{
			attackRange = a.attackRange + b.attackRange,
			attackRangeModifier = AdjustModifier(a.attackRangeModifier, b.attackRangeModifier),
			attackTime = a.attackTime + b.attackTime,
			attackTimeModifier = AdjustModifier(a.attackTimeModifier, b.attackTimeModifier),
			damage = a.damage + b.damage,
			damageModifier = AdjustModifier(a.damageModifier, b.damageModifier),
			health = a.health + b.health,
			healthModifier = AdjustModifier(a.healthModifier, b.healthModifier),
			speed = a.speed + b.speed,
			speedModifier = AdjustModifier(a.speedModifier, b.speedModifier),
		};
	}
}