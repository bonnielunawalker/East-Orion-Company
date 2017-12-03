using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SteeringBasics))]
[RequireComponent(typeof(Evade))]
public class Hide : MonoBehaviour {
    public float distanceFromBoundary = 0.6f;

    private SteeringBasics steeringBasics;
    private Evade evade;

    // Use this for initialization
    void Start () {
        steeringBasics = GetComponent<SteeringBasics>();
        evade = GetComponent<Evade>();
	}

	public Vector2 getSteering(Rigidbody2D target, ICollection<Rigidbody2D> obstacles)
    {
		Vector2 bestHidingSpot;
        return getSteering(target, obstacles, out bestHidingSpot);
    }

	public Vector2 getSteering(Rigidbody2D target, ICollection<Rigidbody2D> obstacles, out Vector2 bestHidingSpot)
    {
        //Find the closest hiding spot
        float distToClostest = Mathf.Infinity;
		bestHidingSpot = Vector2.zero;

        foreach(Rigidbody2D r in obstacles)
        {
			Vector2 hidingSpot = getHidingPosition(r, target);

			float dist = Vector2.Distance(hidingSpot, transform.position);

            if(dist < distToClostest)
            {
                distToClostest = dist;
                bestHidingSpot = hidingSpot;
            }
        }

        //If no hiding spot is found then just evade the enemy
        if(distToClostest == Mathf.Infinity)
        {
            return evade.getSteering(target);
        }

        //Debug.DrawLine(transform.position, bestHidingSpot);

        return steeringBasics.arrive(bestHidingSpot);
    }

    private Vector3 getHidingPosition(Rigidbody2D obstacle, Rigidbody2D target)
    {
        float distAway = SteeringBasics.getBoundingRadius(obstacle.transform) + distanceFromBoundary;

		Vector2 dir = obstacle.position - target.position;
        dir.Normalize();

        return obstacle.position + dir * distAway;
    }
}
