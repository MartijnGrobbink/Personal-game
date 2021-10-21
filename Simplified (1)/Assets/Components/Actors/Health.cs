using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	float maxHealth;
	public float MaxHealth
	{
		get { return maxHealth; }
		set { maxHealth = value; }
	}
	float healthModifier;
	float health;

	public UnityAction onDie;

	public void SetHealthModifiers(BrainModifiers modifiers)
	{
		healthModifier = modifiers.healthModifier;
		healthModifier = Random.Range(healthModifier * 0.9F, healthModifier * 1.1F);
		MaxHealth += modifiers.health;
		health = MaxHealth;
	}

	public void AdjustHealth(float amount, float damageModifier)
	{
		float modifier = Random.Range(damageModifier * 0.9F, damageModifier * 1.1F);

		health += amount * modifier;

		if (health <= 0)
			onDie?.Invoke();
		else if (health > MaxHealth)
			health = MaxHealth;
	}
}
