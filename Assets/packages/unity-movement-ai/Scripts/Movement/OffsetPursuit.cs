using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringBasics))]
public class OffsetPursuit : MonoBehaviour {
    /* Maximum prediction time the pursue will predict in the future */
    public float maxPrediction = 1f;

    private Rigidbody2D rb;
    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        steeringBasics = GetComponent<SteeringBasics>();
    }

	public Vector2 getSteering(Rigidbody2D target, Vector2 offset)
    {
		Vector2 targetPos;
        return getSteering(target, offset, out targetPos);
    }

	public Vector2 getSteering(Rigidbody2D target, Vector2 offset, out Vector2 targetPos)
    {
		Vector2 worldOffsetPos = target.position + (Vector2)target.transform.TransformDirection(offset);

        //Debug.DrawLine(transform.position, worldOffsetPos);

        /* Calculate the distance to the offset point */
		Vector2 displacement = worldOffsetPos - (Vector2)transform.position;
        float distance = displacement.magnitude;

        /* Get the character's speed */
        float speed = rb.velocity.magnitude;

        /* Calculate the prediction time */
        float prediction;
        if (speed <= distance / maxPrediction)
        {
            prediction = maxPrediction;
        }
        else
        {
            prediction = distance / speed;
        }

        /* Put the target together based on where we think the target will be */
        targetPos = worldOffsetPos + target.velocity * prediction;

        return steeringBasics.arrive(targetPos);
    }
}
