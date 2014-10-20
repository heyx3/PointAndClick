using System;
using UnityEngine;


/// <summary>
/// Only one of this component can exist at once.
/// Represents the target object that has to be photographed.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PhotographableObject : MonoBehaviour
{
	/// <summary>
	/// Static reference to the only instance of this component in the scene.
	/// </summary>
	public static PhotographableObject Instance { get; private set; }

	/// <summary>
	/// Removes this component from the current object it's attached to and
	/// attaches itself to the given new object.
	/// Clears the "OnPhotoTaken" event.
	/// </summary>
	public static void SwitchTo(GameObject newObj)
	{
		Destroy(Instance);
		newObj.AddComponent<PhotographableObject>();
	}
	/// <summary>
	/// Removes this component from the current object it's attached to.
	/// </summary>
	public static void Erase()
	{
		Destroy(Instance);
	}


	public Collider2D MyCollider { get; private set; }
	public Transform MyTransform { get; private set; }

	/// <summary>
	/// The signature for a function that reacts to a photograph being taken of this object.
	/// </summary>
	public delegate void ReactionToPhotoTaken();
	/// <summary>
	/// Raised when the player's cellphone takes a picture of this object.
	/// </summary>
	public event ReactionToPhotoTaken OnPhotoTaken;

	void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There are two photographable objects in the scene!");
			return;
		}

		Instance = this;

		MyCollider = collider2D;
		MyTransform = transform;
	}

	/// <summary>
	/// Gets whether this object is visible inside the given camera viewport.
	/// </summary>
	public bool IsInCamera(Camera cam)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2.0f;

		Vector3 center3 = camera.transform.position;
		Vector2 center = new Vector2(center3.x, center3.y),
				extents = 0.5f * new Vector2(cameraHeight * screenAspect, cameraHeight);

		return IsInRegion(new Rect(center.x - extents.x, center.y - extents.y,
								   extents.x * 2.0f, extents.y * 2.0f));
	}
	/// <summary>
	/// Gets whether any part of this object is touching the given region of world space.
	/// </summary>
	public bool IsInRegion(Rect region)
	{
		Bounds myBounds3D = MyCollider.bounds;
		Rect myBounds = new Rect(myBounds3D.min.x, myBounds3D.min.y,
								 myBounds3D.size.x, myBounds3D.size.y);

		return region.Overlaps(myBounds, true);
	}
}