using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource // TODO: Consider changing this to a struct and whenever resource amounts are changed, simply replace it with a new instance of the struct
{
	public ResourceType type;
	[Range(0, 99999)]
	public int amount;

	public Resource (ResourceType resourceType, int startingAmount = 0) {
		type = resourceType;
		amount = startingAmount;
	}

    public bool GreaterOrEqual(Resource otherResource)
    {
        return (type == otherResource.type && amount >= otherResource.amount);
    }
}
