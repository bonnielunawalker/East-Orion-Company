using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IndustryNode : IInspectable {
	StorageNode ConnectedStorageNode { get; } 	// Basically we need this because we need an easy way of accessing the storage of an industrynode without knowing what type of node it is.
												// If the type of the industry node is a StorageNode then this will simply point to itself.
	bool SuppliesResource(ResourceType type);
}