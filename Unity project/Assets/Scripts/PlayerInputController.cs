﻿using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Manages all aspects of the player's mouse input.
/// </summary>
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerInputController : MonoBehaviour
{
	public static PlayerInputController Instance { get; private set; }


	/// <summary>
	/// The container that all clickable objects should be inside.
	/// </summary>
	public Transform ClickableObjectsContainer = null;
	/// <summary>
	/// The prefab for the indicator when the player tells the character where to move.
	/// </summary>
	public GameObject TargetPosIndicatorPrefab = null;

	/// <summary>
	/// The flashlight light cone.
	/// </summary>
	public Transform FlashlightCone = null;

	/// <summary>
	/// The player's walking speed.
	/// </summary>
	public float WalkSpeed = 40.0f;
	/// <summary>
	/// The distance to the target pos at which the player stops moving.
	/// </summary>
	public float MaxDistToTarget = 5.0f;

	public bool IsUsingFlashlight = false;


	/// <summary>
	/// Performs a non-trivial series of operations that is directly proportional
	/// to the number of children in "ClickableObjectsContainer".
	/// If "ClickableObjectsContainer" is null, an empty collection is returned.
	/// </summary>
	public IEnumerable<ClickableObject> ClickableObjects
	{
		get
		{
			if (ClickableObjectsContainer == null) yield break;

			for (int i = 0; i < ClickableObjectsContainer.childCount; ++i)
			{
				ClickableObject clo = ClickableObjectsContainer.GetChild(i).GetComponent<ClickableObject>();
				if (clo == null)
				{
					Debug.LogWarning("Child object '" + clo.gameObject.name + "' of container '" +
										 ClickableObjectsContainer.gameObject.name +
										 "' does not have a 'ClickableObject' component");
				}
				else
				{
					yield return clo;
				}
			}
		}
	}
	/// <summary>
	/// A cached reference to this player's animation controller.
	/// </summary>
	public PlayerAnimationController MyAnimations { get; private set; }
	/// <summary>
	/// A cached reference to this player's transform.
	/// </summary>
	public Transform MyTransform { get; private set; }
	/// <summary>
	/// Whether the mouse was clicked last frame.
	/// </summary>
	public bool MousePressedLastFrame { get; private set; }

	private Transform targetPosIndicator = null;


	void Awake()
	{
		MousePressedLastFrame = false;

		MyAnimations = GetComponent<PlayerAnimationController>();
		MyTransform = transform;

		if (ClickableObjectsContainer == null)
		{
			Debug.LogWarning("'ClickableObjectsContainer' field in PlayerInputController component of object '" +
							     gameObject.name + "' is null");
		}
		if (TargetPosIndicatorPrefab == null)
		{
			Debug.LogError("'TargetPosIndicatorPrefab' field in PlayerInputController component of object '" +
						       gameObject.name + "' is null");
			return;
		}
		if (Instance != null)
		{
			Debug.LogError("There is more than one player in the scene!");
			return;
		}

		Instance = this;
	}

	private Transform GenerateTargetPosIndicator(float x)
	{
		if (targetPosIndicator == null)
			targetPosIndicator = ((GameObject)Instantiate(TargetPosIndicatorPrefab)).transform;

		targetPosIndicator.position = new Vector3(x, targetPosIndicator.position.y, targetPosIndicator.position.z);
		return targetPosIndicator;
	}
	void Update()
	{
		//First update mouse input.
		bool mouseThisFrame = Input.GetMouseButton(0);
		if (mouseThisFrame && !MousePressedLastFrame)
		{
			Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 worldMouse = new Vector2((Screen.width * -0.5f) + Mathf.Clamp(mouse.x, 0.0f, Screen.width),
											 (Screen.height * -0.5f) + Mathf.Clamp(mouse.y, 0.0f, Screen.height));
			bool foundClick = false;

			//First see if any objects were clicked on.
			foreach (ClickableObject co in ClickableObjects)
			{
				if (co.MyCollider.OverlapPoint(worldMouse))
				{
					co.OnClicked(mouse);
					foundClick = true;
					break;
				}
			}

			//Next see if the cell phone was clicked on.
			if (CellPhone.Instance != null)
			{
				if (foundClick)
				{
					CellPhone.Instance.OnClickedOff(worldMouse);
				}
				else if (CellPhone.Instance.MyCollider.OverlapPoint(worldMouse))
				{
					CellPhone.Instance.OnClickedOn(worldMouse);
					IsUsingFlashlight = false;
					foundClick = true;
				}
				else if (CellPhone.Instance.IsUp)
				{
					CellPhone.Instance.OnClickedOff(worldMouse);
					foundClick = true;
				}
			}

			//Finally, just interpret the mouse click as a movement input.
			if (!foundClick)
			{
				GenerateTargetPosIndicator(worldMouse.x);
			}
		}
		MousePressedLastFrame = mouseThisFrame;


		//Now update the movement logic.
		if (targetPosIndicator == null)
		{
			MyAnimations.SwitchToIdleAnim(MyAnimations.IsFacingRight);
		}
		else
		{
			float towardsTarget = targetPosIndicator.position.x - MyTransform.position.x;
			if (Mathf.Abs(towardsTarget) < MaxDistToTarget)
			{
				MyAnimations.SwitchToIdleAnim(MyAnimations.IsFacingRight);
				if (targetPosIndicator != null) Destroy(targetPosIndicator.gameObject);
			}
			else
			{
				MyAnimations.SwitchToWalkAnim(towardsTarget > 0.0f);
				MyTransform.position += new Vector3(WalkSpeed * Mathf.Sign(towardsTarget) * Time.deltaTime, 0.0f, 0.0f);
			}
		}

		//Update flashlight behavior.
		FlashlightCone.gameObject.SetActive(IsUsingFlashlight);
	}
}