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
							 Vector2 border, GUIStyle style, Texture2D tex)
	{
		Vector2 buttonPos = new Vector2(Mathf.Lerp(data.MinPos.x + border.x,
												   data.MaxPos.x - border.x,
												   posLerp.x),
										Mathf.Lerp(data.MinPos.y + border.y,
												   data.MaxPos.y - border.y,
												   posLerp.y));

		buttonSize.x *= data.ScreenSizeScale.x;
		buttonSize.y *= data.ScreenSizeScale.y;

		Rect guiArea = new Rect(buttonPos.x - (0.5f * buttonSize.x),
								buttonPos.y - (0.5f * buttonSize.y),
								buttonSize.x, buttonSize.y);

		GUI.DrawTexture(guiArea, tex);
		return GUI.Button(guiArea, "", style);
	}
	/// <summary>
	/// Draws a background for this phone screen.
	/// </summary>
	protected void GUIBackground(CellPhone.ButtonPositioningData data, Texture2D tex)
	{
		GUI.DrawTexture(new Rect(data.MinPos.x + Cellphone.BackgroundSpriteBorderSize.x,
								 data.MaxPos.y - Cellphone.BackgroundSpriteBorderSize.y,
								 data.MaxPos.x - data.MinPos.x - (2.0f * Cellphone.BackgroundSpriteBorderSize.x),
								 -data.MaxPos.y + data.MinPos.y + (2.0f * Cellphone.BackgroundSpriteBorderSize.y)),
						tex);
	}
}