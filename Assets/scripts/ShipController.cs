using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, Inspectable {
	
	public GameObject destination;
	[Range(0.0001f, 1000f)]
	public float thrust = 100f;

	private SteeringBasics steeringBasics;

	private bool _selected;
	private GameObject _infoPanel;

	public void Start() {
		steeringBasics = GetComponent<SteeringBasics>();
		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");
	}

	public void FixedUpdate () {
		Arrive (destination);

		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}

	private void Arrive(GameObject dest) {
		Vector3 accel = steeringBasics.arrive(dest.transform.position);

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

	public string ObjectInfo () {
		return "Hey I'm renderin ere!!\nName: " + name + "\nDestination: " + destination.name;
	}
}
