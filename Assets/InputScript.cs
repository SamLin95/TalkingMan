using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputScript : MonoBehaviour {


	public InputField input;
	// Use this for initialization
	void Start () {
		input = GetComponent<InputField> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void clearText() {
		input.text = "";
	}

	public string getText() {
		if (input.text == null)
			return "";
		return input.text;
	}
}
