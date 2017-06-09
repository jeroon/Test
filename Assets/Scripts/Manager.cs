using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Manager : MonoBehaviour
{
	public static Manager ThisManager;

	[SerializeField] GameObject panelFolder = null;
	[SerializeField] GameObject animalPrefab = null;
	[SerializeField] GameObject foodPrefab = null;
	[SerializeField] Image panelPrefab = null;
	[SerializeField] Camera cam = null;
	[SerializeField] int spaceBetween = 5;
	[SerializeField] int numberOfStartMobs = 5;
	[SerializeField] int arraySize = 10;
	[SerializeField] GameObject mobsParentFolder = null;
	[SerializeField] Text textLabel = null;

	public EventHandler AgeTickHandler;
	public List<Mob> Ready = new List<Mob>();

	private Mob[,] positions;
	private int counter = 0;
	private System.Random random;
	private int maxCount = 0;
	private float evoFactor = .1f;
	private int maxHealthInitial;
	private int maxAgeInitial;

	private void Awake()
	{
		if (ThisManager == null) ThisManager = this;
		positions = new Mob[arraySize, arraySize];
		random = new System.Random();
	}
	void Start()
	{
		CreateFood();
		for (int i = 0; i < numberOfStartMobs; i++)
		{
			CreateAnimal(null, null);
		}
		maxHealthInitial = 40;
		maxAgeInitial = 100;

	}
	void Update()
	{
		if (counter % 20 == 0 && counter != 0)
		{
			if (AgeTickHandler != null) AgeTickHandler(this, null);
			counter = 0;
		}
		counter++;
		if(Ready.Count >=2)
		{
			CreateAnimal(Ready[0], Ready[1]);
			Ready.Remove(Ready[0]);
			Ready.Remove(Ready[0]);
		}
		int counter2 = 0;
		foreach (Animal animal in positions)
		{
			if (animal != null)
			{
				Vector3 screenPos = cam.WorldToScreenPoint(animal.transform.position);
				animal.Panel.transform.position = screenPos;
				counter2++;
			}
		}
		if (counter2 > maxCount) maxCount = counter2;
		textLabel.text = "count: " + counter2 + Environment.NewLine + "Max:	" + maxCount;
	}


	private void CreateAnimal(Mob mobA, Mob mobB)
	{
		GameObject go = Instantiate(animalPrefab);
		go.transform.parent = mobsParentFolder.transform;
		Point p = GetFreePosition();
		if (p == null)
		{
			print("array is full");
			Destroy(go);
			EditorApplication.isPaused = true;
			return;
		}
		go.transform.position = new Vector3(p.X * spaceBetween, 0, p.Y * spaceBetween);
		Animal animal = go.GetComponent<Animal>();
		positions[p.X, p.Y] = animal;
		animal.Panel = Instantiate(panelPrefab).GetComponent<Image>();
		animal.Panel.transform.parent = panelFolder.transform;
		animal.Panel.name = "AnimalPanel";

		Vector3 screenPos = cam.WorldToScreenPoint(animal.transform.position);
		animal.Panel.transform.position = screenPos;
		int maxHealth;
		if (mobA == null || mobB == null)
			maxHealth = maxHealthInitial;
		 else
			maxHealth = Math.Max(mobA.MaxHealth, mobB.MaxHealth);
		if (random.NextDouble() <= evoFactor)
			animal.SetMaxHealth(maxHealth +	1);
		else
			animal.SetMaxHealth(maxHealth);

		int maxAge;
		if (mobA == null || mobB == null)
			maxAge = maxAgeInitial;
		else
			maxAge = Math.Max(mobA.MaxAge, mobB.MaxAge);
		print("Max:" + maxAge);
		if (random.NextDouble() <= evoFactor)
			animal.SetMaxAge(maxAge + 1);
		else
			animal.SetMaxAge(maxAge);

		animal.DiedHandler += new EventHandler(MobDied);
	}

	private void CreateFood()
	{
		GameObject go = Instantiate(foodPrefab);
		go.transform.position = new Vector3(-spaceBetween, 0, 0);
		Food food = go.GetComponent<Food>();
		food.Panel = Instantiate(panelPrefab).GetComponent<Image>();
		food.Panel.transform.parent = panelFolder.transform;
		food.Panel.name = "FoodPanel";
		Vector3 screenPos = cam.WorldToScreenPoint(go.transform.position);
		food.Panel.transform.position = screenPos;
	}

	private void MobDied(object sender, EventArgs e)
	{
		for (int j = 0; j < positions.GetLength(1); j++)
		{
			for (int i = 0; i < positions.GetLength(0); i++)
			{
				if(positions[i,j] == (Mob)sender)
				{
					positions[i, j] = null;
					return;
				}
			}
		}
	}

	private Point GetFreePosition()
	{
		for (int j = 0; j < positions.GetLength(1); j++)
		{
			for (int i = 0; i < positions.GetLength(0); i++)
			{
				if (positions[i, j] == null) return new Point(i, j);
			}
		}
		return null;
	}


}
public class Point
{
	public int X { get; set; }
	public int Y { get; set; }

	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
}