using UnityEngine;

public class Zombie : Brain
{
	// Tell the brain that we're taking over from here
	// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override
	public override void Execute()
	{
		// Still call the base behaviour, if there is any
		base.Execute();

		if (!IsActive)
			return;

		if (enemies.Count == 0 || !Attack())
			Move();
	}

	public override bool Attack()
	{
		if (!CanAttack(enemies[0]))
			return base.Attack();

		timeStamp = Time.time;
		DoDamage(enemies[0]);

		return true;
	}

	public override void Move()
	{
		if (enemies.Count == 0)
		{
			base.Move();
			return;
		}

		Vector3 direction = enemies[0].position - Owner.transform.position;
		Owner.Move(direction.normalized);
	}
}
