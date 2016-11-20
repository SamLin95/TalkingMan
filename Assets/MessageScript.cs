using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour {

	static Animator anim;

	private string postUrl = "http://172.30.8.208:8080/chat";
	private string getUrl;

	private string messageToSend = "";
	private string messageFromServer = "";

	private const int WAIT_USER_INPUT = 0;// idle
	private const int WAIT_SERVER_RESPONSE = 1;// thinking
	private const int TALKING = 2;
	private const int ERROR = 3;

	private int curState;
	private int lastState;

	private TouchScreenKeyboard touchScreenKeyboard;

	public Text talkingScript;
	public Text inputText;

	textScript ts;

	GameObject text;
	GameObject ty;

	InputScript ist;


	// Use this for initialization
	void Start () {

		ty = GameObject.Find ("Ty");
		anim = ty.GetComponent<Animator>();
		talkingScript = GetComponentInChildren<Text>();
		curState = WAIT_SERVER_RESPONSE;
		lastState = WAIT_SERVER_RESPONSE;

		touchScreenKeyboard = TouchScreenKeyboard.Open (string.Empty, TouchScreenKeyboardType.Default);

		// For blackboard text.
		text = GameObject.Find("Text");
		ts = text.GetComponent<textScript> ();

		 //For input text.
		GameObject inputObject = GameObject.Find("InputField");
		ist = inputObject.GetComponent<InputScript> ();

//		// For submit button.
//		btn = GameObject.Find("Button");
//		submitBtn = btn.GetComponent<Button> ();
//		submitBtn.onClick.AddListener(TaskOnClick);


		StartCoroutine("startLogics");
	}

	IEnumerator startLogics() {
		while (true) {
			switch (curState) {
			case WAIT_USER_INPUT:
				while (!touchScreenKeyboard.done) {
					yield return null;
				}
				//yield return new WaitForSeconds(2.0f);
				messageToSend = ist.getText ();
				ist.clearText ();
				lastState = curState;
				curState = WAIT_SERVER_RESPONSE;
				break;

				case WAIT_SERVER_RESPONSE:
					WWWForm form = new WWWForm();
					form.AddField("message", messageToSend);
					UnityWebRequest www = UnityWebRequest.Post(postUrl, form);
					yield return www.Send();

					if (www.isError) {
						Debug.Log("Post error!");
						lastState = curState;
						curState = ERROR;
					} else {
						messageFromServer = www.downloadHandler.text;
						Debug.Log("Receive Server Text: " + messageFromServer);
						lastState = curState;
						curState = TALKING;
					}
					break;

				case TALKING:
					Debug.Log("Talking about message: " + messageFromServer);
					yield return new WaitForSeconds(5.0f);
					lastState = curState;
					curState = WAIT_USER_INPUT;
					break;

				case ERROR:
					yield return new WaitForSeconds(10.0f);
					Debug.Log("Error occurs!");
					lastState = curState;
					curState = ERROR;
					break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//talkingScript.text = messageFromServer;
		//Debug.Log(ty.ToString());
//		Debug.Log(messageFromServer);
		ts.SetString(messageFromServer);
		if (lastState == WAIT_SERVER_RESPONSE && curState == TALKING) {
			anim.SetTrigger("ThinkToTalk");
		} else if (lastState == TALKING && curState == WAIT_USER_INPUT) {
			anim.SetTrigger("TalkToIdle");
		} else if (lastState == WAIT_USER_INPUT && curState == WAIT_SERVER_RESPONSE) {
			anim.SetTrigger("IdleToThink");
		} else if (curState == ERROR) {
			anim.SetTrigger("ErrorOccurs");
		}
	}

	void TaskOnClick() {
		Debug.Log("Button Clicked!");
	}
}
