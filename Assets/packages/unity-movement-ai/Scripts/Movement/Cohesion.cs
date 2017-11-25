using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringBasics))]
public class Cohesion : MonoBehaviour {

    public float facingCosine = 120f;

    private float facingCosineVal;

    private SteeringBasics steeringBasics;

	// Use this for initialization
	void Start () {
        facingCosineVal = Mathf.Cos(facingCosine * Mathf.Deg2Rad);
        steeringBasics = GetComponent<SteeringBasics>();
	}

	public Vector2 getSteering(ICollection<Rigidbody2D> targets)
    {
		Vector2 centerOfMass = Vector2.zero;
        int count = 0;

        /* Sums up everyone's position who is close enough and in front of the character */
        foreach (Rigidbody2D r in targets)
        {
            if (steeringBasics.isFacing(r.position, facingCosineVal))
            {
                centerOfMass += r.position;
                count++;
            }
        }

        if (count == 0)
        {
			return Vector2.zero;
        }
        else
        {
            centerOfMass = centerOfMass / count;

            return steeringBasics.arrive(centerOfMass);
        }
    }
}
