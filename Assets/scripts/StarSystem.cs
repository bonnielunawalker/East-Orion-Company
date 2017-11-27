using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystem : MonoBehaviour, IInspectable {

	private bool _selected = false;
	private GameObject _infoPanel;
	private List<Planet> _planets;

	public List<Planet> Planets {
		get { return _planets; }
	}

	private JobBoard _jobBoard;

	public JobBoard JobBoard {
		get { return _jobBoard; }
	}

	public GameObject planetTemplate;
	public GameObject shipTemplate;
	public GameObject storageNodeTemplate;
	public GameObject[] resourceNodeTemplates = { };
	public GameObject[] factoryTemplates = { };

	public Transform star;
	[Range(1f, 3000f)]
	public float planetSeperationDistance = 50;
	[Range(0, 1000)]
	public int planetSpawnMax = 10;
	[Range(0, 1000)]
	public int planetSpawnMin = 0;
	[Range(1f, 100f)]
	public float moonDistance = 7f;
	[Range(0, 500)]
	public int numOfShips = 20;

	public Sprite[] possiblePlanetSprites;

	public Sprite[] possibleShipSprites;

	private string[] _starNames = {
		"Archimedes",
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
		"Copernicus",
		"Descartes",
		"Pericles"
	};

	public void Start () {
		name = _starNames[Random.Range(0, _starNames.Length)];
		_planets = new List<Planet> ();
		star = gameObject.transform;

		_jobBoard = new JobBoard (this);

		int numOfPlanets = Random.Range (planetSpawnMin, planetSpawnMax);

		for (int i = 0; i < numOfPlanets; i++)
			SpawnPlanet (i, numOfPlanets);

		for (int i = 0; i < numOfShips; i++)
			SpawnShip ();

		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");
	}

	public void Update() {
		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}
		
	private void SpawnShip() {
		Vector2 pos = new Vector2 (Random.Range (0, planetSeperationDistance), Random.Range (0, planetSeperationDistance)) + (Vector2)star.position;
		GameObject newShip = Instantiate (shipTemplate);
		newShip.GetComponent<SpriteRenderer> ().sprite = possibleShipSprites [Random.Range (0, possibleShipSprites.Length)];
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

		// Add resource and production nodes to the new planet.
		ResourceNode newRNode = Instantiate (resourceNodeTemplates[Random.Range(0, resourceNodeTemplates.Length)], newPlanet.transform).GetComponent<ResourceNode>();
		ProductionNode newPNode = Instantiate (factoryTemplates[Random.Range(0, factoryTemplates.Length)], newPlanet.transform).GetComponent<ProductionNode>();
		StorageNode newSNode = Instantiate (storageNodeTemplate, newPlanet.transform).GetComponent<StorageNode>();

		// Connect the industry nodes to the storage node.
		newRNode.connectedStorageNode = newSNode;
		newPNode.connectedStorageNode = newSNode;

		Planet planetScript = newPlanet.GetComponent<Planet> () as Planet;
		planetScript.industryNodes.Add (newRNode);
		planetScript.industryNodes.Add (newPNode);
		planetScript.industryNodes.Add (newSNode);

		_planets.Add (planetScript);

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

		GameObject newMoon = Instantiate (planetTemplate, parentPlanet.transform);
		newMoon.transform.parent = parentPlanet.transform;
		newMoon.transform.Translate (pos);

		newMoon.GetComponent<SpriteRenderer> ().sprite = possiblePlanetSprites [Random.Range (3, 5)];

		newMoon.transform.localScale /= 2;

		newMoon.name = parentPlanet.gameObject.name + " moon";

		// Add resource and production nodes to the new moon.
		ResourceNode newRNode = Instantiate (resourceNodeTemplates[Random.Range(0, resourceNodeTemplates.Length)], newMoon.transform).GetComponent<ResourceNode>();
		ProductionNode newPNode = Instantiate (factoryTemplates[Random.Range(0, factoryTemplates.Length)], newMoon.transform).GetComponent<ProductionNode>();
		StorageNode newSNode = Instantiate (storageNodeTemplate, newMoon.transform).GetComponent<StorageNode>();

		// Connect the industry nodes to the storage node.
		newRNode.connectedStorageNode = newSNode;
		newPNode.connectedStorageNode = newSNode;

		Planet planetScript = newMoon.GetComponent<Planet> () as Planet;
		planetScript.industryNodes.Add (newRNode);
		planetScript.industryNodes.Add (newPNode);
		planetScript.industryNodes.Add (newSNode);

		_planets.Add (newMoon.GetComponent<Planet>());
	}

	public FreightJob TakeFreightJob() {
		foreach (Contract j in _jobBoard.Jobs) {
			if (j.GetType() == typeof(FreightJob)) {
				_jobBoard.Jobs.Remove (j);
				return j as FreightJob;
			}
		}

		return null;
	}

	public void RemoveJob(Contract job) {
		if (_jobBoard.Jobs.Contains (job))
			_jobBoard.Jobs.Remove (job);
		else
			Debug.LogError ("Job board does not contain this job!");
	}

	public void OnMouseOver() {
		_selected = true;
		FindObjectOfType<InputController> ().inspectableSelected = true;
	}

	public void OnMouseExit() {
		_selected = false;
		FindObjectOfType<InputController> ().inspectableSelected = false;
	}

	public string ObjectInfo () {
		string result = "Name: " + gameObject.name + "\nJobs in system: ";

		result += _jobBoard.Jobs.Count;

		return result;
	}
}
