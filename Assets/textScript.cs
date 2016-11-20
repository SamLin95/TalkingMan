using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class textScript : MonoBehaviour {

	public string mText;
	Text text;
	
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		mText = "init value";
	}

	public void SetString(string str) {
		mText = str;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = mText;
	}
}
