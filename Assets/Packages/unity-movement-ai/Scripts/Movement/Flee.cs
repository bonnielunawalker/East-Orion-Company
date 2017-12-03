using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Flee : MonoBehaviour {

    public float panicDist = 3.5f;

    public bool decelerateOnStop = true;

    public float maxAcceleration = 10f;

    public float timeToTarget = 0.1f;

    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    /* A flee steering behavior. Will return the steering for the current game object to flee a given position */
	public Vector2 getSteering(Vector2 targetPosition)
    {
        //Get the direction
		Vector2 acceleration = (Vector2)transform.position - targetPosition;

        //If the target is far way then don't flee
        if (acceleration.magnitude > panicDist)
        {
            //Slow down if we should decelerate on stop
            if (decelerateOnStop && rb.velocity.magnitude > 0.001f)
            {
                //Decelerate to zero velocity in time to target amount of time
                acceleration = -rb.velocity / timeToTarget;

                if (acceleration.magnitude > maxAcceleration)
                {
                    acceleration = giveMaxAccel(acceleration);
                }

                return acceleration;
            }
            else
            {
                rb.velocity = Vector2.zero;
				return Vector2.zero;
            }
        }

        return giveMaxAccel(acceleration);
    }

	private Vector2 giveMaxAccel(Vector2 v)
    {
        v.Normalize();

        //Accelerate to the target
        v *= maxAcceleration;

        return v;
    }
}
