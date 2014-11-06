using UnityEngine;


/// <summary>
/// Stores all sound assets for the map.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SoundAssets : MonoBehaviour
{
	public static SoundAssets Instance { get; private set; }


	public AudioSource Source { get; private set; }

	public AudioClip CarEngine, BackgroundNoise, TextNotification,
					 TextSent, PhotoTaken, BadButton;


	void Awake()
	{
		Instance = this;
		Source = audio;
	}

	public void PlaySound(AudioClip clip)
	{
		Source.PlayOneShot(clip);
	}
}