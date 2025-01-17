﻿using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Manages all aspects of the player's mouse input.
/// </summary>
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerInputController : MonoBehaviour
{
	public static PlayerInputController Instance { get; private set; }

	public int AlertDistance;
	public int AlertButtonDistance;

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

	/// <summary>
	/// The difference between the ground height and the player's center.
	/// </summary>
	public float GroundHeightOffset = 10.0f;
	/// <summary>
	/// The difference between the ground height and the target indicator's center.
	/// </summary>
	public float TargetGroundHeightOffset = 34.0f;

	public bool IsUsingFlashlight = false;

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

	private float CalculateGroundHeight(float x)
	{
		if (GroundHeight.Instance == null)
		{
			Debug.LogError("No 'GroundHeight' component in the scene!");
			return MyTransform.position.y;
		}
		else return GroundHeightOffset + GroundHeight.Instance.GetHeightAt(x);
	}
	private Transform GenerateTargetPosIndicator(float x)
	{
		float height = CalculateGroundHeight(x);

		if (!System.Single.IsNaN(height))
		{
			if (targetPosIndicator == null)
				targetPosIndicator = ((GameObject)Instantiate(TargetPosIndicatorPrefab)).transform;

			targetPosIndicator.position = new Vector3(x, height + TargetGroundHeightOffset, targetPosIndicator.position.z);
			return targetPosIndicator;
		}

		return null;
	}
	void Update()
	{
		//First update mouse input.
		
		RenderTexture gameRend = MainCamera.Instance.targetTexture;

		Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y),
				mouseLerp = new Vector2(mouse.x / (float)Screen.width, mouse.y / (float)Screen.height),
				cameraViewSize = new Vector2(2.0f * MainCamera.Instance.orthographicSize *
												(float)gameRend.width / gameRend.height,
											 2.0f * MainCamera.Instance.orthographicSize),
				camPos = new Vector2(MainCamera.Instance.transform.position.x,
									 MainCamera.Instance.transform.position.y),
				worldMouse = new Vector2(Mathf.Lerp(camPos.x - (cameraViewSize.x * 0.5f),
													camPos.x + (cameraViewSize.x * 0.5f),
													mouseLerp.x),
										 Mathf.Lerp(camPos.y - (cameraViewSize.y * 0.5f),
													camPos.y + (cameraViewSize.y * 0.5f),
													mouseLerp.y));

		bool mouseThisFrame = Input.GetMouseButton(0);
		bool clicked = mouseThisFrame && !MousePressedLastFrame;
		if (mouseThisFrame && !MousePressedLastFrame)
		{
			bool foundClick = false;

			//First see if the cell phone was clicked on.
			if (CellPhone.Instance != null)
			{
				if (CellPhone.Instance.MyCollider.OverlapPoint(worldMouse))
				{
					Inventory.Instance.CurrentlySelected = null;
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

			//Next see if the inventory was clicked on.
			if (Inventory.Instance != null)
			{
				if (Inventory.Instance.MyCollider.OverlapPoint(worldMouse))
				{
					Inventory.Instance.OnClickedOn();
					foundClick = true;
				}
				else if (Inventory.Instance.IsSelected)
				{
					Inventory.Instance.OnClickedOff();
					foundClick = true;
				}
			}

			//Next see if any objects were clicked on.
			foreach (ClickableObject co in ClickableObject.CurrentObjects)
			{
				if (co.MyCollider.OverlapPoint(worldMouse))
				{
					Inventory.InventoryObjects? invObj = Inventory.Instance.CurrentlySelected;
					Inventory.Instance.CurrentlySelected = null;
					co.OnClicked(mouse, invObj);
					foundClick = true;
					break;
				}
			}

			//Finally, just interpret the mouse click as a movement input.
			if (!foundClick)
			{
				if (Inventory.Instance.CurrentlySelected.HasValue)
					Inventory.Instance.CurrentlySelected = null;
				else GenerateTargetPosIndicator(worldMouse.x);
			}
		
		}
		else
		{
			//See if any objects were moused over.
			foreach (ClickableObject co in ClickableObject.CurrentObjects)
			{
				if (co.MyCollider.OverlapPoint(worldMouse))
				{
					co.OnMousedOver(mouse);
					break;
				}
			}
		}
		MousePressedLastFrame = mouseThisFrame;


		//Now update the movement logic.
		if (targetPosIndicator == null)
		{
			if (IsUsingFlashlight)
			{
				MyAnimations.SwitchToIdleLightAnim(MyAnimations.IsFacingRight);
			}
			else if (CellPhone.Instance.IsUp)
			{
				MyAnimations.SwitchToIdlePhoneAnim(MyAnimations.IsFacingRight);
			}
			else
			{
				MyAnimations.SwitchToIdleAnim(MyAnimations.IsFacingRight);
			}
		}
		else
		{
			float towardsTarget = targetPosIndicator.position.x - MyTransform.position.x;
			if (Mathf.Abs(towardsTarget) < MaxDistToTarget)
			{
				if (targetPosIndicator != null) Destroy(targetPosIndicator.gameObject);
				if (IsUsingFlashlight)
				{
					MyAnimations.SwitchToIdleLightAnim(MyAnimations.IsFacingRight);
				}
				else if (CellPhone.Instance.IsUp)
				{
					MyAnimations.SwitchToIdlePhoneAnim(MyAnimations.IsFacingRight);
				}
				else
				{
					MyAnimations.SwitchToIdleAnim(MyAnimations.IsFacingRight);
				}
			}
			else
			{
				if (IsUsingFlashlight)
				{
					MyAnimations.SwitchToWalkLightAnim(towardsTarget > 0.0f);
				}
				else if (CellPhone.Instance.IsUp)
				{
					MyAnimations.SwitchToWalkPhoneAnim(towardsTarget > 0.0f);
				}
				else 
				{
					MyAnimations.SwitchToWalkAnim(towardsTarget > 0.0f);
				}
				
				Vector3 pos = MyTransform.position;

				//Make sure the player can move to the next spot.
				float newX = pos.x + (WalkSpeed * Mathf.Sign(towardsTarget) * Time.deltaTime);
				float newY = CalculateGroundHeight(newX);
				if (System.Single.IsNaN(newY))
				{
					if (IsUsingFlashlight)
					{
						MyAnimations.SwitchToIdleLightAnim(MyAnimations.IsFacingRight);
					}
					else if (CellPhone.Instance.IsUp)
					{
						MyAnimations.SwitchToIdlePhoneAnim(MyAnimations.IsFacingRight);
					}
					else
					{
						MyAnimations.SwitchToIdleAnim(MyAnimations.IsFacingRight);
					}
					if (targetPosIndicator != null) Destroy(targetPosIndicator.gameObject);
				}
				else
				{
					MyTransform.position = new Vector3(newX, newY + GroundHeightOffset, pos.z);
				}
			}
		}
		ClickableObject[] objectList = GameObject.FindObjectsOfType(typeof(ClickableObject)) as ClickableObject[];
		bool buttonOn = false;
		foreach (ClickableObject co in objectList){
			float dist = Vector3.Distance(co.MyTransform.position, this.MyTransform.position);
			if (dist < AlertButtonDistance){
				MyAnimations.SetAlertButtonOn();
				buttonOn = true;
				break;
			}
		}
		if (!buttonOn){
			foreach (ClickableObject co in objectList){
				float dist = Vector3.Distance(co.MyTransform.position, this.MyTransform.position);
				if (dist < AlertDistance){
					MyAnimations.SetAlertOn();
					buttonOn = true;
					break;
				}
			}
		}
		if (!buttonOn){
			MyAnimations.SetAlertOff();
		}

		//Update flashlight behavior.
		FlashlightCone.gameObject.SetActive(IsUsingFlashlight);
	}
}