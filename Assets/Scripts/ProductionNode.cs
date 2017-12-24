using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductionNode : IndustryNode
{
	public enum NodeState
    {
		Idle,
		Producing,
		WaitingForDelivery
	}

	[SerializeField]
	public List<ResourceFlow> inputs = new List<ResourceFlow>();
	[SerializeField]
	public List<ResourceFlow> outputs = new List<ResourceFlow>();

	private List<Resource> _reservedResources = new List<Resource>();
	private List<Resource> _requiredResources = new List<Resource>();
    private List<string> _requestedResourceTypes = new List<string>();

	[Range(1, 1000)]
	public float cycleTime = 3f;

	public NodeState state = NodeState.Idle;

    private bool _gatheringRequirements = false;

	public void Start()
    {
		employmentData = GetComponent<Employee> ();
	}

	public void Update ()
    {
        // If we are producing, skip everything else
		if (state == NodeState.Producing)
			return;

        // Update our list of required resources
        UpdateRequirements();

        // If after all those checks we did not need to place any orders and do not need any resources, start producing our product
        if (_requestedResourceTypes.Count == 0 && _requiredResources.Count == 0)
        {
            state = NodeState.Producing;
            StartCoroutine(Produce());
            return;
        }

        // If we have resources that we need to request, create contracts for them.
        if (_requiredResources.Count != 0)
        {
            state = NodeState.WaitingForDelivery;

            foreach (Resource r in _requiredResources)
                CreateFreightContract(r);

            // Clean up. Remove all required resources for which we have already placed an order.
            foreach (string t in _requestedResourceTypes)
                _requiredResources.RemoveAll(r => r.type == t);
        }
    }

    private void UpdateRequirements()
    {
        foreach (ResourceFlow resource in inputs)
            if (connectedStorageNode.HasResourceAmount(resource.type, resource.amount) || _reservedResources.Exists(r => r.type == resource.type))
                continue;
            // Only add a new resource requirement if one for this resource does not already exist and we have not already made a contract
            else if (!_requiredResources.Exists(r => r.type == resource.type) && !_requestedResourceTypes.Exists(r => r == resource.type))
                _requiredResources.Add(new Resource(resource.type, resource.amount));
    }

    public override bool SuppliesResource(string type)
    {
		return outputs.Exists (output => output.type == type);
	}

	public override bool HasResourceAmount (string type, int amount)
    {
		return connectedStorageNode.HasResourceAmount (type, amount);
	}

	private void ReserveResources()
    {
		foreach (ResourceFlow r in inputs) {
			if (_requiredResources.Exists (resource => resource.type == r.type))
				continue;
			else
				_reservedResources.Add (connectedStorageNode.Take (new Resource(r.type, r.amount)));
		}
	}

	private void UnreserveResources()
    {
        foreach (Resource r in _reservedResources)
            connectedStorageNode.Put(r);

        // Remove all resources that have been delivered from the reserved resource list
        // since they are now back in the connected storage node.
        _reservedResources.Clear();
	}

	private void CreateFreightContract(Resource resource)
    {
        Star star = gameObject.GetComponentInParent<Star>();

        // Check if the resource is available in this system
        if (!star.HasResourceAmount(resource))
            return;

        // Make a reservation
        ResourceReservation reservation = MakeReservation(resource, star);

        // Add the resource type to the list of requested types.
        // The resource will be removed from the _requiredResources list later in Update()
        _requestedResourceTypes.Add(resource.type);

		FreightContract newContract = new FreightContract (employmentData, employmentData.employer, reservation, star.jobBoard);
		employmentData.employer.ConsiderIssuingContract (newContract);

        Debug.Log("Posting contract");
	}

    public void NotifyOfContractRejection(FreightContract c)
    {
        _requestedResourceTypes.Remove(c.reservation.resource.type);
        _requiredResources.Add(c.reservation.resource);
    }

    // Make and return a new reservation for the required resource type and amount
    private ResourceReservation MakeReservation(Resource r, Star s)
    {
        foreach (KeyValuePair<Resource, StorageNode> kvp in s.ResourceLocations())
            if (kvp.Key.GreaterOrEqual(r))
                return kvp.Value.MakeReservation(this.connectedStorageNode, r);

        return default(ResourceReservation);
    }

	public void NotifyOfContractCompletion(Contract c)
    {
        FreightContract fc = (FreightContract)c;
        _requestedResourceTypes.RemoveAll(r => r == fc.reservation.resource.type);
    }

	private void DeductResources()
    {
		foreach (ResourceFlow r in inputs)
			connectedStorageNode.Take(new Resource(r.type, r.amount));
	}

	private IEnumerator Produce()
    {
        UnreserveResources();
		DeductResources();
		yield return new WaitForSeconds (cycleTime);
		foreach (ResourceFlow r in outputs)
			connectedStorageNode.Put(new Resource(r.type, r.amount));

		state = NodeState.Idle;

        Debug.Log("Production cycle complete!");
	}

	public override string ObjectInfo()
    {
		string result = "ProductionNode\nInputs:\n\t";

		foreach (ResourceFlow resource in inputs)
			result += "Type: " + resource.type.ToString() + "\tAmount: " + resource.amount + "\n";

		result += "Outputs:\t";

		foreach (ResourceFlow resource in outputs)
			result += "\tType: " + resource.type.ToString() + "\n\tProduced per cycle: " + resource.amount + "\n\t" + "Cycle time: " + cycleTime + "\n";

		return result;
	}

	private IEnumerator Wait(float secs)
    {
		yield return new WaitForSeconds (secs);
	}
}
