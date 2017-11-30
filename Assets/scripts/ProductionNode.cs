using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductionNode : IndustryNode
{
	public enum NodeState
    {
		None,
		Producing,
		WaitingForDelivery
	}

	[SerializeField]
	public List<ResourceFlow> inputs = new List<ResourceFlow>();
	[SerializeField]
	public List<ResourceFlow> outputs = new List<ResourceFlow>();

	private List<Resource> _reservedResources = new List<Resource>();
	private List<Resource> _requiredResources = new List<Resource>();
    private List<ResourceType> _requestedResourceTypes = new List<ResourceType>();

	[Range(1, 1000)]
	public float cycleTime = 3f;

	public NodeState state = NodeState.None;

	public void Start()
    {
		employmentData = GetComponent<Employee> ();
	}

	public void Update ()
    {
		if (state == NodeState.Producing)
			return;
		else
        {
			if (state != NodeState.WaitingForDelivery && RequirementsMet ())
				StartCoroutine (Produce ());
			else if (state != NodeState.WaitingForDelivery)
				state = NodeState.WaitingForDelivery;

            if (_requiredResources.Count != 0)
                foreach (Resource r in _requiredResources)
                    CreateFreightContract(r);

            foreach (ResourceType t in _requestedResourceTypes)
                _requiredResources.RemoveAll(r => r.type == t);
        }
	}

	public override bool SuppliesResource(ResourceType type)
    {
		return outputs.Exists (output => output.type == type);
	}

	public override bool HasResourceAmount (ResourceType type, int amount)
    {
		return connectedStorageNode.HasResourceAmount (type, amount);
	}

	private bool RequirementsMet()
    {
		bool reqsMet = true;

		_requiredResources.Clear ();

		foreach (ResourceFlow resource in inputs)
        {
			if (connectedStorageNode.HasResourceAmount (resource.type, resource.amount))
				continue;
			else
            {
				reqsMet = false;
				if (!_requiredResources.Exists(r => r.type == resource.type))
					_requiredResources.Add(new Resource(resource.type, resource.amount));
			}
		}

		if (!reqsMet)
			Debug.Log ("Requirements not met! I should post a job");

		return reqsMet && connectedStorageNode != null;
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

	private void UnreserveResources(FreightContract c)
    {
		List<Resource> temp = new List<Resource> ();

		foreach (Resource r in _reservedResources) {
			if (c.reservation.resource.type == r.type && c.reservation.resource.amount == r.amount)
            {
				connectedStorageNode.Put (new Resource(r.type, r.amount));
				temp.Add (r);
			}
		}

		// Remove all resources that have been delivered from the reserved resource list
		// since they are now back in the connected storage node.
		_reservedResources.RemoveAll (resource => temp.Contains (resource));
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
	}

    public void InformOfContractRejection(FreightContract c)
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
		if (c.GetType() == typeof(FreightContract))
        {
			//UnreserveResources ((FreightJob)j);
			state = NodeState.None;
			FreightContract fc = c as FreightContract;
			_requiredResources.RemoveAll (r => r.type == fc.reservation.resource.type);
		}
	}

	private void DeductResources()
    {
		foreach (ResourceFlow r in inputs)
			connectedStorageNode.Take(new Resource(r.type, r.amount));
	}

	private IEnumerator Produce()
    {
		DeductResources ();
		yield return new WaitForSeconds (cycleTime);
		foreach (ResourceFlow r in outputs)
			connectedStorageNode.Put(new Resource(r.type, r.amount));

		state = NodeState.None;
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
