using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSpawner : MonoBehaviour {

	public GameObject planet;
	public Transform star;
	[Range(50, 3000)]
	public float spawnLimit = 500;
	[Range(0, 10)]
	public int planetSpawnLimit;

	public Sprite[] possibleSprites;

	public void Start () {
		int numOfPlanets = Random.Range (1, planetSpawnLimit);

		for (int i = 0; i < numOfPlanets; i++)
			SpawnPlanet ();
	}

	private void SpawnPlanet() {
		GameObject newPlanet = Instantiate(planet, transform.position, Quaternion.identity);
		newPlanet.transform.parent = star;
		newPlanet.transform.Translate (RandomiseStartingPosition());
		GiveSprite (newPlanet);
	}

	private Vector3 RandomiseStartingPosition() {
		float x = Random.Range (-spawnLimit, spawnLimit + 1);
		float y = Random.Range (-spawnLimit, spawnLimit + 1);

		if (Mathf.Abs (x) < 30)
			y += 30;
		if (Mathf.Abs (y) < 30)
			x += 30;

		return new Vector3 (x, y, 0);
	}

	private void GiveSprite(GameObject planet) {
		planet.GetComponent<SpriteRenderer> ().sprite = possibleSprites[Random.Range(0, possibleSprites.Length)];
	}
}
