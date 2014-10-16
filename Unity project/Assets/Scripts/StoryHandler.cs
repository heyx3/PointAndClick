using UnityEngine;
using System.Collections;

public class StoryHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DialogController.Instance.SendMessage("StaticMessage", "August 29th 2008\nSouthern New Jersey", SendMessageOptions.RequireReceiver);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
