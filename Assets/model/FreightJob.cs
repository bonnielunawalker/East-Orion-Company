using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreightJob : Job {

	public ResourceType resource;
	public int amount;

	public FreightJob(GameObject loc, ResourceType res, int amt) {
		creator = loc;
		resource = res;
		amount = amt;
	}
}
