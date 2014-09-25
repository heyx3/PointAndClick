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


	public Camera MyCamera { get; private set; }


	void Awake()
	{
		MyCamera = camera;
	}
	void Update()
	{
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