using UnityEngine;
using System.Collections;

public class DialogController : MonoBehaviour
{

		public GameObject DialogBox;

		private bool[] playerTalking;
		public string currentText;
		private int textTracker;

		private float textTime = 0;
		public float textSpeed = 3;

		public bool donePrinting = true;
		public bool mouseClicked = false;
		private float nextTracker = 0;
		private string[] dialog;
		private Rect dialogRect;

		public GUISkin textSkin;

		public Transform MyTransform { get; private set; }

		// Use this for initialization
		void Start ()
		{
				this.enabled = true;
				SomeDialog ();
		}
	
		// Update is called once per frame
		void Update ()
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
		if (playerTalking[textTracker]){
				GUI.Label (PlayerInputController.Instance.PlayerDialogBox, currentText);
		}
		else GUI.Label (dialogRect, currentText);
//		GUI.Label(nameRect, charNames[textTracker]);
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

		void SomeDialog ()
		{
		
				dialog = new string[2];
				dialog [0] = "Look at that \"Dead Rat\" over there";
				dialog [1] = "I wish somebody would tell me some cool RAT FACTS";

				playerTalking = new bool[2];
				playerTalking [0] = playerTalking [1] = true;

				dialogRect = new Rect (Screen.width / 10, Screen.height * 3 / 4, Screen.width * 8 / 13, Screen.height / 4);
				donePrinting = false;
		}

		void Activate (Vector2 mousePos)
		{
				DialogBox.SetActive (true);
		}
}
