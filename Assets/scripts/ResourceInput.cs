using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInput : MonoBehaviour {

	public ResourceType type;
	[Range(1, 9999)]
	public int amount;
}
