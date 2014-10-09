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


	private Transform playerTr { get { return PlayerInputController.Instance.MyTransform; } }
	private Vector3 lastPlayerPos;

	private Transform myTr;

	
	void Start()
	{
		lastPlayerPos = playerTr.position;
		myTr = transform;
	}
	void FixedUpdate()
	{
		Vector3 newPos = playerTr.position;
		float deltaX = newPos.x - lastPlayerPos.x;

		myTr.position -= new Vector3(deltaX / ForegroundDistanceMultiplier, 0.0f, 0.0f);

		lastPlayerPos = newPos;
	}
}