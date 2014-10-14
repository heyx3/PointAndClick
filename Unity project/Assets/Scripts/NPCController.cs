using UnityEngine;
using System.Collections;

public class NPCController : ClickableObject {

	public Vector2 colliderPos;
	private MonoBehaviour DialogScript;
	// Use this for initialization
	void Start () {
		DialogScript = gameObject.GetComponent<DialogController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnClicked(Vector2 mouse){
		DialogScript.enabled = true;
	}
}
