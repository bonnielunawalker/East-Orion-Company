using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelController : MonoBehaviour {

	private UnityEngine.UI.Text _textField;

	public void Start() {
		_textField = gameObject.GetComponent<UnityEngine.UI.Text> ();
	}

	public void DisplayInfo(string info) {
		_textField.text = info;
	}
}
