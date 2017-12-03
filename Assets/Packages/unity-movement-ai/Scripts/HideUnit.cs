using UnityEngine;
using System.Collections;

public class HideUnit : MonoBehaviour {
	public Rigidbody2D target;

    private SteeringBasics steeringBasics;
    private Hide hide;
    private Spawner obstacleSpawner;

    private WallAvoidance wallAvoid;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        hide = GetComponent<Hide>();
        obstacleSpawner = GameObject.Find("ObstacleSpawner").GetComponent<Spawner>();

        wallAvoid = GetComponent<WallAvoidance>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 hidePosition;
		Vector2 hideAccel = hide.getSteering(target, obstacleSpawner.objs, out hidePosition);

		Vector2 accel = wallAvoid.getSteering(hidePosition - (Vector2)transform.position);

        if (accel.magnitude < 0.005f)
        {
            accel = hideAccel;
        }

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();
    }
}
