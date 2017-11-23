using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionNode : MonoBehaviour {

	[Range(1, 1000)]
	public float cycleTime = 3f;
	public bool producing = false;

	public StorageNode connectedStorageNode;

	void Update () {
		if (RequirementsMet () && !producing)
			StartCoroutine (Produce());
	}

	private bool RequirementsMet() {
		foreach (ResourceInput resource in GetComponentsInChildren<ResourceInput>()) {
			if (connectedStorageNode.HasResource (resource.type, resource.amount))
				continue;
			else
				return false;
		}

		return true;
	}

	private void DeductResources() {
		foreach (ResourceInput resource in GetComponentsInChildren<ResourceInput>())
			connectedStorageNode.Take(resource.type, resource.amount);
	}

	private IEnumerator Produce() {
		producing = true;
		DeductResources ();
		yield return new WaitForSeconds (cycleTime);
		foreach (ResourceOutput resource in GetComponentsInChildren<ResourceOutput>())
			connectedStorageNode.Put(resource.type, resource.amount);

		producing = false;
	}
}
