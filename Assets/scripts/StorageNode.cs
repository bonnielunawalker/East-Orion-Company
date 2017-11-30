using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageNode : IndustryNode
{

	[SerializeField]
	public List<Resource> resources = new List<Resource> ();
	public Dictionary<StorageNode, ResourceReservation> reservations = new Dictionary<StorageNode, ResourceReservation>();
	public int maxUnits;
	public int usedCapacity = 0;
	public Resource template;

	public void Start()
    {
		employmentData = GetComponent<Employee> ();
		connectedStorageNode = this;
	}

	public int CapacityRemaining()
    {
		return maxUnits - usedCapacity;
	}

	public override bool SuppliesResource(ResourceType type)
    {
		return resources.Exists (r => r.type == type);
	}

	public override bool HasResourceAmount(ResourceType type, int amount)
    {
		foreach (Resource r in resources)
			if (r.type == type && r.amount >= amount)
				return true;

		return false;

		//return resources.Exists (r => r.type == type && r.amount >= amount);
	}

	public Resource Take(Resource resource)
    {
		foreach (Resource r in resources)
			if (r.type == resource.type)
            {
				r.amount -= resource.amount;

				// Destroy this resource pool if no resources remain.
				if (r.amount == 0)
					resources.Remove (r);

				return resource;
			}

		Debug.LogError ("No resource could be removed");
		return null; // TODO: Make this throw an error of some sort instead of returning a null object.
	}

	public void Put(Resource resource)
    {
		if (resource.amount > CapacityRemaining())
        {
			Debug.LogError ("Trying to put " + resource.amount + " of " + System.Enum.GetName(typeof(ResourceType), resource.type) + " when only " + CapacityRemaining() + " units of space remain.");
			return;
		}

		foreach (Resource r in resources)
			if (r.type == resource.type)
            {
				r.amount += resource.amount;
				return;
			}
					
		// Create a new resource pool of this type doesn't exist in this node's storage.
		resources.Add (resource);
	}

	public void TransferResources(StorageNode to, Resource resource)
    {
		int storedAmount = QueryAmount (resource.type);

		if (storedAmount >= resource.amount)
			to.Put(Take (resource));
		else
			Debug.LogWarning("Not enough units of " + System.Enum.GetName(typeof(ResourceType), resource.type) + ". " + resource.amount + " requested, only " + storedAmount + " stored.");
	}

	public int QueryAmount(ResourceType type)
    {
		if (resources.Exists (resource => resource.type == type))
			return resources.Find (resource => resource.type == type).amount;
		else
			return 0;
	}

	public ResourceReservation MakeReservation (StorageNode reserver, Resource resource)
    {
		ResourceReservation newReservation = new ResourceReservation (reserver, Take(resource), this);
		reservations.Add(reserver, newReservation);
		return newReservation;
	}

	public void CancelReservation(ResourceReservation reservation)
    {
		Put (reservation.resource);
		reservations.Remove (reservation.reserver);
	}

	public List<ResourceReservation> FindReservations(StorageNode reserver)
    {
		List<ResourceReservation> result = new List<ResourceReservation> ();

		foreach (KeyValuePair<StorageNode, ResourceReservation> kvp in reservations)
			if (kvp.Key == reserver)
				result.Add (kvp.Value);

		return result;
	}

	public bool HasReservation(StorageNode reserver)
    {
		return reservations.ContainsKey (reserver);
	}

	public void TransferReservedResources(ResourceReservation reservation)
    {
		reservations.Remove (reservation.reserver);
		reservation.reserver.Put (reservation.resource);
	}

	public override string ObjectInfo()
    {
		string result = "StorageNode\n";

		foreach (Resource resource in resources)
			result += "\tType: " + resource.type.ToString() + "\n\tAmount: " + resource.amount + "\n";

		return result + "\n";
	}
}
