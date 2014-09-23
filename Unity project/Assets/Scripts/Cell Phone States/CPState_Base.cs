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
	public abstract CPState_Base OnGUI();
	/// <summary>
	/// Called if this state is interrupted by closing the cell phone.
	/// Default behavior: does nothing.
	/// </summary>
	public virtual void OnInterrupt() { }


	/// <summary>
	/// Performs some heavy calculations to get the min/max position available
	/// for buttons on the phone screen (in GUI space).
	/// Intended use is to be constructed once every OnGUI()
	/// and used for positioning all cell phone buttons.
	/// </summary>
	protected class ButtonPositioningData
	{
		public Vector2 MinPos, MaxPos;
		public ButtonPositioningData(Camera worldCam)
		{
			Vector3 screenPos = worldCam.WorldToScreenPoint(CellPhone.Instance.MyTransform.position);
			Vector2 halfSize = CellPhone.Instance.MySprite.bounds.extents,
					size = 2.0f * halfSize;
			MinPos = new Vector2(screenPos.x - halfSize.x + CellPhone.Instance.SpriteBorderSize.x,
								 screenPos.y - halfSize.y + CellPhone.Instance.SpriteBorderSize.y);
			MaxPos = new Vector2(screenPos.x + halfSize.x - CellPhone.Instance.SpriteBorderSize.x,
								 screenPos.y + halfSize.y -	CellPhone.Instance.SpriteBorderSize.y);

			MinPos.y = Screen.height - MinPos.y;
			MaxPos.y = Screen.height - MaxPos.y;
		}
	}
	/// <summary>
	/// Draws a GUI.Button with the given parameters and returns whether it was pressed.
	/// </summary>
	/// <param name="posLerp">Values between 0 and 1 representing where on the screen the button is.</param>
	protected bool GUIButton(Vector2 posLerp, Vector2 buttonSize, ButtonPositioningData data,
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