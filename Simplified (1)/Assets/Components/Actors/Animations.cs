using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// Make sure that movement and Animator are added once this component is added to a GameObject
// The player "feature" of incorrect movement is in here, it's related to stopping and getting the last movement
[RequireComponent(typeof(Movement), typeof(Animator))]
public class Animations : MonoBehaviour
{
	Movement movement;
	Animator animator;
	Vector3 lastDirection;
	[SerializeField]
	string HorizontalAxis = "Horizontal";
	[SerializeField]
	string VerticalAxis = "Vertical";
	[SerializeField]
	float stopHorizontal = -1;
	[SerializeField]
	float stopVertical = -1;

	private void Start()
	{
		// Assign all components to their respective EventHandlers
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
		movement.onMove += PlayMoveAnimation;
		movement.onStopMove += StopMoveAnimation;
	}

	private void PlayMoveAnimation()
	{
		// Get the previous direction
		Vector3 currentDirection = movement.GetLastDirection(true);

		float horizontal = stopHorizontal;
		float vertical = stopVertical;

		if (currentDirection == lastDirection)
			return;

		bool isHorizontal = Mathf.Abs(currentDirection.x) > Mathf.Abs(currentDirection.y);

		// Ensure character always moves left or right instead of both up and left
		if(isHorizontal)
		{
			horizontal = currentDirection.x > 0 ? 1 : 0;
			SetMovement(currentDirection, horizontal, vertical);
			return;
		}

		vertical = currentDirection.y > 0 ? 1 : 0;
		SetMovement(currentDirection, horizontal, vertical);
	}

	private void StopMoveAnimation()
	{
		// Get last direction
		Vector3 lastDirection = movement.GetLastDirection(true);
		// Tell the animator to stop animating movement
		SetMovement(lastDirection, stopHorizontal, stopVertical);
	}

	/// <summary>
	/// Transfer data to the animator so it functions
	/// </summary>
	/// <param name="currentDirection">The current direction</param>
	/// <param name="horizontal">X</param>
	/// <param name="vertical">Y</param>
	private void SetMovement(Vector3 currentDirection, float horizontal, float vertical)
	{
		animator.SetFloat(HorizontalAxis, horizontal);
		animator.SetFloat(VerticalAxis, vertical);
		lastDirection = currentDirection;
	}
}
