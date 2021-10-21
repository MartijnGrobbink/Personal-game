using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basically a data container, perhaps could've been a struct as well
public class Tile : MonoBehaviour
{
	int x, y, z;

	public void SetPosition(int x, int y, int z = 0)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vector3Int GetPosition()
	{
		return new Vector3Int(x, y, z);
	}
}
