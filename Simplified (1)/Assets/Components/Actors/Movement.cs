using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// See "EnemyInput" for more information
/// </summary>
[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
	// Rigidbody2D should already be in use, hence the "new" Rigidbody2D rigidbody2D
	new Rigidbody2D rigidbody2D;
	CircleCollider2D circleCollider2D;
	// SerializeField, to keep class encapsulated: https://unity3d.college/2016/02/15/editing-unity-variables-encapsulation-serializefield/
	[SerializeField]
	float speed;
	float speedModifier;

	Vector3 direction;
	Vector3 lastDirection;
	[SerializeField]
	LayerMask movementBlockMask;
	Transform myTransform;

	public UnityAction onStopMove;
	public UnityAction onMove;

	public float SpeedModifier
	{
		get { return speedModifier; }
		set { speedModifier = value; }
	}

	public Vector3 Direction
	{
		get { return direction; }
		private set { direction = value; }
	}

	private void Start()
	{ // Make sure we never need GetComponents after Start
		rigidbody2D = GetComponent<Rigidbody2D>();
		circleCollider2D = GetComponent<CircleCollider2D>();
		myTransform = transform;
	}

	private void FixedUpdate()
	{
		ApplyMovement();
	}

	public void SetMovementModifiers(BrainModifiers modifiers)
	{
		speedModifier += modifiers.speedModifier;
		speedModifier += Random.Range(speedModifier * 0.9F, speedModifier * 1.1F);
		speed += modifiers.speed;
	}

	private void ApplyMovement()
	{
		if (this.direction == Vector3.zero)
		{
			rigidbody2D.velocity = Direction;
			onStopMove?.Invoke();
			return;
		}

		Vector3 direction = this.direction * (speed * SpeedModifier);

		// Check if anything is crossing our path before we move
		if (Physics2D.OverlapCircle(myTransform.position + direction * Time.fixedDeltaTime, circleCollider2D.radius, movementBlockMask))
		{
			// Stop moving if there is something blocking
			// Tip: It's better to move in the plane that is still possible
			// It does require some math if you want that to happen (i.e. check for normal and cross)
			this.direction = Vector3.zero;
			return;
		}

		rigidbody2D.velocity = direction;
		onMove?.Invoke();
	}

	public void Move(Vector3 direction)
	{
		if (direction != Vector3.zero)
			lastDirection = Direction;

		Direction = direction;
	}

	public bool IsMoving()
	{
		return direction != Vector3.zero;
	}

	public Vector3 GetLastDirection(bool getLargestAxisMagnitude = true)
	{
		// Get the direction
		if (!getLargestAxisMagnitude)
			return lastDirection;

		Vector3 movementVector = lastDirection;
		if (Mathf.Abs(lastDirection.x) > Mathf.Abs(lastDirection.y))
			movementVector.y = 0;
		else
			movementVector.x = 0;

		// This is for the animator, to return the last position the player was facing
		return movementVector;
	}
}
