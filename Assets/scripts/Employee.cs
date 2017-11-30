using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Employee : MonoBehaviour
{
    public List<System.Type> acceptedContracts = new List<System.Type>();

	public Company employer;
	public Contract contract;

    public void AddAcceptedContractType<T>() where T : Contract
    {
        acceptedContracts.Add(typeof(T));
    }

    public bool AcceptsContractType<T>() where T : Contract
    {
        return acceptedContracts.Contains(typeof(T));
    }

    public bool CanAcceptContract(Contract c)
    {
        return acceptedContracts.Contains(c.GetType());
    }

    public bool HasContract()
    {
		return contract != null;
	}

	public void RejectContract()
    {
		employer.ReturnToAccepted (contract);
		contract = null;
	}
}