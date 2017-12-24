using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IndustryNode : MonoBehaviour, IInspectable
{
	public StorageNode connectedStorageNode;
	public Employee employmentData;

	public abstract bool SuppliesResource (string type);
	public abstract bool HasResourceAmount (string type, int amount);

	public abstract string ObjectInfo();
}