using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Contract
{
	public Employee creator;
	public Company issuer;
	public Company owner;
	public Employee completingEntity;
	public JobBoard jobBoard;

    public abstract void MarkAsAccepted();

    public abstract void MarkAsComplete();

    public abstract void MarkAsUnableToComplete();

    public abstract void NotifyOfCompletion();
}