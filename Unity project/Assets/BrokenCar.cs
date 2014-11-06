using UnityEngine;
using System.Collections;

public class BrokenCar : MonoBehaviour {

	public GameObject SmokeAnim;

	// Use this for initialization
	void Start () {
		SmokeAnim.GetComponent<Animator>().speed = .5f;
		SoundAssets.Instance.PlaySound(SoundAssets.Instance.CarEngine);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
