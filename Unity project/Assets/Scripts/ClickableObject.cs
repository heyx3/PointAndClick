using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// An object that can be interacted with.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class ClickableObject : MonoBehaviour
{
	/// <summary>
	/// All currently-clickable objects.
	/// </summary>
	public static List<ClickableObject> CurrentObjects = new List<ClickableObject>();


	/// <summary>
	/// Cached reference to this object's Transform.
	/// </summary>
	public Transform MyTransform { get; private set; }
	/// <summary>
	/// Cached reference to this object's collider.
	/// Calculated on Awake().
	/// </summary>
	public Collider2D MyCollider { get; private set; }

	public bool BeenClicked = false;

	void Awake()
	{
		MyCollider = collider2D;
		MyTransform = transform;

		CurrentObjects.Add(this);
		
		ChildAwake();
	}
	void OnDestroy()
	{
		CurrentObjects.Remove(this);
	}


	public abstract void OnClicked(Vector2 mouse, Inventory.InventoryObjects? currentlySelected);
	protected virtual void ChildAwake() { }
}