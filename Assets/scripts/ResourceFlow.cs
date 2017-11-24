using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourceFlow {
	public ResourceType type;
	public int amount;
}

[System.Serializable]
public enum ResourceType {
	Ice,
	Ore,
	Gas,
	Water,
	Grain,
	Cattle
}
