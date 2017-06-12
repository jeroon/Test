using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{

	public static Food ThisFood;

	public static int Amount { get; private set; }
	public Image Panel { get; set; }

	private int maxAmount = 0;
	private Text panelText;
	public static int amountToEat;

	private void Awake()
	{
		if (ThisFood == null)
			ThisFood = this;
		// 
	}
	void Start()
	{
		maxAmount = 200;
		amountToEat = 1;
		panelText = Panel.GetComponentInChildren<Text>();
	}
	void Update()
	{
		if (Amount < maxAmount)
			Amount++;
		panelText.text = "Amount: " + Amount;
	}


	public int TryToEat()
	{
		if (Amount >= amountToEat)
		{
			Amount -= amountToEat;
			return amountToEat;
		}
		return 0;
	}
}