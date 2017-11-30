using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceReservation
{
	public StorageNode reserver;
	public Resource resource;
    public StorageNode location;

	public ResourceReservation(StorageNode reserver, Resource resource, StorageNode location)
    {
		this.reserver = reserver;
		this.resource = resource;
        this.location = location;
	}

	public bool Matches(ResourceType type, int amount, StorageNode reserver)
    {
		return this.reserver == reserver && resource.type == type && resource.amount == amount;
	}

    public void Resolve()
    {
        location.TransferReservedResources(this);
    }
}
