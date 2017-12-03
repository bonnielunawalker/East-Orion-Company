using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Separation : MonoBehaviour {

    /* The maximum acceleration for separation */
    public float sepMaxAcceleration = 25;

    /* This should be the maximum separation distance possible between a separation
     * target and the character.
     * So it should be: separation sensor radius + max target radius */
    public float maxSepDist = 1f;

    private float boundingRadius;

    // Use this for initialization
    void Start()
    {
        boundingRadius = SteeringBasics.getBoundingRadius(transform);
    }

	public Vector2 getSteering(ICollection<Rigidbody2D> targets)
    {
		Vector2 acceleration = Vector2.zero;

        foreach (Rigidbody2D r in targets)
        {
            /* Get the direction and distance from the target */
			Vector2 direction = (Vector2)transform.position - r.position;
            float dist = direction.magnitude;

            if (dist < maxSepDist)
            {
                float targetRadius = SteeringBasics.getBoundingRadius(r.transform);

                /* Calculate the separation strength (can be changed to use inverse square law rather than linear) */
                var strength = sepMaxAcceleration * (maxSepDist - dist) / (maxSepDist - boundingRadius - targetRadius);

                /* Added separation acceleration to the existing steering */
                direction.Normalize();
                acceleration += direction * strength;
            }
        }

        return acceleration;
    }
}
