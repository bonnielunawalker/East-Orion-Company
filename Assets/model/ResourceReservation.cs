using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceReservation {
	public GameObject reserver;
	public Resource resource;

	public ResourceReservation(Resource resource, GameObject reserver) {
		this.reserver = reserver;
		this.resource = resource;
	}

	public bool Matches(ResourceType type, int amount, GameObject reserver) {
		return this.reserver == reserver && resource.type == type && resource.amount == amount;
	}
}
