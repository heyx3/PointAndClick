using UnityEngine;
using System.Collections;


/// <summary>
/// Controls the player.
/// </summary>
public class PlayerController : MonoBehaviour
{
	public GameObject IdleAnim, WalkLeftAnim, WalkRightAnim;
	[System.NonSerialized]public GameObject CurrentlyActiveAnim = null;


	public void SwitchToIdleAnim()
	{
		IdleAnim.SetActive(true);
		WalkLeftAnim.SetActive(false);
		WalkRightAnim.SetActive(false);
		CurrentlyActiveAnim = IdleAnim;
	}
	public void SwitchToWalkLeftAnim()
	{
		IdleAnim.SetActive(false);
		WalkLeftAnim.SetActive(true);
		WalkRightAnim.SetActive(false);
		CurrentlyActiveAnim = WalkLeftAnim;
	}
	public void SwitchToWalkRightAnim()
	{
		IdleAnim.SetActive(false);
		WalkLeftAnim.SetActive(false);
		WalkRightAnim.SetActive(true);
		CurrentlyActiveAnim = WalkRightAnim;
	}


	void Awake()
	{
		SwitchToIdleAnim();
	}
}