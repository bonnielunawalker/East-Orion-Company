using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IInspectable
{

	public enum ShipState
    {
		Idle,
		SearchingForJob,
		Landed,
		Pickup,
		Dropoff
	}

	public ShipState state;

	public Star currentSystem;
	public GameObject destination = null;

	public int cargoCapacity = 20;

	private SteeringBasics _steeringBasics;
	private Rigidbody2D _rb;

	private bool _selected;
	private bool _atDestination = false;
	private GameObject _infoPanel;

	public StorageNode cargoHold;

	public Employee employmentData;

	public void Start()
    {
		state = ShipState.Idle;

		currentSystem = FindObjectOfType<Star> (); //TODO: when additional starsystems are implemented this will not work.

		_rb = GetComponent<Rigidbody2D> ();
		_steeringBasics = GetComponent<SteeringBasics> ();
		cargoHold = GetComponentInChildren<StorageNode> ();
		_infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");
		employmentData = GetComponent<Employee>();
	}

	public void Update()
	{
		if (_selected)
			_infoPanel.SendMessage("DisplayInfo", ObjectInfo());
	}

	public void FixedUpdate()
    {
        if (!employmentData.HasContract())
            state = ShipState.Idle;
        else if (employmentData.HasContract() && state == ShipState.Idle)
        {
            FreightContract c = (FreightContract)employmentData.contract;
            state = ShipState.Pickup;
            destination = c.reservation.location.gameObject;
        }

        if (state == ShipState.Pickup)
        {
            if (_atDestination)
            {
                _atDestination = false;
                _rb.velocity = Vector2.zero;

                FreightContract c = (FreightContract)employmentData.contract;

                Resource r = c.reservation.resource;
				c.reservation.Resolve(cargoHold);
				ResourceReservation newReservation = cargoHold.MakeReservation(c.reservation.location, c.reservation.resource);
                c.reservation = newReservation;

                destination = c.creator.GetComponent<IndustryNode>().connectedStorageNode.gameObject;

                state = ShipState.Dropoff;
            }
            else
                Arrive(destination);
        }
        else if (state == ShipState.Dropoff)
        {
            if (_atDestination)
            {
                _atDestination = false;
                destination = null;
                _rb.velocity = Vector2.zero;

                FreightContract f = (FreightContract)employmentData.contract;
				f.reservation.Resolve(f.creator.GetComponent<IndustryNode>().connectedStorageNode);
                employmentData.contract.MarkAsComplete();

                state = ShipState.Idle;
            }
            else
                Arrive(destination);
        }
	}

	private void Arrive(GameObject dest)
    {
		Vector2 accel = new Vector2(0, 0); // TODO: Tighten this up, we're creating a LOT of new Vector2s here...

		if (destination != null)
			accel = _steeringBasics.arrive (dest.transform.position);

		_steeringBasics.steer(accel);
		_steeringBasics.lookWhereYoureGoing();
	}

	public void OnMouseOver()
    {
		_selected = true;
		FindObjectOfType<InputController> ().inspectableSelected = true;
	}

	public void OnMouseExit()
    {
		_selected = false;
		FindObjectOfType<InputController> ().inspectableSelected = false;
	}

	public void OnMouseDown ()
    {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.SendMessage ("SetFollow", gameObject);
	}

	public void OnTriggerEnter2D(Collider2D c)
    {
		if (c.gameObject == destination)
			_atDestination = true;
	}

	public string ObjectInfo ()
    {
		string result = "Name: " + name + "\nState: " + System.Enum.GetName(typeof(ShipState), state); 

		if (destination != null)
			result += "\nDestination: " + destination.name;

		return result;
	}
}
