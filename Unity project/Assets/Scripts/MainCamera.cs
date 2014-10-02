using UnityEngine;


/// <summary>
/// Marks this camera as the main one for the scene.
/// Should only ever be one of these.
/// </summary>
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
	public static Camera Instance { get; private set; }


	public Transform MyTransform { get; private set; }


	void Awake()
	{
		Instance = camera;
		MyTransform = transform;

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

	void Update()
	{
		Vector3 pos = MyTransform.position,
				playerPos = PlayerInputController.Instance.MyTransform.position;
		MyTransform.position = new Vector3(playerPos.x, pos.y, pos.z);
	}
}