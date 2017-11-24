using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : MonoBehaviour, IndustryNode {

	[SerializeField]
	public List<Resource> resources = new List<Resource> ();

	public Resource template;

	public bool HasResource(ResourceType resource, int amount) {
		// Loop through all resources that are children of this storage node and check if they match the type and have the required amount.
		foreach (Resource r in resources) {
			if (r.type == resource && r.amount >= amount)
				return true;
		}
			

		return false;
	}

	public void Take(ResourceType resource, int amount) {
		foreach (Resource r in resources) {
			if (r.type == resource) {
				r.amount -= amount;

				// Destroy this resource pool if no resources remain.
				if (r.amount == 0)
					resources.Remove (r);

				return;
			}
		}
	}

	public void Put(ResourceType resource, int amount) {
		foreach (Resource r in resources) {
			if (r.type == resource) {
				r.amount += amount;
				return;
			}
		}
					
		// Create a new resource pool of this type doesn't exist in this node's storage.
		resources.Add (new Resource(resource, amount));
	}

	public string ObjectInfo() {
		string result = "StorageNode\n";

		foreach (Resource resource in resources)
			result += "\tType: " + resource.type.ToString() + "\n\tAmount: " + resource.amount + "\n";

		return result + "\n";
	}
}
