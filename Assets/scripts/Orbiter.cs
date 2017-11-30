using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{

	private float _gravitationalConstant;
	private GameObject _parentBody;
	private Rigidbody2D _parentRb;
	private Rigidbody2D _rb;

	public bool isMoon = false;

	public Orbiter moonPrefab;

	public void Start ()
    {
		_gravitationalConstant = 1000;
		_parentBody = transform.parent.gameObject;
		_rb = GetComponent<Rigidbody2D> ();
		_parentRb = _parentBody.GetComponent<Rigidbody2D> ();
	}

	public void FixedUpdate ()
    {
		_rb.velocity = CircularOrbit () + _parentRb.velocity;
	}

	private Vector2 CircularOrbit ()
    {
		Vector2 dist = new Vector2 (transform.position.x - _parentBody.transform.position.x, transform.position.y - _parentBody.transform.position.y);
		float r = dist.magnitude;
		Vector2 pdist = new Vector2 (dist.y, -dist.x).normalized;
		float m1 = _rb.mass;
		float m2 = _parentBody.GetComponent<Rigidbody2D> ().mass;

		float f = Mathf.Sqrt (_gravitationalConstant * ((m1 + m2) / r)) * Time.fixedDeltaTime;

		return pdist * f;
	}

	public void OnMouseDown ()
    {
		GameObject camera = GameObject.FindGameObjectWithTag ("MainCamera");
		camera.SendMessage ("SetFollow", gameObject);
	}
}
