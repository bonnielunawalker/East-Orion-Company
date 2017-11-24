using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private Camera _camera;

	[Range(1, 500)]
	public float scrollSpeed = 40;
	[Range(1, 10)]
	public float dragSpeed = 8;

	public GameObject followTarget;

	private Vector3 _dragOrigin;

	public void Start () {
		_camera = GetComponent<Camera> ();
	}

	public void LateUpdate () {
		// If the middle mouse button is pressed, drag the camera around.
		if (Input.GetMouseButtonDown (1)) {
			followTarget = null;
			_dragOrigin = Input.mousePosition;
		}

		if (!Input.GetMouseButton (1)) {
			GetScroll();
			Follow ();
			return;
		}
			
		Vector3 pos = _camera.ScreenToViewportPoint (Input.mousePosition - _dragOrigin);
		Vector3 move = new Vector3 (-pos.x * dragSpeed, -pos.y * dragSpeed, 0);
		_camera.transform.Translate (move * (-_camera.transform.position.z / 10), Space.World);

		GetScroll ();
		Follow ();
	}

	private void GetScroll() {
		// Zoom in and out with the scroll wheel.
		_camera.transform.Translate(new Vector3(0, 0, (Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed) * (-_camera.transform.position.z / 50)));
		if (_camera.transform.position.z > -8)
			_camera.transform.position = new Vector3 (_camera.transform.position.x, _camera.transform.position.y, -8);
	}

	public void SetFollow(GameObject target) {
		followTarget = target;
	}

	private void Follow() {
		if (followTarget != null)
			transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
	}
}
