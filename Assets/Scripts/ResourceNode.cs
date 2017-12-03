using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : IndustryNode
{
	public static ResourceType[] rawResources = new ResourceType[]
    {
		ResourceType.Ice,
		ResourceType.IronOre,
		ResourceType.AluminiumOre,
		ResourceType.Gas
	};

	[Range(1, 1000)]
	public float cycleTime = 3f;
	public bool producing = false;

	[SerializeField]
	public List<ResourceFlow> outputs = new List<ResourceFlow> ();

	public void Start()
    {
		employmentData = GetComponent<Employee> ();
	}

	public void Update ()
    {
		if (RequirementsMet () && !producing)
			StartCoroutine (Produce());
	}

	public override bool SuppliesResource(ResourceType type)
    {
		return outputs.Exists (output => output.type == type);
	}

	public override bool HasResourceAmount(ResourceType type, int amount)
    {
		return connectedStorageNode.HasResourceAmount (type, amount);
	}

	private bool RequirementsMet()
    {
		return connectedStorageNode != null;
	}

	private IEnumerator Produce()
    {
		producing = true;
		yield return new WaitForSeconds (cycleTime);
		foreach (ResourceFlow r in outputs)
			connectedStorageNode.Put(new Resource(r.type, r.amount));

		producing = false;
	}

	public override string ObjectInfo()
    {
		string result = "ResourceNode\n";

		foreach (ResourceFlow resource in outputs)
			result += "\tType: " + resource.type.ToString() + "\n\tProduced per cycle: " + resource.amount + "\n\t" + "Cycle time: " + cycleTime + "\n";

		return result + "\n";
	}
}
