using UnityEngine;
using System.Collections;

public class StoryHandler : MonoBehaviour {

	public static StoryHandler Instance { get; private set; }

	public ClickableObject[] ObjectList;
	public int storyProgression;
	public GameObject BlackDoctor;

	// Use this for initialization
	void Start () { 
		ObjectList = Object.FindObjectsOfType(typeof(ClickableObject)) as ClickableObject[];
		storyProgression = 0;

		DialogController.Instance.SendMessage("StaticMessage", "August 29th 2008\nSouthern New Jersey", SendMessageOptions.RequireReceiver);
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(storyProgression){ 
		case 0:
			int objectTracker = 0;
			foreach (ClickableObject co in ObjectList){
				if (co.BeenClicked){
					objectTracker++;
				}
			}
			if (objectTracker >= 2){
				storyProgression++;
				ProgressStory();
			}
			break;
		case 1:
			if (CPState_Messenger.CurrentMessage > 5){
				storyProgression++;
				ProgressStory();
			}
			break;
		case 2:
			break;
		}
	}

	void ProgressStory(){
		switch(storyProgression){
		case 0:
			break;

		case 1:
			CellPhone.Instance.TriggerNewMessage = true;
			break;
		case 2:
			BlackDoctor.SetActive(true);
			break;

		
		}
	}
}
