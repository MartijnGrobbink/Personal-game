using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Movement), typeof(NewHealth))]
public class PlayerInput : MonoBehaviour
{
	//getting horizontal and vertical this is needed for movement
	[SerializeField]
	string horizontal = "Horizontal";
	[SerializeField]
	string vertical = "Vertical";
	//getting other scripts
	//for looking at how much health the player has
	NewHealth newHealth;
	//for player movement
	Movement movement;
	//for shooting out magic
	Weapon weapon;
	//Later for disabaling the movement at the start of the game
	RoomTemplates roomTemplates;
	//delay for when you can fire
	float fireTimer;

	private void Start()
	{
		//finding all the components
		movement = GetComponent<Movement>();
		newHealth = GetComponent<NewHealth>();
		roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
		weapon = GetComponent<Weapon>();
		//setting speed modifier to 1
		movement.SpeedModifier = 1;
	}

	// Update is called once per frame
	void Update()
	{
		//activating move and fire
		Move();
		Fire();
		//if player has no health activate die
		if(newHealth.health <= 0)
		{
			Die();
		}
	}

	private void Move()
	{
		//setting x and y and then send them to the movement script
		float x = Input.GetAxis(horizontal);
		float y = Input.GetAxis(vertical);
	
		movement.Move(new Vector3(x, y));
	}

	private void Fire()
	{
	//if left mousbutton pressed fire wait until time is 0 and then you can fire again
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			if(fireTimer <= 0)
			{
				weapon.trigger = true;
				fireTimer = 1f;	
			}
		}
		if(fireTimer > 0)
		{
			fireTimer -= Time.deltaTime;
		}
	}
	//if player died remove gameobject and say in debug that player has died
	//There still has to be a death screen added
	private void Die()
	{
		Debug.Log($"Player {gameObject.GetInstanceID()} has died");
		Destroy(gameObject);
	}
}
