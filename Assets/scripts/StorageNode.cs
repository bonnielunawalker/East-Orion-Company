using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : MonoBehaviour, IndustryNode {

	[SerializeField]
	public List<Resource> resources = new List<Resource> ();
	public List<ResourceReservation> reservations = new List<ResourceReservation> ();
	public int maxUnits;
	public int usedCapacity = 0;
	public Resource template;

	public StorageNode ConnectedStorageNode { 
		get { return this; }
	}

	public int CapacityRemaining() {
		return maxUnits - usedCapacity;
	}

	public bool HasResource(ResourceType resource, int amount) {
		// Loop through all resources that are children of this storage node and check if they match the type and have the required amount.
		foreach (Resource r in resources) {
			if (r.type == resource && r.amount >= amount)
				return true;
		}
			

		return false;
	}

	public Resource Take(Resource resource) {
		foreach (Resource r in resources) {
			if (r.type == resource.type) {
				r.amount -= resource.amount;

				// Destroy this resource pool if no resources remain.
				if (r.amount == 0)
					resources.Remove (r);

				return resource;
			}
		}

		Debug.LogError ("No resource could be removed");
		return null; // TODO: Make this throw an error of some sort instead of returning a null object.
	}

	public void Put(Resource resource) {
		if (resource.amount > CapacityRemaining()) {
			Debug.LogError ("Trying to put " + resource.amount + " of " + System.Enum.GetName(typeof(ResourceType), resource.type) + " when only " + CapacityRemaining() + " units of space remain.");
			return;
		}

		foreach (Resource r in resources) {
			if (r.type == resource.type) {
				r.amount += resource.amount;
				return;
			}
		}
					
		// Create a new resource pool of this type doesn't exist in this node's storage.
		resources.Add (resource);
	}

	public void TransferResources(StorageNode to, Resource resource) {
		int storedAmount = QueryAmount (resource.type);

		if (storedAmount >= resource.amount)
			to.Put(Take (resource));
		else
			Debug.LogWarning("Not enough units of " + System.Enum.GetName(typeof(ResourceType), resource.type) + ". " + resource.amount + " requested, only " + storedAmount + " stored.");
	}


	public void TransferReserved(StorageNode to, ResourceReservation reservation) {
		ReturnReservationToPool (reservation);
		TransferResources (to, reservation.resource);
	}

	private void ReturnReservationToPool(ResourceReservation reservation) {
		Put (reservation.resource);
		reservations.Remove (reservation);
	}

	public bool SuppliesResource(ResourceType type) {
		return resources.Exists (resource => resource.type == type);
	}

	public int QueryAmount(ResourceType type) {
		if (resources.Exists (resource => resource.type == type))
			return resources.Find (resource => resource.type == type).amount;
		else
			return 0;
	}

	public ResourceReservation ReserveResources(Resource resource, GameObject reserver) {
		Debug.Log ("Reserving " + resource.amount + " units of " + System.Enum.GetName (typeof(ResourceType), resource.type));
		ResourceReservation newReservation = new ResourceReservation (Take (resource), reserver);
		reservations.Add (newReservation);
		return newReservation;
	}

	public bool ReservationExists(ResourceType type, int amount, GameObject reserver) {
		foreach (ResourceReservation reservation in reservations) {
			if (reservation.Matches (type, amount, reserver)) {
				return true;
			}
		}

		Debug.Log ("No reservation matching the request was found.");
		return false;
	}

	// Overload that takes a resource instead of type and amount.
	public bool ReservationExists(Resource resource, GameObject reserver) {
		return ReservationExists (resource.type, resource.amount, reserver);
	}

	public ResourceReservation GetReservation (Resource resource, GameObject reserver) {
		return reservations.Find(reservation => reservation.Matches(resource.type, resource.amount, reserver));
	}

	public string ObjectInfo() {
		string result = "StorageNode\n";

		foreach (Resource resource in resources)
			result += "\tType: " + resource.type.ToString() + "\n\tAmount: " + resource.amount + "\n";

		return result + "\n";
	}
}
