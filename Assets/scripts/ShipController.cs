using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {
	
	public GameObject destination;
	[Range(0.0001f, 1000f)]
	public float thrust = 100f;
	[Range(0.0001f, 100f)]
	public float maxForce = 1f;

	private SteeringBasics steeringBasics;

	public void Start() {
		steeringBasics = GetComponent<SteeringBasics>();
	}

	public void FixedUpdate () {
		Arrive (destination);
	}

	private void Arrive(GameObject dest) {
		Vector3 accel = steeringBasics.arrive(dest.transform.position);

		steeringBasics.steer(accel);
		steeringBasics.lookWhereYoureGoing();
	}

	public void OnMouseDown () {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.SendMessage ("SetFollow", gameObject);
	}
}
