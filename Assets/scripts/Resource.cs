using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

	public ResourceType type;
	[Range(0, 9999)]
	public int amount;
}
