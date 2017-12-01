using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreightContract : Contract
{
    public ResourceReservation reservation;

	public FreightContract (Employee contractCreator, Company issuingCompany, ResourceReservation reservedResource, JobBoard board)
    {
		issuer = issuingCompany;
		creator = contractCreator;
        reservation = reservedResource;
		base.jobBoard = board;
	}

	public override void MarkAsAccepted ()
    {
		return;
	}

	public override void MarkAsComplete ()
    {
		owner.acceptedContracts.Remove (this);
		issuer.outstandingContracts.Remove (this);
		NotifyOfCompletion ();
        completingEntity.contract = null;
	}

	public override void MarkAsUnableToComplete ()
    {
		return;
	}

	public override void NotifyOfCompletion ()
    {
		issuer.NotifyOfContractCompletion (this);

		ProductionNode pn = (ProductionNode) creator.GetComponent<IndustryNode>() as ProductionNode;
		pn.NotifyOfContractCompletion (this);
	}
}