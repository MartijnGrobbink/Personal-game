using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	readonly List<Transform> players = new List<Transform>();
	Transform myTransform;
	[SerializeField]
	float smoothness;
	Vector3 velocity;
	[SerializeField]
	Vector3 cameraOffset;

	public Vector3 CameraOffset
	{
		get { return cameraOffset; }
		private set { cameraOffset = value; }
	}
	private void Start()
	{
		myTransform = transform;
	}

	// LateUpdate is called after everything has moved
	private void LateUpdate()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		// This code allows for the following of two players, but does NOT account for distance between them
		// You might want to move away in the Z axis or activate Cameras for each player 
		// Do that in this class because it already knows where the players are

		if (players.Count == 0)
			return;

		Vector3 newPosition = Vector3.zero;

		for (int i = 0; i < players.Count; i++)
			newPosition += players[i].position;
		
		// Divide and assign the variable by what's on the right
		newPosition /= players.Count;
		// Add and assign the offset to the new position
		newPosition += cameraOffset;

		// Smooth the movement so it doesn't "jerk"
		myTransform.position = Vector3.SmoothDamp(myTransform.position, newPosition, ref velocity, smoothness);
	}

	public void AddPlayer(Transform newPlayer)
	{
		if (players.Contains(newPlayer))
			return;

		players.Add(newPlayer);
	}

	public void RemovePlayer(Transform newPlayer)
	{
		players.Remove(newPlayer);
	}
}
