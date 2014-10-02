using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FinalCamera : MonoBehaviour
{
	public float Zoom = 3.0f;

void Update(){
	camera.orthographicSize = (Screen.height/2.0f) / Mathf.Pow(2.0f, Zoom);
}
}
