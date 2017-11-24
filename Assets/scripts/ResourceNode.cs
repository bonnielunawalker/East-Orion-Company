using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour, IndustryNode {

	public static ResourceType[] rawResources = new ResourceType[] {
		ResourceType.Ice,
		ResourceType.Ore,
		ResourceType.Gas
	};

	[Range(1, 1000)]
	public float cycleTime = 3f;
	public bool producing = false;

	public StorageNode connectedStorageNode;

	[SerializeField]
	public List<ResourceFlow> outputs = new List<ResourceFlow> ();

	public void Update () {
		if (RequirementsMet () && !producing)
			StartCoroutine (Produce());
	}

	private bool RequirementsMet() {
		return connectedStorageNode != null;
	}

	private IEnumerator Produce() {
		producing = true;
		yield return new WaitForSeconds (cycleTime);
		foreach (ResourceFlow resource in outputs)
			connectedStorageNode.Put(resource.type, resource.amount);

		producing = false;
	}

	public string ObjectInfo() {
		string result = "ResourceNode\n";

		foreach (ResourceFlow resource in outputs)
			result += "\tType: " + resource.type.ToString() + "\n\tProduced per cycle: " + resource.amount + "\n\t" + "Cycle time: " + cycleTime + "\n";

		return result + "\n";
	}
}
