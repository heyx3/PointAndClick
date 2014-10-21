using System;
using UnityEngine;


/// <summary>
/// Only one of this component can exist at once.
/// Represents the target object that has to be photographed.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class PhotographableObject : MonoBehaviour
{
	/// <summary>
	/// Static reference to the only instance of this component in the scene.
	/// </summary>
	public static PhotographableObject Instance { get; private set; }


	public Collider2D MyCollider { get; private set; }
	public Transform MyTransform { get; private set; }

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

		OnAwake();
	}

	/// <summary>
	/// Gets whether this object is visible inside the given camera viewport.
	/// </summary>
	public bool IsInCamera(Camera cam)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = cam.orthographicSize * 2.0f;

		Vector3 center3 = cam.transform.position;
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


	//Virtual/abstract functions.

	public virtual bool IsPhotographable() { return true; }
	public abstract void OnPhotographed();
	
	protected virtual void OnAwake() { }
}