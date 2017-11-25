using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, Inspectable {

	private bool _selected;
	private GameObject _infoPanel;

	[SerializeField]
	public List<IndustryNode> industryNodes;

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

		// Set up resource and storage nodes.
		industryNodes = new List<IndustryNode>();
		StorageNode sNode = gameObject.AddComponent<StorageNode> ();
		ResourceNode rNode = gameObject.AddComponent<ResourceNode> ();

		// Set up a new output flow for this planet's resource node.
		ResourceFlow newOutput = new ResourceFlow();
		newOutput.type = ResourceNode.rawResources [Random.Range (0, ResourceNode.rawResources.Length - 1)];
		newOutput.amount = Random.Range (1, 6);

		rNode.outputs.Add(newOutput);
		rNode.cycleTime = Random.Range (1, 11);
		rNode.connectedStorageNode = sNode;

		// Add the nodes to the planet's list of nodes to be accessed later.
		industryNodes.Add (sNode);
		industryNodes.Add (rNode);
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
		string result = gameObject.name + "\n";

		foreach (IndustryNode n in industryNodes)
			result += n.ObjectInfo ();

		return result;
	}
}
