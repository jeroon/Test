using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Mob : MonoBehaviour {

	public int Health { get; set; }
	public int MaxHealth { get; private set; }
	public int Age { get; private set; }
	public int MaxAge { get; private set; }

	public int Height { get; set; }
	public int Strength { get; set; }

	public EventHandler DiedHandler;

	public Point position;

	public Image Panel { get; set; }
	private Text panelText;
	private string text;


	protected virtual void Start ()
	{
		Manager.ThisManager.AgeTickHandler += new EventHandler(AgeTick);
		panelText = Panel.GetComponentInChildren<Text>();
		Age = 0;
		Health = MaxHealth/2;
	}
	protected virtual void Update ()
	{
		Eat();
		UpdateText();

	}
	protected virtual void OnDestroy()
	{
		if(Panel != null)
			Destroy(Panel.gameObject);
		Manager.ThisManager.AgeTickHandler -= new EventHandler(AgeTick);
		if(DiedHandler!=null) DiedHandler(this, null);
	}


	private void UpdateText()
	{
		text = "";
		text += "Age: " + Age + "/ " + MaxAge + Environment.NewLine;
		text += "Health: " + Health + "/ " + MaxHealth + Environment.NewLine;
		text += "Height: " + Height + Environment.NewLine;
		text += "Strenght: " + Strength + Environment.NewLine;
		panelText.text = text;
	}
	private void Eat()
	{
		if(Health < MaxHealth)
		{
			Health += Food.ThisFood.TryToEat();
		}
	}
	private void CheckHealth()
	{
		if (Health <= 0) MobDied();
	}
	protected void AgeTick(object sender, EventArgs e)
	{
		Age++;
		if (Age>20 && Age % 10 == 0)
		{
			Manager.ThisManager.Ready.Add(this);
		}
		if (Age >= MaxAge)
			MobDied();
		Health--;
		CheckHealth();
		
	}

	public void TakeDamage(int amount)
	{
		Health -= amount;
		if(Health <= 0)
		{
			MobDied();
			Health = 0;
		}
	}

	private void MobDied()
	{
		Destroy(gameObject);
	}

	public void SetMaxHealth(int maxHealth)
	{
		MaxHealth = maxHealth;
	}
	public void SetMaxAge(int maxAge)
	{
		MaxAge = maxAge;
	}
}