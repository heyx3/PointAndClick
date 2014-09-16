using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Manages all aspects of the player's mouse input.
/// </summary>
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerInputController : MonoBehaviour
{
	public Transform ClickableObjectsContainer = null;
	public GameObject TargetPosIndicatorPrefab = null;

	/// <summary>
	/// The player's walking speed.
	/// </summary>
	public float WalkSpeed = 40.0f;
	/// <summary>
	/// The distance to the target pos at which the player stops moving.
	/// </summary>
	public float MaxDistToTarget = 5.0f;


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

	private Transform targetPosIndicator = null;


	void Awake()
	{
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
	}

	private void GenerateTargetPosIndicator(float x)
	{
		if (targetPosIndicator == null)
			targetPosIndicator = ((GameObject)Instantiate(TargetPosIndicatorPrefab)).transform;

		targetPosIndicator.position = new Vector3(x, targetPosIndicator.position.y, targetPosIndicator.position.z);
	}
	void Update()
	{
		//First update mouse input.
		if (Input.GetMouseButton(0))
		{
			Vector2 mouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//First see if any objects were clicked on.
			foreach (ClickableObject co in ClickableObjects)
			{
				if (co.MyCollider.OverlapPoint(mouse))
				{
					co.OnClicked(mouse);
					return;
				}
			}

			//Next see if the cell phone was clicked on.
			if (CellPhone.Instance != null && CellPhone.Instance.MyCollider.OverlapPoint(mouse))
			{
				CellPhone.Instance.OnClicked(mouse);
				return;
			}

			//Finally, just interpret the mouse click as a movement input.
			float worldX = (Screen.width * -0.5f) + Mathf.Clamp(mouse.x, 0.0f, Screen.width);
			GenerateTargetPosIndicator(worldX);
		}


		//Now update the movement logic.
		if (targetPosIndicator == null)
		{
			MyAnimations.SwitchToIdleAnim();
		}
		else
		{
			float towardsTarget = targetPosIndicator.position.x - MyTransform.position.x;
			if (Mathf.Abs(towardsTarget) < MaxDistToTarget)
			{
				MyAnimations.SwitchToIdleAnim();
				if (targetPosIndicator != null) Destroy(targetPosIndicator.gameObject);
			}
			else if (towardsTarget < 0.0f)
			{
				MyAnimations.SwitchToWalkLeftAnim();
				MyTransform.position += new Vector3(-WalkSpeed * Time.deltaTime, 0.0f, 0.0f);
			}
			else
			{
				MyAnimations.SwitchToWalkRightAnim();
				MyTransform.position += new Vector3(WalkSpeed * Time.deltaTime, 0.0f, 0.0f);
			}
		}
	}
}