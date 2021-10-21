using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// C# Attributes: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/
/// RequireComponent: https://docs.unity3d.com/ScriptReference/RequireComponent.html
/// This class takes care of all input to its object
/// </summary>
[RequireComponent(typeof(Movement), typeof(CircleCollider2D), typeof(Health))]
public class EnemyInput : MonoBehaviour
{
	// What brain is possessing it?
	// Tip: If you're creating a zombie game, you can have recently deceased their brain be replaced by a zombie "brain"
	Brain brain;
	// Same movement as player
	Movement movement;
	// Health
	Health health;
	// Bitwise mask, it's set to default currently (Unity has 32 layers, so is using a 32 bit integer for mask
	[SerializeField]
	LayerMask enemyMask = 0x01;
	public UnityAction<Collider2D> onBeginOverlap;
	public UnityAction<Collider2D> onEndOverlap;
	public UnityAction onUpdate;

	private void Awake()
	{
		movement = GetComponent<Movement>();
		movement.SpeedModifier = 1;

		health = GetComponent<Health>();
	}

	private void OnTriggerEnter2D(Collider2D collider2D)
	{
		onBeginOverlap?.Invoke(collider2D);
	}

	private void OnTriggerExit2D(Collider2D collider2D)
	{
		onEndOverlap?.Invoke(collider2D);
	}

	private void Update()
	{
		onUpdate?.Invoke();
	}

	public void Move(Vector3 direction)
	{
		movement.Move(direction);
	}

	/// <summary>
	/// Remove the brain (object doesn't do anything anymore)
	/// </summary>
	public void RemoveBrain()
	{
		brain.Stop();
		onUpdate -= brain.Execute;
	}

	/// <summary>
	/// Apply all modifiers
	/// </summary>
	/// <param name="modifiers">Modifiers that were gathered outside of this class</param>
	public void ApplyModifiers(BrainModifiers modifiers)
	{
		health.SetHealthModifiers(modifiers);
		movement.SetMovementModifiers(modifiers);
	}
}
