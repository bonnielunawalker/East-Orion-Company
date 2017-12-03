using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteeringBasics))]
public class WallAvoidance : MonoBehaviour {

    /* How far ahead the ray should extend */
    public float mainWhiskerLen = 1.25f;

    /* The distance away from the collision that we wish go */
    public float wallAvoidDistance = 0.5f;

    public float sideWhiskerLen = 0.701f;

    public float sideWhiskerAngle = 45f;

    public float maxAcceleration = 40f;

    private Rigidbody2D rb;
    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        steeringBasics = GetComponent<SteeringBasics>();
    }

	public Vector2 getSteering()
    {
        return getSteering(rb.velocity);
    }

	public Vector2 getSteering(Vector2 facingDir)
    {
		Vector2 acceleration = Vector2.zero;

        /* Creates the ray direction vector */
		Vector2[] rayDirs = new Vector2[3];
        rayDirs[0] = facingDir.normalized;

        float orientation = Mathf.Atan2(rb.velocity.y, rb.velocity.x);

        rayDirs[1] = orientationToVector(orientation + sideWhiskerAngle * Mathf.Deg2Rad);
        rayDirs[2] = orientationToVector(orientation - sideWhiskerAngle * Mathf.Deg2Rad);

        RaycastHit hit;

        /* If no collision do nothing */
        if (!findObstacle(rayDirs, out hit))
        {
            return acceleration;
        }

        /* Create a target away from the wall to seek */
		Vector2 targetPostition = hit.point + hit.normal * wallAvoidDistance;

        return steeringBasics.seek(targetPostition, maxAcceleration);
    }

    /* Returns the orientation as a unit vector */
    private Vector3 orientationToVector(float orientation)
    {
        return new Vector3(Mathf.Cos(orientation), Mathf.Sin(orientation), 0);
    }

	private bool findObstacle(Vector2[] rayDirs, out RaycastHit firstHit)
    {
        firstHit = new RaycastHit();
        bool foundObs = false;

        for (int i = 0; i < rayDirs.Length; i++)
        {
            float rayDist = (i == 0) ? mainWhiskerLen : sideWhiskerLen;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, rayDirs[i], out hit, rayDist))
            {
                foundObs = true;
                firstHit = hit;
                break;
            }

            //Debug.DrawLine(transform.position, transform.position + rayDirs[i] * rayDist);
        }

        return foundObs;
    }
}
