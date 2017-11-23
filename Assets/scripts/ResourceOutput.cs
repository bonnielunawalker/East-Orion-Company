using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceOutput : MonoBehaviour {

	public ResourceType type;
	[Range(1, 9999)]
	public int amount;
}
