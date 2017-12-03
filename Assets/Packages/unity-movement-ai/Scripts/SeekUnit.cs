using UnityEngine;
using System.Collections;

public class SeekUnit : MonoBehaviour {

    public Transform target;

    private SteeringBasics steeringBasics;

	// Use this for initialization
	void Start () {
        steeringBasics = GetComponent<SteeringBasics>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 accel = steeringBasics.seek(target.position);

        steeringBasics.steer(accel);
        steeringBasics.lookWhereYoureGoing();
    }
}
