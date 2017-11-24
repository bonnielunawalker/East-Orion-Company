using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource {

	public ResourceType type;
	[Range(0, 99999)]
	public int amount;

	public Resource (ResourceType resourceType, int startingAmount = 0) {
		type = resourceType;
		amount = startingAmount;
	}
}
