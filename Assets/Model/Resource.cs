using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource // TODO: Consider changing this to a struct and whenever resource amounts are changed, simply replace it with a new instance of the struct
{
    private static List<string> _resourceTypes = new List<string>();

    public static List<string> ResourceTypes
    {
        get { return _resourceTypes; }
    }

    public static void AddResourceType(string name, string data)
    {
        if (!_resourceTypes.Contains(name))
            _resourceTypes.Add(name);
    }

	public string type;
	[Range(0, 99999)]
	public int amount;

	public Resource (string resourceType, int startingAmount = 0) {
		type = resourceType;
		amount = startingAmount;
	}

    public bool GreaterOrEqual(Resource otherResource)
    {
        return (type == otherResource.type && amount >= otherResource.amount);
    }
}
