using UnityEngine;
using System.Collections;


/// <summary>
/// Controls the player.
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
	public GameObject IdleAnim, WalkAnim;
	[System.NonSerialized]public GameObject CurrentlyActiveAnim = null;


	public Transform MyTransform { get { if (tr == null) tr = transform; return tr; } }
	private Transform tr = null;

	public bool IsFacingRight { get { return MyTransform.localScale.x > 0.0f; } }


	/// <summary>
	/// Makes sure this player is flipped to face along the negative or positive X.
	/// </summary>
	private void Flip(bool negative)
	{
		if ((negative && MyTransform.localScale.x > 0.0f) ||
			(!negative && MyTransform.localScale.x < 0.0f))
		{
			MyTransform.localScale = new Vector3(-MyTransform.localScale.x,
												 MyTransform.localScale.y, MyTransform.localScale.z);
		}
	}

	public void SwitchToIdleAnim(bool faceRight)
	{
		Flip(!faceRight);

		if (CurrentlyActiveAnim == IdleAnim) return;

		IdleAnim.SetActive(true);
		WalkAnim.SetActive(false);
		CurrentlyActiveAnim = IdleAnim;
	}
	public void SwitchToWalkAnim(bool faceRight)
	{
		Flip(!faceRight);

		if (CurrentlyActiveAnim == WalkAnim) return;

		IdleAnim.SetActive(false);
		WalkAnim.SetActive(true);
		CurrentlyActiveAnim = WalkAnim;
	}


	void Awake()
	{
		if (IdleAnim == null)
		{
			Debug.LogError("'IdleAnim' in PlayerAnimationController has not been assigned!");
			return;
		}
		if (WalkAnim == null)
		{
			Debug.LogError("'WalkAnim' in PlayerAnimationController has not been assigned!");
			return;
		}

		SwitchToIdleAnim(IsFacingRight);
	}
}