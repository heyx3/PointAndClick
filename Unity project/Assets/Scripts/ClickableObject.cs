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
	/// Cached reference to this object's collider.
	/// Calculated on Awake().
	/// </summary>
	public Collider2D MyCollider { get; private set; }


	void Awake()
	{
		MyCollider = collider2D;

		ChildAwake();
	}


	public abstract void OnClicked(Vector2 mouse);
	protected virtual void ChildAwake() { }
}