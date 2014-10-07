using UnityEngine;


/// <summary>
/// Marks this camera as the main one for the scene.
/// Should only ever be one of these.
/// </summary>
[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
	public static Camera Instance { get; private set; }


	/// <summary>
	/// The background object that represents the farthest horizontal extents of the camera.
	/// </summary>
	public Transform MainBackground = null;


	public Transform MyTransform { get; private set; }


	void Awake()
	{
		Instance = camera;
		MyTransform = transform;

		if (MainBackground == null)
		{
			Debug.LogError("The 'MainBackground' field of the MainCamera component in the '" +
								gameObject.name + "' object is null!");
			return;
		}

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

		//Keep the camera from going outside the game bounds.
		float camViewWidth = 2.0f * MainCamera.Instance.orthographicSize *
							 (float)MainCamera.Instance.targetTexture.width /
								(float)MainCamera.Instance.targetTexture.height;
		float camX = playerPos.x;
		float minCamView = camX - (camViewWidth * 0.5f),
			  maxCamView = camX + (camViewWidth * 0.5f);

		float backgroundX = MainBackground.position.x,
			  backgroundWidth = MainBackground.lossyScale.x;
		float minBackground = backgroundX - (backgroundWidth * 0.5f),
			  maxBackground = backgroundX + (backgroundWidth * 0.5f);

		float camMinToBackMin = minBackground - minCamView,
			  camMaxToBackMax = maxBackground - maxCamView;

		//If the camera is off the left side, push it right.
		if (camMinToBackMin > 0.1f)
		{
			MyTransform.position = new Vector3(playerPos.x + camMinToBackMin, pos.y, pos.z);
		}
		//Otherwise, if it's off the right side, push it left.
		else if (camMaxToBackMax < -0.1f)
		{
			MyTransform.position = new Vector3(playerPos.x + camMaxToBackMax, pos.y, pos.z);
		}
		//Otherwise, just track the player perfectly.
		else
		{
			MyTransform.position = new Vector3(playerPos.x, pos.y, pos.z);
		}
	}
}