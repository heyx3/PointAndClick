using System;
using UnityEngine;


/// <summary>
/// Handles ambient lighting for the lighting camera this component is attached to.
/// </summary>
[RequireComponent(typeof(Camera))]
public class AmbientLighting : MonoBehaviour
{
	public float NormalBrightness = 0.4f,
				 UsingFlashlightBrightness = 0.2f;

	public Transform MyTransform { get; private set; }
	public Camera MyCamera { get; private set; }


	void Awake()
	{
		MyTransform = transform;
		MyCamera = camera;
	}
	void Update()
	{
		Vector3 pos = MyTransform.position;
		Vector3 playerPos = PlayerInputController.Instance.MyTransform.position;

		MyTransform.position = new Vector3(playerPos.x, pos.y, pos.z);

		if (PlayerInputController.Instance.IsUsingFlashlight)
		{
			MyCamera.backgroundColor = new Color(UsingFlashlightBrightness,
												 UsingFlashlightBrightness,
												 UsingFlashlightBrightness);
		}
		else
		{
			MyCamera.backgroundColor = new Color(NormalBrightness,
												 NormalBrightness,
												 NormalBrightness);
		}
	}
}