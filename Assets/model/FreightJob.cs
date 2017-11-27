using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreightJob : Contract {

	public ResourceType resource;
	public int amount;

	public FreightJob(GameObject loc, ResourceType res, int amt) {
		issuer = loc;
		resource = res;
		amount = amt;
	}
}
