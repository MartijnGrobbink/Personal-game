using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	EnemyInput[] enemies;
	float lastTimestamp;
	[SerializeField]
	float spawnInterval = 4f;
	[SerializeField]
	float spawnRange = 1f;
	public bool CanSpawn { get; set; }

	private void Start()
	{
		if (enemies == null || enemies.Length == 0)
			Debug.LogError("No enemies set or enemies is null");
	}

	void Update()
	{
		if (Time.time > lastTimestamp + spawnInterval && CanSpawn)
			Spawn();
	}

	private void Spawn()
	{
		// Spawn enemy
		EnemyInput input = enemies.GetRandomEntry();

		// Spawn around the camera OR player.
		// Easiest? Player with a range
		Vector3 offset = new Vector3(GenerateRandomPosition(), GenerateRandomPosition(), 0);
		// Camera follows the player if it's there
		Vector3 spawnPosition = Camera.main.transform.position + offset;
		// Remove the camera offset
		spawnPosition.z = 0;

		if (input)
		{
			EnemyInput enemy = Instantiate(input, spawnPosition, Quaternion.identity);
			enemy.name = input.name;
		}

		// Better: spawn outside the camera view frustrum (harder)
		lastTimestamp = Time.time;
	}

	private float GenerateRandomPosition()
	{
		return Random.Range(-spawnRange, spawnRange);
	}
}
