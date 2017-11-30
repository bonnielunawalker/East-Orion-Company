using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Company : MonoBehaviour
{

	public List<Contract> outstandingContracts = new List<Contract>();
	public List<Contract> acceptedContracts = new List<Contract>();
    private List<Contract> _acceptedContractRemovalBuffer = new List<Contract>();

	public List<Employee> employees = new List<Employee> ();

	public bool searchingForContracts = false;

	// This is for looping through a list of contracts and accepting them.
	// Since the iteration and the removal happen in different methods, we need a list to store them in accessable by both.
	private List<Contract> _contractsToRemove = new List<Contract>();

	public void Update()
    {
		if (!searchingForContracts)
			StartCoroutine (SearchForValidContracts ());

        AssignContracts();

        foreach (Contract c in _acceptedContractRemovalBuffer)
            acceptedContracts.Remove(c);

        _acceptedContractRemovalBuffer.Clear();
	}

	public void AddEmployee(Employee e)
    {
		employees.Add (e);
		e.employer = this;
	}
		
	 // Methods relating to contract behaviour

	public IEnumerator SearchForValidContracts()
    {
		searchingForContracts = true;
		JobBoard[] jobBoards = FindObjectsOfType<JobBoard> ();

		foreach (JobBoard jb in jobBoards)
			foreach (Contract c in jb.Contracts)
				ConsiderAcceptingContract (c);

		foreach (Contract c in _contractsToRemove)
			c.jobBoard.RemoveContract (c);

		_contractsToRemove.Clear();

		yield return new WaitForSeconds (5f);
		searchingForContracts = false;
	}

    public void AssignContracts()
    {
        foreach (Employee e in employees)
            if (!e.HasContract() && acceptedContracts.Count > 0)
                foreach (Contract c in acceptedContracts)
                    if (!_acceptedContractRemovalBuffer.Contains(c) && e.CanAcceptContract(c))
                    {
                        Assign(e, c);
                        return;
                    }
    }

	// Recieve the contract for consideration for issuing.
	// For now this just means posting the contract. Later, more restrictions and checks can be added.
	public void ConsiderIssuingContract(Contract c)
    {
		IssueContract (c);
	}

	// Actually issue the contract to the job board.
	public void IssueContract(Contract c)
    {
		c.jobBoard.AddContract (c);
		outstandingContracts.Add (c);
	}

	// As with ConsiderIssuingContract, right now this means just accepting the contract.
	// However in future the company will make a value assessment on the contract before accepting it.
	public void ConsiderAcceptingContract(Contract c)
    {
		AcceptContract (c);
	}

	public void AcceptContract(Contract c)
    {
		_contractsToRemove.Add (c);
		c.owner = this;
		acceptedContracts.Add (c);
	}

	public void Assign(Employee assignee, Contract c)
    {
		assignee.contract = c;
		c.completingEntity = assignee;
        _acceptedContractRemovalBuffer.Add(c);
    }

    public void NotifyOfContractCompletion (Contract c)
    {
		outstandingContracts.Remove (c);
	}

	public void ReturnToAccepted(Contract c)
    {
		c.completingEntity = null;
		acceptedContracts.Add (c);
	}
}
