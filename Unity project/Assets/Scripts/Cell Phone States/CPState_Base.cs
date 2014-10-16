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
	public abstract CPState_Base OnGUI(CellPhone.ButtonPositioningData data);
	/// <summary>
	/// Called if this state is interrupted by closing the cell phone.
	/// Returns whether the interrupt should be ignored.
	/// Default behavior: returns false.
	/// </summary>
	public virtual bool OnInterrupt() { return false; }


	
	/// <summary>
	/// Draws a GUI.Button with the given parameters and returns whether it was pressed.
	/// </summary>
	/// <param name="posLerp">Values between 0 and 1 representing where on the screen the button is.</param>
	protected bool GUIButton(Vector2 posLerp, Vector2 buttonSize,
							 CellPhone.ButtonPositioningData data,
							 Vector2 border, GUIStyle style, Texture2D tex)
	{
		Vector2 buttonPos = new Vector2(Lerp(data.MinPos.x + border.x,
											 data.MaxPos.x - border.x,
											 posLerp.x),
										Lerp(data.MinPos.y + border.y,
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
		Vector2 border = new Vector2(data.ScreenSizeScale.x * Cellphone.BackgroundSpriteBorderSize.x,
									 data.ScreenSizeScale.y * Cellphone.BackgroundSpriteBorderSize.y);
		Vector2 offset = new Vector2(data.ScreenSizeScale.x * Cellphone.BackgroundSpriteOffset.x,
									 data.ScreenSizeScale.y * Cellphone.BackgroundSpriteOffset.y);
		GUI.DrawTexture(new Rect(data.MinPos.x + border.x + offset.x,
								 data.MaxPos.y - border.y + offset.y,
								 data.MaxPos.x - data.MinPos.x - (2.0f * border.x),
								 -data.MaxPos.y + data.MinPos.y + (2.0f * border.y)),
						tex);
	}
	/// <summary>
	/// Draws a texture on this phone screen.
	/// </summary>
	protected void GUITexture(Vector2 centerLerp, CellPhone.ButtonPositioningData data, Texture2D tex)
	{
		Vector2 pos = new Vector2(Lerp(data.MinPos.x, data.MaxPos.x, centerLerp.x),
								  Lerp(data.MinPos.y, data.MaxPos.y, centerLerp.y));
		Vector2 size = new Vector2(data.ScreenSizeScale.x * tex.width,
								   data.ScreenSizeScale.y * tex.height);
		GUI.DrawTexture(new Rect(pos.x - (0.5f * size.x), pos.y - (0.5f * size.y),
								 size.x, size.y),
						tex);
	}
	/// <summary>
	/// Draws a label on this phone screen.
	/// </summary>
	protected void GUILabel(Vector2 topLeftLerp, Vector2 size,
							CellPhone.ButtonPositioningData data,
							GUIStyle style, string text)
	{
		Vector2 topLeft = new Vector2(Lerp(data.MinPos.x, data.MaxPos.x, topLeftLerp.x),
									  Lerp(data.MinPos.y, data.MaxPos.y, topLeftLerp.y));

		GUI.Label(new Rect(topLeft.x, topLeft.y,
						   size.x * data.ScreenSizeScale.x,
						   size.y * data.ScreenSizeScale.y),
				  text, style);
	}
}