using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Company : MonoBehaviour
{

	public List<Contract> outstandingContracts = new List<Contract>();
	public List<Contract> acceptedContracts = new List<Contract>();
	private List<Contract> _acceptedContractsRemovalBuffer = new List<Contract>();
	private List<Contract> _assignedContractsRemovalBuffer = new List<Contract>();

	public List<Employee> employees = new List<Employee>();

	public bool searchingForContracts = false;

	public void Update()
    {
		if (!searchingForContracts)
			StartCoroutine (SearchForValidContracts ());

        AssignContracts();

        foreach (Contract c in _assignedContractsRemovalBuffer)
            acceptedContracts.Remove(c);

        _assignedContractsRemovalBuffer.Clear();
	}

	public void AddEmployee(Employee e)
    {
		employees.Add (e);
		e.employer = this;
	}
		
	 /*
	  * Contract behaviour methods
	  */

	public IEnumerator SearchForValidContracts()
    {
		searchingForContracts = true;
		JobBoard[] jobBoards = FindObjectsOfType<JobBoard> ();

		foreach (JobBoard jb in jobBoards)
			if (jb.Contracts.Count > 0)
				foreach (Contract c in jb.Contracts)
					ConsiderAcceptingContract (c);			

		foreach (Contract c in _acceptedContractsRemovalBuffer)
			c.jobBoard.RemoveContract (c);

		_acceptedContractsRemovalBuffer.Clear();

		yield return new WaitForSeconds (5f);
		searchingForContracts = false;
	}

    public void AssignContracts()
	{
		foreach (Employee e in employees)
			if (!e.HasContract () && acceptedContracts.Count > 0)
				foreach (Contract c in acceptedContracts)
					if (!_assignedContractsRemovalBuffer.Contains (c) && e.CanAcceptContract (c))
						Assign (e, c);
	}

	// Recieve the contract for consideration for issuing.
	// For now this just means posting the contract. Later, more restrictions and checks can be added.
	public void ConsiderIssuingContract(Contract c)
    {
		if (true)
			IssueContract(c);
	}

	// Actually issue the contract to the job board.
	public void IssueContract(Contract c)
    {
		c.jobBoard.AddContract(c);
		outstandingContracts.Add(c);
	}
		
	public void ConsiderAcceptingContract(Contract c)
    {
		foreach (Employee e in employees)
			if (e.CanAcceptContract(c))
			{
				AcceptContract (c);
				return;
			}
	}

	public void AcceptContract(Contract c)
    {
		_acceptedContractsRemovalBuffer.Add (c);
		c.owner = this;
		acceptedContracts.Add (c);
		c.MarkAsAccepted ();
	}

	public void Assign(Employee assignee, Contract c)
    {
		assignee.contract = c;
		c.completingEntity = assignee;
		_assignedContractsRemovalBuffer.Add(c);
    }

    public void NotifyOfContractCompletion(Contract c)
    {
		outstandingContracts.Remove (c);
	}

	public void ReturnToAccepted(Contract c)
    {
		c.completingEntity = null;
		acceptedContracts.Add (c);
	}
}