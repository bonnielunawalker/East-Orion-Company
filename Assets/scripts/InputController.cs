using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

	private GameObject text;
	private GameObject infoPanel;

	public bool inspectableSelected;

	public void Start () {
		text = GameObject.FindGameObjectWithTag ("Pause text").gameObject;
		inspectableSelected = false;
		infoPanel = GameObject.FindGameObjectWithTag ("Infopanel");
	}

	public void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && Time.timeScale != 0)
			Time.timeScale = 0;
		else if (Input.GetKeyDown (KeyCode.Space) && Time.timeScale == 0)
			Time.timeScale = 1;

		if (Input.GetKeyDown (KeyCode.KeypadPlus) && Time.timeScale < 50)
			Time.timeScale += 1;
		else if (Input.GetKeyDown (KeyCode.KeypadMinus) && Time.timeScale > 0)
			Time.timeScale -= 1;

		if (Time.timeScale == 0)
			text.GetComponent<UnityEngine.UI.Text> ().text = "Paused";
		else
			text.GetComponent<UnityEngine.UI.Text> ().text = "Gamespeed: " + Time.timeScale.ToString();

		if (!inspectableSelected) {
			infoPanel.SendMessage ("DisplayInfo", "");
		}
	}
}
