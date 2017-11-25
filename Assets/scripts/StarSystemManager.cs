using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemManager : MonoBehaviour {

	private List<GameObject> _planets;

	public List<GameObject> Planets {
		get { return _planets; }
	}

	public GameObject planetTemplate;
	public GameObject shipTemplate;

	public Transform star;
	[Range(1f, 3000f)]
	public float planetSeperationDistance = 50;
	[Range(0, 1000)]
	public int planetSpawnLimit = 5;
	[Range(1f, 100f)]
	public float moonDistance = 7f;

	public Sprite[] possiblePlanetSprites;

	public Sprite[] possibleShipSprites;

	private string[] _starNames = {
		"Archimedes",
		"Da Vinci",
		"Plato",
		"Socrates",
		"Pasteur",
		"Galileo",
		"Kepler",
		"Gauss",
		"Curie",
		"Joule",
		"Olympus",
		"Hades",
		"Copernicus"
	};



	public void Start () {
		name = _starNames[Random.Range(0, _starNames.Length)];
		_planets = new List<GameObject> ();
		star = gameObject.transform;

		int numOfPlanets = Random.Range (2, planetSpawnLimit);

		for (int i = 0; i < numOfPlanets; i++)
			SpawnPlanet (i, numOfPlanets);

		for (int i = 0; i < 20; i++)
			SpawnShip ();
	}
		
	private void SpawnShip() {
		Vector2 pos = new Vector2 (Random.Range (0, planetSeperationDistance), Random.Range (0, planetSeperationDistance));
		GameObject newShip = Instantiate (shipTemplate);
		newShip.GetComponent<SpriteRenderer> ().sprite = possibleShipSprites [Random.RandomRange (0, possibleShipSprites.Length)];
		newShip.transform.position = pos;

		SteeringBasics sb = newShip.GetComponent<SteeringBasics> ();
		sb.maxVelocity += Random.Range(-(sb.maxVelocity / 5), sb.maxVelocity / 5);
		sb.maxAcceleration += Random.Range(-(sb.maxAcceleration / 5), sb.maxAcceleration / 5);
	}

	private void SpawnPlanet(int planetNumber, int maxPlanets) {
		Vector2 pos = Random.insideUnitCircle;
		pos.Normalize ();
		pos *= (planetSeperationDistance * (planetNumber + 1));

		GameObject newPlanet = Instantiate(planetTemplate, transform);
		newPlanet.transform.parent = star;
		newPlanet.transform.Translate (pos);

		GiveSprite (newPlanet);
		newPlanet.name = name + " " + (planetNumber + 1);
		// Set this after the sprite to ensure the collider fits the sprite.
		_planets.Add (newPlanet);

		if (Random.Range (0, 5) == 0)
			SpawnMoon (newPlanet);
	}

	private void GiveSprite(GameObject newPlanet) {
		SpriteRenderer renderer = newPlanet.GetComponent<SpriteRenderer> ();
		renderer.sprite = possiblePlanetSprites[Random.Range(0, possiblePlanetSprites.Length)];

		CircleCollider2D collider = newPlanet.GetComponent<CircleCollider2D> ();
		Vector3 spriteHalfSize = renderer.sprite.bounds.extents;
		collider.radius = spriteHalfSize.x > spriteHalfSize.y ? spriteHalfSize.x : spriteHalfSize.y;
	}

	private void SpawnMoon(GameObject parentPlanet) {
		Vector2 pos = Random.insideUnitCircle;
		pos.Normalize ();
		pos *= moonDistance;

		GameObject moon = Instantiate (planetTemplate, parentPlanet.transform);
		moon.transform.parent = parentPlanet.transform;
		moon.transform.Translate (pos);

		moon.GetComponent<SpriteRenderer> ().sprite = possiblePlanetSprites [Random.Range (3, 5)];

		moon.transform.localScale /= 2;

		moon.name = parentPlanet.gameObject.name + " moon";

		_planets.Add (moon);
	}
}
