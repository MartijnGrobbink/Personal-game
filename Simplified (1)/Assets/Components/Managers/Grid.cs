using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Grid
{
	Tile[] prefab;

	Tile[,] tiles;
	int width, height;
	Transform root;

	public void Initialise(Tile[] prefab, int width, int height)
	{
		this.prefab = prefab;
		tiles = new Tile[width, height];
		this.width = width;
		this.height = height;

		// Was there already a Grid available
		if (root)
			Object.Destroy(root.gameObject);

		// Create a new root to apply everything on
		root = new GameObject("Level").transform;
	}

	public void GenerateMap(bool onlyEmpty = true)
	{
		// For row, generate every column
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				if (onlyEmpty && tiles[x, y] == null)
					tiles[x, y] = SpawnPrefab(prefab.GetRandomEntry(), x, y);
	}

	/// <summary>
	/// Create a tile
	/// Tip: you can also use a Vector2Int
	/// </summary>
	/// <param name="prefab">Prefab tile</param>
	/// <param name="x">x position</param>
	/// <param name="y">y position</param>
	/// <returns>The spawned tile (if it needs to be used straight away</returns>
	private Tile SpawnPrefab(Tile prefab, int x, int y)
	{
		Tile newTile = Object.Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
		newTile.name = $"[{x},{y}]: {prefab.name}";
		newTile.transform.parent = root;

		return newTile;
	}

	public Tile GetTile(int x, int y)
	{
		return tiles[x, y];
	}

	/// <summary>
	/// Update a specific tile with a new Tile
	/// </summary>
	/// <param name="newTile">The new tile</param>
	/// <param name="x">x position</param>
	/// <param name="y">y position</param>
	/// <returns></returns>
	public Tile SetTile(Tile newTile, int x, int y)
	{
		if (!newTile.gameObject.scene.IsValid())
		{// We're dealing with a prefab, spawn it
			newTile = SpawnPrefab(newTile, x, y);
		}

		newTile.name = $"Tile{x},{y}";

		if (newTile.transform.position != new Vector3(x, y))
			newTile.transform.position = new Vector3(x, y);

		newTile.transform.parent = root;

		// I would create a pool for it instead of destroying the object
		if (tiles[x, y] != null)
			Object.Destroy(tiles[x, y].gameObject);

		tiles[x, y] = newTile;

		return newTile;
	}
}
