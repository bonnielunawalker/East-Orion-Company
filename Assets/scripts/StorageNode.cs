using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : MonoBehaviour {

	public Resource template;

	public bool HasResource(ResourceType resource, int amount) {
		// Loop through all resources that are children of this storage node and check if they match the type and have the required amount.
		foreach (Resource r in gameObject.GetComponentsInChildren<Resource>()) {
			if (r.type == resource && r.amount >= amount)
				return true;
		}
			

		return false;
	}

	public void Take(ResourceType resource, int amount) {
		foreach (Resource r in gameObject.GetComponentsInChildren<Resource>()) {
			if (r.type == resource) {
				r.amount -= amount;

				// TODO: Check if we've got no more resources of this type left and clean up if so.
				if (r.amount == 0)
					Destroy(r.gameObject);

				return;
			}
		}
	}

	public void Put(ResourceType resource, int amount) {
		foreach (Resource r in gameObject.GetComponentsInChildren<Resource>()) {
			if (r.type == resource) {
				r.amount += amount;
				return;
			}
		}
					
		// Create a new resource if one of this type doesn't exist in this node's storage.
		Resource newResource = Instantiate(template, gameObject.transform);
		newResource.amount = amount;
		newResource.type = resource;
	}

	public void Update() {

	}
}
