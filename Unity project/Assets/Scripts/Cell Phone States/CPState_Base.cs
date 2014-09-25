using System;
using UnityEngine;


/// <summary>
/// Represents a cell phone screen.
/// </summary>
public abstract class CPState_Base
{
	protected CellPhone Cellphone { get { return CellPhone.Instance; } }
	protected Camera WorldCam { get { return MainCamera.Instance; } }


	/// <summary>
	/// Called on OnGUI(). Returns the next state to go to
	/// (or this object itself if state shouldn't change).
	/// </summary>
	public abstract CPState_Base OnGUI(CellPhone.ButtonPositioningData data);
	/// <summary>
	/// Called if this state is interrupted by closing the cell phone.
	/// Default behavior: does nothing.
	/// </summary>
	public virtual void OnInterrupt() { }


	
	/// <summary>
	/// Draws a GUI.Button with the given parameters and returns whether it was pressed.
	/// </summary>
	/// <param name="posLerp">Values between 0 and 1 representing where on the screen the button is.</param>
	protected bool GUIButton(Vector2 posLerp, Vector2 buttonSize,
							 CellPhone.ButtonPositioningData data,
							 GUIStyle style, Texture2D tex)
	{
		Vector2 buttonPos = new Vector2(Mathf.Lerp(data.MinPos.x, data.MaxPos.x, posLerp.x),
										Mathf.Lerp(data.MinPos.y, data.MaxPos.y, posLerp.y));
		Rect guiArea = new Rect(buttonPos.x - (0.5f * buttonSize.x),
								buttonPos.y - (0.5f * buttonSize.y),
								buttonSize.x, buttonSize.y);

		return GUI.Button(guiArea, tex, style);
	}
}