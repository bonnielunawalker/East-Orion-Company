using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearSensor : MonoBehaviour {

	public HashSet<Rigidbody2D> targets = new HashSet<Rigidbody2D>();

	void OnTriggerEnter(Collider other) {
		targets.Add (other.GetComponent<Rigidbody2D>());
	}
	
	void OnTriggerExit(Collider other) {
		targets.Remove (other.GetComponent<Rigidbody2D>());
	}
}
