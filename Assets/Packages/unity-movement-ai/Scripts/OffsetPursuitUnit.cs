using UnityEngine;
using System.Collections;

public class OffsetPursuitUnit : MonoBehaviour {

    public Rigidbody2D target;
	public Vector2 offset;
    public float groupLookDist = 1.5f;

    private SteeringBasics steeringBasics;
    private OffsetPursuit offsetPursuit;
    private Separation separation;

    private NearSensor sensor;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        offsetPursuit = GetComponent<OffsetPursuit>();
        separation = GetComponent<Separation>();

        sensor = transform.Find("SeparationSensor").GetComponent<NearSensor>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
		Vector2 targetPos;
		Vector2 offsetAccel = offsetPursuit.getSteering(target, offset, out targetPos);
		Vector2 sepAccel = separation.getSteering(sensor.targets);

        steeringBasics.steer(offsetAccel + sepAccel);

        /* If we are still arriving then look where we are going, else look the same direction as our formation target */
		if (Vector2.Distance(transform.position, targetPos) > groupLookDist)
        {
            steeringBasics.lookWhereYoureGoing();
        } else
        {
            steeringBasics.lookAtDirection(target.rotation);
        }
    }
}
