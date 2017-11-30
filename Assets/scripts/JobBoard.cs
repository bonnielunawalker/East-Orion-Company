using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBoard : MonoBehaviour
{

	private List<Contract> _contracts = new List<Contract>();

	public List<Contract> Contracts
    {
		get { return _contracts; }
	}

	public void AddContract(Contract c)
    {
		_contracts.Add (c);
		c.jobBoard = this;
	}

	public void RemoveContract(Contract c)
    {
		_contracts.Remove (c);
	}
}
