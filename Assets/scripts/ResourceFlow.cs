using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceFlow
{
	public ResourceType type;
	public int amount;

	public ResourceFlow() {}

	public ResourceFlow(ResourceType t, int amt)
    {
		type = t;
		amount = amt;
	}
}

[System.Serializable]
public enum ResourceType
{
	Ice,
	IronOre,
	AluminiumOre,
	Gas,
	Water,
	Steel,
	Aluminium,
	Grain,
	Cattle,
	Fibre,
	Cloth,
	Clothing,
	Meat,
	CannedFood
}
