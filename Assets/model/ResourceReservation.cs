using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceReservation
{
	public Resource resource;
    public StorageNode location;

	public ResourceReservation(Resource resource, StorageNode location)
    {
		this.resource = resource;
        this.location = location;
	}

	public bool Matches(ResourceType type, int amount, StorageNode reserver)
    {
		return resource.type == type && resource.amount == amount;
	}

	public void Resolve(StorageNode destinationNode)
    {
        location.TransferReservedResources(this, destinationNode);
    }
}
