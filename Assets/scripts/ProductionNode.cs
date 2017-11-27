using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductionNode : MonoBehaviour, IndustryNode {

	public enum NodeState {
		None,
		Producing,
		WaitingForDelivery
	}

	[SerializeField]
	public List<ResourceFlow> inputs = new List<ResourceFlow> ();
	[SerializeField]
	public List<ResourceFlow> outputs = new List<ResourceFlow> ();

	private List<Resource> _reservedResources = new List<Resource> ();
	private List<Resource> _requiredResources = new List<Resource> ();

	[Range(1, 1000)]
	public float cycleTime = 3f;
	public NodeState state = NodeState.None;

	public StorageNode connectedStorageNode;

	public StorageNode ConnectedStorageNode { 
		get { return connectedStorageNode; }
	}

	public void Update () {
		if (state == NodeState.Producing || state == NodeState.WaitingForDelivery)
			return;
		else {
			if (RequirementsMet ())
				StartCoroutine (Produce ());
			else {
				state = NodeState.WaitingForDelivery;

				foreach (Resource r in _requiredResources)
					PostFreightJob (r);
			}
		}
	}

	public bool SuppliesResource(ResourceType type) {
		return outputs.Exists (output => output.type == type);
	}

	private bool RequirementsMet() {
		bool reqsMet = true;

		foreach (ResourceFlow resource in inputs) {
			if (connectedStorageNode.HasResource (resource.type, resource.amount))
				continue;
			else {
				reqsMet = false;
				_requiredResources.Add(new Resource(resource.type, resource.amount * 10));
			}
		}

		if (!reqsMet)
			Debug.Log ("Requirements not met! I should post a job");

		return reqsMet && connectedStorageNode != null;
	}

	private void ReserveResources() {
		foreach (ResourceFlow r in inputs) {
			if (_requiredResources.Exists (resource => resource.type == resource.type))
				continue;
			else {
				_reservedResources.Add (connectedStorageNode.Take (new Resource(r.type, r.amount)));
			}
		}
	}

	private void UnreserveResources(FreightJob j) {
		List<Resource> temp = new List<Resource> ();

		foreach (Resource r in _reservedResources) {
			if (j.resource == r.type && j.amount == r.amount) {
				connectedStorageNode.Put (new Resource(r.type, r.amount));
				temp.Add (r);
			}
		}

		// Remove all resources that have been delivered from the reserved resource list
		// since they are now back in the connected storage node.
		_reservedResources.RemoveAll (resource => temp.Contains (resource));
	}

	private void PostFreightJob(Resource r) {
		Debug.Log ("Need " + r.amount + " " + System.Enum.GetName (typeof(ResourceType), r.type) + ". Posting job.");
		JobBoard jb = gameObject.GetComponentInParent<StarSystem>().JobBoard;

		jb.AddFreightJob (gameObject, r.type, r.amount);
	}

	private void NotifyOfJobCompletion(Job j) {
		if (j.GetType() == typeof(FreightJob)) {
			//UnreserveResources ((FreightJob)j);
			state = NodeState.None;
			FreightJob fj = j as FreightJob;
			_requiredResources.RemoveAll (r => r.type == fj.resource);
		}
	}

	private void DeductResources() {
		foreach (ResourceFlow r in inputs)
			connectedStorageNode.Take(new Resource(r.type, r.amount));
	}

	private IEnumerator Produce() {
		DeductResources ();
		yield return new WaitForSeconds (cycleTime);
		foreach (ResourceFlow r in outputs)
			connectedStorageNode.Put(new Resource(r.type, r.amount));

		state = NodeState.None;
	}

	public string ObjectInfo() {
		string result = "ProductionNode\nInputs:\n\t";

		foreach (ResourceFlow resource in inputs)
			result += "Type: " + resource.type.ToString() + "\tAmount: " + resource.amount + "\n";

		result += "Outputs:\t";

		foreach (ResourceFlow resource in outputs)
			result += "\tType: " + resource.type.ToString() + "\n\tProduced per cycle: " + resource.amount + "\n\t" + "Cycle time: " + cycleTime + "\n";

		return result;
	}

	private IEnumerator Wait(float secs) {
		yield return new WaitForSeconds (secs);
	}
}
