using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, Inspectable {

	public StarSystemManager currentSystem;
	public GameObject destination = null;
	[Range(0.0001f, 1000f)]
	public float thrust = 100f;

	private SteeringBasics steeringBasics;

	private bool _selected;
	private GameObject _infoPanel;

	public void Start() {
		currentSystem = FindObjectOfType<StarSystemManager> (); //TODO: find a way to do this better

		steeringBasics = GetComponent<SteeringBasics>();
		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");

		StartCoroutine(GetRandomDestination());
	}

	public void FixedUpdate () {
		Arrive (destination);

		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}

	private IEnumerator GetRandomDestination() {
		yield return new WaitForSeconds (0.1f);
		destination = currentSystem.Planets [Random.Range (0, currentSystem.Planets.Count)];
		steeringBasics.slowRadius = (destination.GetComponent<SpriteRenderer> ().size.x + destination.GetComponent<SpriteRenderer> ().size.y) / 2;
	}

	private void Arrive(GameObject dest) {
		Vector2 accel = new Vector2(0, 0); // TODO: Fix this up, we're creating a LOT of new Vector2s here...

		if (destination != null)
			accel = steeringBasics.arrive (dest.transform.position);
		else
			GetRandomDestination ();

		steeringBasics.steer(accel);
		steeringBasics.lookWhereYoureGoing();
	}

	public void OnMouseOver() {
		_selected = true;
		FindObjectOfType<InputController> ().inspectableSelected = true;
	}

	public void OnMouseExit() {
		_selected = false;
		FindObjectOfType<InputController> ().inspectableSelected = false;
	}

	public void OnMouseDown () {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.SendMessage ("SetFollow", gameObject);
	}

	public void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject == destination)
			StartCoroutine(GetRandomDestination());
	}

	public string ObjectInfo () {
		return "Hey I'm renderin ere!!\nName: " + name + "\nDestination: " + destination.name;
	}
}
