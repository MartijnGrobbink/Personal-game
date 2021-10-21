using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Spawner))]
public class GameManager : MonoBehaviour
{
	[Header("Management")]
	[SerializeField]
	RNGPasses passes;

	[Header("Room Generation")]
	[SerializeField]
	private MapCellInfo mapCellInfo;
	[SerializeField]
	private Tile[] backgroundTiles;

	Grid grid;

	[Header("Play settings")]
	[SerializeField]
	GameObject player;
	[SerializeField]
	Vector3 spawnOffset = new Vector3(0, 0, -0.00001F);
	CameraManager cameraManager;
	public UnityAction onUpdate;
	GameObject activePlayer;
	[SerializeField]
	int inGameUISceneBuildIndex;
	Text feedbackText;
	// Preprocessor directives allow for adapative code, especially in this case the #if preprocessor directive
	// I.e. you can have a #if PS4 #elseif XBOX360.. etc
	// They would also have different code. DEBUG is always active in Editor, but not in a build (can be enabled)
#if DEBUG
	float timeStamp;
	[SerializeField]
	float debugDisplayLength = 3F;
#endif

	[Header("Enemy settings")]
	[SerializeField]
	BrainLinkModifier[] brainWeaknessModifiers;
	[SerializeField]
	BrainLinkModifier[] brainStrengthModifiers;
	[SerializeField]
	BrainSettings[] brainSettings;

	private Type[] brains;
	public Type[] Brains
	{
		get { return brains; }
		set { brains = value; }
	}

	Spawner spawner;

	// Thse can probably be put into a class or struct for readability purposes

	private void Awake()
	{
		// Load the UI scene, if it hasn't been loaded already (Separation of Concerns)
		LoadInGameUI();
		// Please read the notice

		// Just imagine putting the code from this function here 4 times. 
		// Repetition can almost always be solved
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref

		brainSettings.OrderBy(x => x.weight);

		// Create the grid
		// Note: you might want to set the grid if you don't plan on creating the world, but still use rooms
		// I.e.: you can create rooms and let them behave like houses
		grid = new Grid();
		// Management for creation of Rooms

		spawner = gameObject.GetComponent<Spawner>();

	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/api/system.type?view=net-5.0
	/// </summary>
	/// <param name="name">Name to type</param>
	/// <returns>Found type (if any)</returns>
	private Type GetBrain(string name)
	{
		for (int i = 0; i < brains.Length; i++)
		{
			if (brains[i].ToString() == name)
				return brains[i];
		}

		return default;
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=net-5.0
	/// </summary>
	/// <param name="name">Type to be constructed</param>
	/// <returns>Type, if any</returns>
	public Brain CreateBrain(string name)
	{
		// Notice: this does break the design of Unity, this can be solved by using MonoBehaviour
		Type type = GetBrain(name);
		// Tells System that we should create a new copy (instance) of the existing type 
		return (Brain)Activator.CreateInstance(type);
	}

	private void Start()
	{
		// Get the camera manager (to follow the player)
		cameraManager = FindObjectOfType<CameraManager>();

#if DEBUG
		// Would be better if the object had the tag
		feedbackText = GameObject.Find("Feedback").GetComponent<Text>();

		DisplayDebugText("");

		// Creates a new function that only exists at this point, which is subscribed to onUpdate EventHandler
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
		onUpdate += () =>
		{
			if (Time.time > timeStamp + debugDisplayLength)
				DisplayDebugText("");

			// "Generate" the world
			if (Input.GetKeyDown(KeyCode.G))
				Generate();

			// Start "Horde"
			if (Input.GetKeyDown(KeyCode.H))
			{
				// Disables and enables it
				spawner.CanSpawn = !spawner.CanSpawn;
				//SpawnTimer timer = GetComponent<SpawnManager>().GetSpawnTimer();
				//timer.Start(GetPlayerRoom());
				//onUpdate += timer.Update;
			}
		};
#else
		feedbackText.transform.parent.gameObject.SetActive(false);
#endif
	}

	/// <summary>
	/// Ref:https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref
	/// Applies data to the modifer and returns it afterward
	/// </summary>
	/// <param name="modifier">the modifier to be altered and used upon return</param>
	public void GetStrengths(ref BrainModifiers modifier, int maxAmount)
	{
		GetModifiers(ref modifier, brainStrengthModifiers, maxAmount);
	}

	/// <summary>
	/// See above.
	/// </summary>
	/// <param name="modifier"></param>
	/// <param name="maxAmount"></param>
	public void GetWeaknesses(ref BrainModifiers modifier, int maxAmount)
	{
		GetModifiers(ref modifier, brainWeaknessModifiers, maxAmount);
	}

	public void GetModifiers(ref BrainModifiers modifier, BrainLinkModifier[] modifiers, int maxAmount)
	{
		// We have to add one because it's exclusive (the Random.Range)
		// i.e. if you had one it will roll from 0 to 0 (doesn't make sense)
		int amount = UnityEngine.Random.Range(0, maxAmount + 1);

		if (amount == 0)
			return;

		// It's better to only select entries that are possible instead of repeating
		for (int i = 0; i < amount; i++)
			modifier += modifiers.GetRandomEntry().modifier;
	}


	public Grid GetGrid()
	{
		return grid;
	}

	public void Generate(bool restart = true)
	{
		// Set everything correctly
		grid.Initialise(backgroundTiles, mapCellInfo.width, mapCellInfo.height);
		//TODO: I'd also remove the enemies here (that means you need to find them and get rid of them)
		// Enemies will persist on the map otherwise

		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives
#if DEBUG
		// Pass: Generate Background
		if (passes.generateGrid)
		{
			DisplayDebugText("Creating Grid");
			grid.GenerateMap(true);
		}

		if (restart && passes.spawnPlayer)
		{
			DisplayDebugText("Spawning player");
			StartPlay();
		}

#else
		// Level Pass

		// Pass: Generate Background
		if(passes.generateGrid)
			grid.GenerateMap(true);

		if (restart && passes.spawnPlayer)
			StartPlay();
#endif
	}

	private void StartPlay()
	{
		if (activePlayer)
			Destroy(activePlayer);

		float width = mapCellInfo.width * 0.5f;
		float height = mapCellInfo.height * 0.5f;

		// Generation is done, getting ready to play:
		Vector3 spawnPosition = new Vector3(width, height, 0);
		Camera.main.transform.position = spawnPosition + cameraManager.CameraOffset;

		// Spawn player here
		activePlayer = Instantiate(player, spawnPosition + spawnOffset, Quaternion.identity);
		activePlayer.name = player.name;
	}

	private void Update()
	{
		onUpdate?.Invoke();
	}

	public Tile GetTile(int x, int y)
	{
		return grid.GetTile(x, y);
	}

#if DEBUG
	public void DisplayDebugText(string text)
	{
		feedbackText.text = text;
		timeStamp = Time.time;
	}
#endif

	// Also have the UI in a different scene is a way of using Separation of Concerns
	private void LoadInGameUI()
	{
		if (!SceneManager.GetSceneByBuildIndex(inGameUISceneBuildIndex).isLoaded)
			SceneManager.LoadScene(inGameUISceneBuildIndex, LoadSceneMode.Additive);
	}
}

[System.Serializable]
public struct MapCellInfo
{
	public int width;
	public int height;
	public int minCellSize;
	public int minRoomSize;
}

[System.Serializable]
public struct RNGPasses
{
	public bool generateGrid;
	public bool spawnPlayer;
}