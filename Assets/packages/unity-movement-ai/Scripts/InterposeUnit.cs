using UnityEngine;
using System.Collections;

public class InterposeUnit : MonoBehaviour {

    public Rigidbody2D target1;
    public Rigidbody2D target2;

    private SteeringBasics steeringBasics;

    // Use this for initialization
    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 accel = steeringBasics.interpose(target1, target2);

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();
    }
}
