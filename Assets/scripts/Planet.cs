using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, IInspectable {

	private bool _selected;
	private GameObject _infoPanel;
	private StarSystem _system;

	[SerializeField]
	public List<IndustryNode> industryNodes =  new List<IndustryNode>();

	public List<ResourceNode> ResourceNodes {
		get {
			List<ResourceNode> result = new List<ResourceNode> ();
			foreach (IndustryNode n in industryNodes)
				if (n.GetType() == typeof(ResourceNode))
					result.Add((ResourceNode)n);

			return result;
		}
	}

	public List<ProductionNode> ProductionNodes {
		get {
			List<ProductionNode> result = new List<ProductionNode> ();
			foreach (IndustryNode n in industryNodes)
				if (n.GetType() == typeof(ProductionNode))
					result.Add((ProductionNode)n);

			return result;
		}
	}

	public List<StorageNode> StorageNodes {
		get {
			List<StorageNode> result = new List<StorageNode> ();
			foreach (IndustryNode n in industryNodes)
				if (n.GetType() == typeof(StorageNode))
					result.Add((StorageNode)n);

			return result;
		}
	}

	void Start () {
		_selected = false;
		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");

		_system = gameObject.GetComponentInParent<StarSystem> ();
	}

	void FixedUpdate () {
		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}

	public void OnMouseOver() {
		_selected = true;
		FindObjectOfType<InputController> ().inspectableSelected = true;
	}

	public void OnMouseExit() {
		_selected = false;
		FindObjectOfType<InputController> ().inspectableSelected = false;
	}

	public string ObjectInfo() {
		string result = gameObject.name + "\nProduces: ";

		foreach (ResourceNode n in ResourceNodes) {
			foreach (ResourceFlow output in n.outputs) {
				result += "\n\t" + System.Enum.GetName(typeof(ResourceType), output.type);
				result += "\tAmount: " + output.amount;
			}
		}

		foreach (ProductionNode n in ProductionNodes) {
			foreach (ResourceFlow output in n.outputs) {
				result += "\n\t" + System.Enum.GetName(typeof(ResourceType), output.type);
				result += "\tAmount: " + output.amount;
			}
		}

		result += "\nConsumes: ";

		foreach (ProductionNode n in ProductionNodes) {
			foreach (ResourceFlow input in n.inputs) {
				result += "\n\t" + System.Enum.GetName(typeof(ResourceType), input.type);
				result += "\tAmount: " + input.amount;
				if (n.state == ProductionNode.NodeState.Producing)
					Debug.Log("PRODUCING!");
			}
		}

		result += "\nStored: ";
		int reservations = 0;

		foreach (StorageNode n in StorageNodes) {
			foreach (Resource storedResource in n.resources) {
				result += "\n\t" + System.Enum.GetName (typeof(ResourceType), storedResource.type);
				result += "\tAmount: " + storedResource.amount;
			}

			reservations += n.reservations.Count;
		}

		result += "\nReservations: " + reservations;
			

		return result;
	}

	public void PostJob() {
		return;
	}
}
