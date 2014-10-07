using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FinalCamera : MonoBehaviour
{
	public static Camera Instance { get; private set; }


	public float Zoom = 3.0f;


	void Awake()
	{
		Instance = camera;
	}
	void Update()
	{
		camera.orthographicSize = (Screen.height / 2.0f) / Mathf.Pow(2.0f, Zoom);
	}
}