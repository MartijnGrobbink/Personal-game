using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement)/*, typeof(Health)*/)]
public class PlayerInput : MonoBehaviour
{
	// Tip: You can probably use the new Input System instead of this...
	[SerializeField]
	string horizontal = "Horizontal";
	[SerializeField]
	string vertical = "Vertical";
	[SerializeField]
	string fireButton;

	//[SerializeField]
	//float maxHealth = 100;
	//Health health;
	Movement movement;

	//public UnityAction onFire;
	//CameraManager cameraManager;

	private void Start()
	{
		movement = GetComponent<Movement>();
		movement.SpeedModifier = 1;
		
		//cameraManager = FindObjectOfType<CameraManager>();
		//health = GetComponent<Health>();
		//health.MaxHealth = maxHealth;
		//health.onDie += Die;
		// Tell the CameraManager that we're going to be followed
		//cameraManager.AddPlayer(transform);
	}

	// Update is called once per frame
	void Update()
	{
		Move();
		//Fire();
	}

	private void Move()
	{
		float x = Input.GetAxis(horizontal);
		float y = Input.GetAxis(vertical);
	
		movement.Move(new Vector3(x, y));
	}

	/*private void Fire()
	{
		// Doesn't do anything currently, you can add a weapon class and then subscribe to "onFire"
		if(Input.GetButton(fireButton))
			onFire?.Invoke();
	}

	private void OnDisable()
	{
		// Tell the CameraManager that this object is gone and doesn't need to be tracked anymore
		//cameraManager.RemovePlayer(transform);
	}

	private void Die()
	{
		Debug.Log($"Player {gameObject.GetInstanceID()} has died");
		Destroy(gameObject);
	}*/
}
