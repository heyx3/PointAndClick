using System;
using UnityEngine;


/// <summary>
/// Represents a cell phone screen.
/// </summary>
public abstract class CPState_Base
{
	private static float Lerp(float min, float max, float t)
	{
		return min + (t * (max - min));
	}


	protected CellPhone Cellphone { get { return CellPhone.Instance; } }
	protected Camera WorldCam { get { return MainCamera.Instance; } }


	/// <summary>
	/// Called on OnGUI(). Returns the next state to go to
	/// (or this object itself if state shouldn't change).
	/// </summary>
	public abstract CPState_Base OnGUI(ScreenPositioningData data);
	/// <summary>
	/// Called if this state is interrupted by closing the cell phone.
	/// Returns whether the interrupt should be ignored.
	/// Default behavior: returns false.
	/// </summary>
	public virtual bool OnInterrupt() { return false; }
}