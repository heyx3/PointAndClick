using UnityEngine;
using System.Collections;


/// <summary>
/// Controls the player.
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
	public GameObject IdleAnim, WalkAnim, IdleLightAnim, IdlePhoneAnim, WalkLightAnim, WalkPhoneAnim, AlertAnim, AlertButtonAnim;
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
		
		WalkAnim.SetActive(false);
		IdleLightAnim.SetActive(false);
		IdlePhoneAnim.SetActive(false); 
		WalkLightAnim.SetActive(false); 
		WalkPhoneAnim.SetActive(false);

		IdleAnim.SetActive(true);
		CurrentlyActiveAnim = IdleAnim;
	}
	public void SwitchToWalkAnim(bool faceRight)
	{
		Flip(!faceRight);

		if (CurrentlyActiveAnim == WalkAnim) return;

		IdleAnim.SetActive(false);
		IdleLightAnim.SetActive(false);
		IdlePhoneAnim.SetActive(false); 
		WalkLightAnim.SetActive(false); 
		WalkPhoneAnim.SetActive(false);

		WalkAnim.SetActive(true);
		CurrentlyActiveAnim = WalkAnim;
	}
	public void SwitchToIdleLightAnim(bool faceRight)
	{
		Flip(!faceRight);
		
		if (CurrentlyActiveAnim == IdleLightAnim) return;
		
		WalkAnim.SetActive(false);
		IdleAnim.SetActive(false);
		IdlePhoneAnim.SetActive(false); 
		WalkLightAnim.SetActive(false); 
		WalkPhoneAnim.SetActive(false);
		
		IdleLightAnim.SetActive(true);
		CurrentlyActiveAnim = IdleLightAnim;
	}
	public void SwitchToWalkLightAnim(bool faceRight)
	{
		Flip(!faceRight);
		
		if (CurrentlyActiveAnim == WalkLightAnim) return;
		
		IdleAnim.SetActive(false);
		IdleLightAnim.SetActive(false);
		IdlePhoneAnim.SetActive(false); 
		WalkAnim.SetActive(false); 
		WalkPhoneAnim.SetActive(false);
		
		WalkLightAnim.SetActive(true);
		CurrentlyActiveAnim = WalkLightAnim;
	}

	public void SwitchToIdlePhoneAnim(bool faceRight)
	{
		Flip(!faceRight);
		
		if (CurrentlyActiveAnim == IdlePhoneAnim) return;
		
		WalkAnim.SetActive(false);
		IdleLightAnim.SetActive(false);
		IdleAnim.SetActive(false); 
		WalkLightAnim.SetActive(false); 
		WalkPhoneAnim.SetActive(false);
		
		IdlePhoneAnim.SetActive(true);
		CurrentlyActiveAnim = IdlePhoneAnim;
	}
	public void SwitchToWalkPhoneAnim(bool faceRight)
	{
		Flip(!faceRight);
		
		if (CurrentlyActiveAnim == WalkPhoneAnim) return;
		
		IdleAnim.SetActive(false);
		IdleLightAnim.SetActive(false);
		IdlePhoneAnim.SetActive(false); 
		WalkLightAnim.SetActive(false); 
		WalkAnim.SetActive(false);
		
		WalkPhoneAnim.SetActive(true);
		CurrentlyActiveAnim = WalkPhoneAnim;
	}

	public void SetAlertOff(){
		AlertAnim.SetActive(false);
		AlertButtonAnim.SetActive(false);
	}

	public void SetAlertOn(){
		AlertButtonAnim.SetActive(false);
		AlertAnim.SetActive(true);
	}

	public void SetAlertButtonOn(){
		if (AlertButtonAnim.activeSelf)
			return;
		else {
		AlertAnim.SetActive(false);
		AlertButtonAnim.SetActive(true);
		AlertButtonAnim.GetComponent<Animator>().Play("AlertButton");
		}
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
		if (IdleLightAnim == null)
		{
			Debug.LogError("'IdleLightAnim' in PlayerAnimationController has not been assigned!");
			return;
		}
		if (WalkLightAnim == null)
		{
			Debug.LogError("'WalkLightAnim' in PlayerAnimationController has not been assigned!");
			return;
		}
		if (IdlePhoneAnim == null)
		{
			Debug.LogError("'IdlePhoneAnim' in PlayerAnimationController has not been assigned!");
			return;
		}
		if (WalkPhoneAnim == null)
		{
			Debug.LogError("'WalkPhoneAnim' in PlayerAnimationController has not been assigned!");
			return;
		}
		if (AlertAnim == null)
		{
			Debug.LogError("'AlertAnim' in PlayerAnimationController has not been assigned!");
			return;
		}
		if (AlertButtonAnim == null)
		{
			Debug.LogError("'AlertButtonAnim' in PlayerAnimationController has not been assigned!");
			return;
		}

		IdleAnim.GetComponent<Animator>().speed = .5f;
		IdleLightAnim.GetComponent<Animator>().speed = .5f;
		IdlePhoneAnim.GetComponent<Animator>().speed = .5f;

		SwitchToIdleAnim(IsFacingRight);
		AlertAnim.SetActive(false);
		AlertButtonAnim.SetActive(false);
	}
}