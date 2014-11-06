using UnityEngine;
using System.Collections;

public class DialogController : MonoBehaviour
{
		public static DialogController Instance { get; private set; }

		private bool[] playerTalking;
		public string currentText;
		private int textTracker;

		private float textTime = 0;
		public float textSpeed = 3;

		public bool donePrinting = true;
		public bool mouseClicked = false;
		private float nextTracker = 0;
		private string[] dialog;
		private Rect playerRect;
		public Rect objectRect;
		public float objectOffset;
		public bool dynamicText;
		

		public GUISkin textSkin;

		public Transform MyTransform { get; private set; }

		// Use this for initialization
		void Awake ()
		{
				this.enabled = true;
				textTracker = 0;
				playerRect = new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 4);

				if (Instance != null) {
						Debug.LogError ("There is more than one DialogController in the scene!");
						return;
				}
				Instance = this;
		}
	// Goal x = 340.78
	//Object x val = 263.73
	// Screen x val = 552;
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				if (Input.GetMouseButton (0)) {
						Next ();
				}

				if ((Time.time - textTime) >= textSpeed / 1000) {
						if (donePrinting) {
								return;
						} else {
								if (dialog [textTracker] == "") {
										donePrinting = true;
										return;
								} else {
										currentText = currentText + dialog [textTracker] [0];
										dialog [textTracker] = dialog [textTracker].Remove (0, 1);
										textTime = Time.time;
								}
						}
				}
		}

		void OnGUI ()
		{
		GUI.skin = textSkin;
			GUI.Label (playerRect, currentText);
	}

		void Next ()
		{
				if (Time.time - nextTracker > 1) {
						nextTracker = Time.time;
						if (gameObject.activeSelf) {
								if (!donePrinting) {
										currentText = currentText + dialog [textTracker];
										donePrinting = true;
								} else {
										if (dialog.Length > (textTracker + 1)) {
												textTracker++;
												donePrinting = false;
												currentText = "";
										} else
												currentText = "";
								}
						}
				}

		}

	void StaticMessage (string message)
		{
		if (donePrinting && currentText == "")
		{
			SetMessage(message);
		}
		else if (donePrinting)
		{
			currentText = "";
			SetMessage(message);
		}
		else return;
		}

	public void DynamicMessage (DynamicDialog[] messages)
	{
		string [] updatedMessages = BranchDialog(messages);
		if (donePrinting && currentText == "")
		{
			SetMessages(updatedMessages);
		}
		else if (donePrinting)
		{
			currentText = "";
			SetMessages(updatedMessages);
		}
		else return;
	}

	void SetMessage(string message){
		dialog = new string[1];
		dialog [0] = message;
		playerTalking = new bool[1];
		playerTalking [0] = true;

		donePrinting = false;
		textTracker = 0;
	}

	void SetMessages(string[] messages){
		dialog = messages;
		
		donePrinting = false;
		textTracker = 0;
	}

	void SetDialogOrder(bool[] order){
		playerTalking = order;
	}

	string[] BranchDialog(DynamicDialog[] messages){
		int length = 0;
		for (int i = 0; i < messages.Length; i++){
			if (dynamicText){
				length++;
				}
			else if (!messages[i].isDynamic){
				length++;
			}
		}

		string[] returnArray = new string[length];

		for (int i = 0; i < messages.Length; i++){
			if (dynamicText){
				returnArray[i] = messages[i].message;
			}
			else if (!messages[i].isDynamic){
				returnArray[i] = messages[i].message;
			}
		}
		dynamicText = false;
		return returnArray;
	}
	
}
