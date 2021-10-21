using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members
public static class Roll
{
	public static int GetWeightedRoll(Dictionary<int, float> weights)
	{
		float weight = Random.value;
		float currentWeight = 0;

		foreach (KeyValuePair<int, float> kvp in weights)
			if (currentWeight + kvp.Value >= weight)
				return kvp.Key;
			else
				currentWeight += kvp.Value;

		Debug.LogError($"Invalid weight: {currentWeight}");
		return -1;
	}

	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
	// https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/types/generics
	public static T GetRandomEntry<T>( this T[] array)
  {
		if (array == null || array.Length == 0)
			return default;

		return array[Random.Range(0, array.Length)];
  }

	public static int DieRoll(int amountOfDie, int minEyes, int maxEyes)
	{
		int amount = 0;

		for (int i = 0; i < amountOfDie; i++)
			amount += Random.Range(minEyes, maxEyes + 1);

		return amount;
	}

	public static int DieRoll(int amountOfDie, int minEyes, int maxEyes, int variance)
	{
		return DieRoll(amountOfDie, minEyes, maxEyes) + amountOfDie * variance;
	}

	// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
	public static Vector3 Randomize(this ref Vector3 vector, float offset = 0.1F)
	{
		Vector3 randomization = new Vector3();

		for (int i = 0; i < 2; i++)
			randomization[i] += Random.Range(-offset, offset);

		return vector + randomization;
	}
}
