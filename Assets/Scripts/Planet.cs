using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Planet : MonoBehaviour, IInspectable
{

	private bool _selected;
	private GameObject _infoPanel;

	[SerializeField]
	public List<IndustryNode> industryNodes =  new List<IndustryNode>();

	public List<ResourceNode> ResourceNodes
    {
		get
        {
            return industryNodes.FindAll(n => n.GetType() == typeof(ResourceNode)).Cast<ResourceNode>().ToList();
        }
	}

	public List<ProductionNode> ProductionNodes
    {
		get
        {
            return industryNodes.FindAll(n => n.GetType() == typeof(ProductionNode)).Cast<ProductionNode>().ToList();
        }
	}

	public List<StorageNode> StorageNodes {
		get
        {
            return industryNodes.FindAll(n => n.GetType() == typeof(StorageNode)).Cast<StorageNode>().ToList();
		}
	}

	void Start ()
    {
		_selected = false;
		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");
	}

	void Update ()
    {
		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}

    /*
    * Planet information lookup methods
    */

    // Returns a list of all unique resource types with at least one unit currently in storage on the planet.
    public List<ResourceType> ResourceTypesAvailable()
    {
        List<ResourceType> result = new List<ResourceType>();

        foreach (StorageNode n in StorageNodes)
            foreach (Resource r in n.resources)
                if (!result.Exists(resource => resource == r.type))
                    result.Add(r.type);

        return result;
    }

    // Returns a list of all unique resource types produced on the planet.
    public List<ResourceType> ResourceTypesProduced()
    {
        List<ResourceType> result = new List<ResourceType>();

        foreach (ProductionNode n in ProductionNodes)
            foreach (ResourceFlow r in n.outputs)
                if (!result.Exists(resource => resource == r.type))
                    result.Add(r.type);

        foreach (ResourceNode n in ResourceNodes)
            foreach (ResourceFlow r in n.outputs)
                if (!result.Exists(resource => resource == r.type))
                    result.Add(r.type);

        return result;
    }

    // Returns a list of all resource objects stored on the planet.
    public List<Resource> StoredResources()
    {
        List<Resource> result = new List<Resource>();

        result = StorageNodes.Aggregate((nodes, next) => next).resources;

        return result;
    }

    // Returns a dictionary of resource objects mapped to their storage locations
    public Dictionary<Resource, StorageNode> ResourceLocations()
    {
        Dictionary<Resource, StorageNode> result = new Dictionary<Resource, StorageNode>();

        foreach (StorageNode n in StorageNodes)
            foreach (Resource r in n.resources)
                result.Add(r, n);

        return result;
    }

    public void OnMouseOver()
    {
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
			foreach (ResourceFlow output in n.outputs)
            {
				result += "\n\t" + System.Enum.GetName(typeof(ResourceType), output.type);
				result += "\tAmount: " + output.amount;
			}
		}

		foreach (ProductionNode n in ProductionNodes)
        {
			foreach (ResourceFlow output in n.outputs)
            {
				result += "\n\t" + System.Enum.GetName(typeof(ResourceType), output.type);
				result += "\tAmount: " + output.amount;
			}
		}

		result += "\nConsumes: ";

		foreach (ProductionNode n in ProductionNodes)
        {
			foreach (ResourceFlow input in n.inputs)
            {
				result += "\n\t" + System.Enum.GetName(typeof(ResourceType), input.type);
				result += "\tAmount: " + input.amount;
				if (n.state == ProductionNode.NodeState.Producing)
					Debug.Log("PRODUCING!");
			}
		}

		result += "\nStored: ";
		int reservations = 0;

		foreach (StorageNode n in StorageNodes)
        {
			foreach (Resource storedResource in n.resources)
            {
				result += "\n\t" + System.Enum.GetName (typeof(ResourceType), storedResource.type);
				result += "\tAmount: " + storedResource.amount;
			}

			reservations += n.reservations.Count;
		}

		result += "\nReservations: " + reservations;
			

		return result;
	}
}
