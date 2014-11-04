using System;
using UnityEngine;


/// <summary>
/// Creates a parallax effect based on the movement of a given object.
/// </summary>
public class ParallaxObject : MonoBehaviour
{
	/// <summary>
	/// 0.0 is the camera, 1.0 is the foregroud, and positive infinity is the far background.
	/// </summary>
	public float ForegroundDistanceMultiplier = 1.0f;


	private Transform cameraTr { get { return MainCamera.Instance.transform; } }
	private Vector3 lastCameraPos;

	private Transform myTr;

	
	void Start()
	{
		lastCameraPos = cameraTr.position;
		myTr = transform;
	}
	void LateUpdate()
	{
		Vector3 newPos = cameraTr.position;
		float deltaX = newPos.x - lastCameraPos.x;

		myTr.position -= new Vector3(deltaX / ForegroundDistanceMultiplier, 0.0f, 0.0f);

		lastCameraPos = newPos;
	}
}