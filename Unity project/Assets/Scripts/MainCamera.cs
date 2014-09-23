using UnityEngine;


/// <summary>
/// Marks this camera as the main one for the scene.
/// Should only ever be one of these.
/// </summary>
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
	public static Camera Instance { get; private set; }

	void Awake()
	{
		Instance = camera;

		//Make sure there aren't more of these.
		foreach (MainCamera mc in FindObjectsOfType<MainCamera>())
		{
			if (mc != this)
			{
				Debug.LogError("There is more than one 'main camera'!");
				return;
			}
		}
	}
}