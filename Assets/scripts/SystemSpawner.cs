using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSpawner : MonoBehaviour {

	public GameObject planet;
	public Transform star;
	[Range(1f, 3000f)]
	public float planetSeperationDistance = 5;
	[Range(0, 1000)]
	public int planetSpawnLimit = 10;
	[Range(0.001f, 1f)]
	public float moonDistance = 0.01f;

	private string[] _spriteNames = {"planet_34", "planet_35", "planet_36", "planet_37",
		"planet_38", "planet_39", "planet_40", "planet_41", "planet_42", "planet_43",
		"planet_44", "planet_45", "planet_46", "planet_47", "planet_48"
	};

	public Sprite[] possibleSprites;

	public void Start () {
		int numOfPlanets = Random.Range (1, planetSpawnLimit);

		for (int i = 0; i < numOfPlanets; i++)
			SpawnPlanet (i, numOfPlanets);
	}

	private void SpawnPlanet(int planetNumber, int maxPlanets) {
		Vector2 pos = Random.insideUnitCircle;
		pos.Normalize ();
		pos *= (planetSeperationDistance * (planetNumber + 1));

		GameObject newPlanet = Instantiate(planet, transform);
		newPlanet.transform.parent = star;
		newPlanet.transform.Translate (pos);
		GiveSprite (newPlanet);

		if (Random.Range (0, 5) == 0)
			SpawnMoon (newPlanet);
	}

	private void GiveSprite(GameObject newPlanet) {
		newPlanet.GetComponent<SpriteRenderer> ().sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
	}

	private void SpawnMoon(GameObject parentPlanet) {
		Vector2 pos = Random.insideUnitCircle;
		pos.Normalize ();
		pos *= moonDistance;

		GameObject moon = Instantiate (planet, parentPlanet.transform);
		moon.transform.parent = parentPlanet.transform;
		moon.transform.Translate (new Vector3 (Random.Range (10, 30), Random.Range (10, 30), 0));
		GiveSprite (moon);

		moon.transform.localScale /= 2;
	}
}
