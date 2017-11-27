using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IInspectable {

	public enum ShipState {
		None,
		SearchingForJob,
		Landed,
		Pickup,
		Dropoff
	}

	public ShipState state;

	public FreightJob currentJob;

	public StarSystem currentSystem;
	public GameObject destination = null;

	public int cargoCapacity = 20;

	private SteeringBasics _steeringBasics;
	private TradeUnit _tradeUnit;
	private Rigidbody2D _rb;

	private bool _selected;
	private bool _atDestination = false;
	private GameObject _infoPanel;

	public StorageNode cargoHold;

	private Dictionary<Job, ResourceReservation> _reservations = new Dictionary<Job, ResourceReservation> ();

	public void Start() {
		state = ShipState.None;

		currentSystem = FindObjectOfType<StarSystem> (); //TODO: find a way to do this better

		_rb = GetComponent<Rigidbody2D> ();
		_tradeUnit = GetComponent<TradeUnit> ();
		_steeringBasics = GetComponent<SteeringBasics> ();
		cargoHold = GetComponentInChildren<StorageNode> ();
		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");
	}

	public void FixedUpdate () {
		if (state == ShipState.None) {											// Do we have no state?
			if (currentJob == null && state != ShipState.SearchingForJob) {							// Is it because we have no job?
				state = ShipState.SearchingForJob;
				StartCoroutine(FindFreightJob ());										// Get a new freight job.
			} else {																// Is it for some other reason?
				Debug.LogWarning ("Ship has no state but has job.");						// Sorry to hear that, log an error.
			}
			_rb.velocity = Vector2.zero;									
		} else if (state == ShipState.Landed) {									// Are we landed?
			_rb.velocity = Vector2.zero;											// Don't move, only placeholder now.
			gameObject.transform.position = destination.transform.position;
		} else if (state == ShipState.Pickup) {									// Are we picking up?
			if (_atDestination) {													// Are we at our pickup destination?
				StorageNode n = destination.GetComponentInChildren<StorageNode>();
				n.TransferReserved (cargoHold, _reservations[currentJob]);
				_reservations.Remove (currentJob);
				_atDestination = false;
				destination = currentJob.creator.gameObject.transform.parent.gameObject;	// Set our destination to the dropoff location. TODO: Actually pick up the goods.
				state = ShipState.Dropoff;												// Update our shipstate
			} else {																// Are we not at our pickup destination?
				Arrive(destination);													// Move towards our destination.
			}
		} else if (state == ShipState.Dropoff) {								// Are we dropping off?
			if (_atDestination) {													// Are we at our dropoff destination?
				state = ShipState.None;												// Set our state to none. TODO: Actually drop off the goods.
				cargoHold.TransferResources(destination.GetComponentInChildren<StorageNode>(), new Resource(currentJob.resource, currentJob.amount));
				currentSystem.JobBoard.CompleteJob(currentJob);
				_atDestination = false;
				currentJob = null;														// Finish our job.
			} else {																// Are we not at our dropoff destination?
				Arrive (destination);													// Move towards our destination.
			}
		}
			

		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}

	private IEnumerator FindFreightJob() {
		currentJob = null;
		state = ShipState.None;
		destination = null;

		FreightJob jobBeingConsidered = currentSystem.TakeFreightJob ();

		if (jobBeingConsidered != null) {
			destination = _tradeUnit.FindProduct (jobBeingConsidered.resource);

			if (destination == null) {
				//Debug.LogWarning ("No goods of type required for job in system: " + System.Enum.GetName (typeof(ResourceType), jobBeingConsidered.resource));
				state = ShipState.None;

				currentSystem.JobBoard.AddJob (jobBeingConsidered);
				yield return new WaitForSeconds (4);
			} else {
				StorageNode destinationStorageNode = destination.GetComponentInChildren<StorageNode> ();

				int storedAmount = destinationStorageNode.QueryAmount (jobBeingConsidered.resource);

				if (storedAmount > 0) {
					_reservations.Add(jobBeingConsidered, destinationStorageNode.ReserveResources (new Resource (jobBeingConsidered.resource, storedAmount), this.gameObject));
					state = ShipState.Pickup;
					currentJob = jobBeingConsidered;
					state = ShipState.Pickup;
					yield return new WaitForSeconds (4);
				} else {
					//Debug.Log ("No resource of type " + System.Enum.GetName (typeof(ResourceType), jobBeingConsidered.resource) + " stored in node");
					state = ShipState.None;
					currentSystem.JobBoard.AddJob (jobBeingConsidered);
					yield return new WaitForSeconds (4);
				}
			}
		} else {
			state = ShipState.None;
			yield return new WaitForSeconds (4);
		}
	}

	private void Arrive(GameObject dest) {
		Vector2 accel = new Vector2(0, 0); // TODO: Fix this up, we're creating a LOT of new Vector2s here...

		if (destination != null)
			accel = _steeringBasics.arrive (dest.transform.position);

		_steeringBasics.steer(accel);
		_steeringBasics.lookWhereYoureGoing();
	}

	public void OnMouseOver() {
		_selected = true;
		FindObjectOfType<InputController> ().inspectableSelected = true;
	}

	public void OnMouseExit() {
		_selected = false;
		FindObjectOfType<InputController> ().inspectableSelected = false;
	}

	public void OnMouseDown () {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.SendMessage ("SetFollow", gameObject);
	}

	public void OnTriggerEnter2D(Collider2D c) {
		if (c.gameObject == destination)
			_atDestination = true;
	}

	public string ObjectInfo () {
		string result = "Name: " + name + "\nState: " + System.Enum.GetName(typeof(ShipState), state); 

		if (destination != null)
			result += "\nDestination: " + destination.name;

		return result;
	}
}
