using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeUnit : MonoBehaviour {

	public Ship ship;

	// Use this for initialization
	public void Start () {
		ship = gameObject.GetComponent<Ship>();
	}
	
	public GameObject FindProduct(ResourceType product) {
		foreach (Planet p in ship.currentSystem.Planets)
			foreach (IndustryNode n in p.industryNodes)
				if (n.SuppliesResource (product))
					return p.gameObject;

		return null;
	}
}
