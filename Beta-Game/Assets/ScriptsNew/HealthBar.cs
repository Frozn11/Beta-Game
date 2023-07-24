using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	public Slider slider;
	public Gradient gradient;
	private PlayerMovementGrappling PlayerHp;

	private void Start()
	{
		PlayerHp = GetComponent<PlayerMovementGrappling>();
	}

	public void SetMaxHealth(float health)
	{
		slider.maxValue = health;
		slider.value = health;
	}

    public void SetHealth(int health)
	{
		slider.value = health;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Death")
		{
			PlayerHp.currentHealth = 0;
			PlayerHp.maxHalth = 0;
		}
	}

}
