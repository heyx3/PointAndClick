using UnityEngine;


/// <summary>
/// Performs a lot of calculations to get the min/max position available
/// for a collider in GUI space.
/// </summary>
public class ScreenPositioningData
{
	/// <summary>
	/// Unity's Mathf.Lerp automatically clamps t to 0-1 for some strange reason.
	/// </summary>
	private static float Lerp(float min, float max, float t)
	{
		return min + (t * (max - min));
	}
	/// <summary>
	/// Unity's Mathf.InverseLerp automatically clamps to 0-1 for some strange reason.
	/// </summary>
	private static float InverseLerp(float min, float max, float val)
	{
		return (val - min) / (max - min);
	}
	private static RenderTexture GameRendTex { get { return MainCamera.Instance.targetTexture; } }

	public Vector2 MinPos, MaxPos;
	public Vector2 ScreenSizeScale;

	public ScreenPositioningData(Collider2D objCollider)
	{
		Bounds objBounds = objCollider.bounds;

		ScreenSizeScale = new Vector2((float)Screen.width / (float)GameRendTex.width,
									  (float)Screen.height / (float)GameRendTex.height);
		ScreenSizeScale *= ((float)GameRendTex.height * 0.5f) / MainCamera.Instance.orthographicSize;

		Vector3 screenPos = MainCamera.Instance.WorldToScreenPoint(objBounds.center);
		screenPos.x *= ((float)Screen.width / (float)MainCamera.Instance.targetTexture.width);
		screenPos.y *= ((float)Screen.height / (float)MainCamera.Instance.targetTexture.height);

		Vector2 size = new Vector2(ScreenSizeScale.x * objBounds.size.x,
								   ScreenSizeScale.y * objBounds.size.y),
				halfSize = 0.5f * size;

		MinPos = new Vector2(screenPos.x - halfSize.x, screenPos.y - halfSize.y);
		MaxPos = new Vector2(screenPos.x + halfSize.x, screenPos.y + halfSize.y);

		MinPos.y = Screen.height - MinPos.y;
		MaxPos.y = Screen.height - MaxPos.y;
	}


	/// <summary>
	/// Converts the given screen position to an interpolant for this instance's MinPos/MaxPos fields.
	/// </summary>
	public Vector2 GetLerpPos(Vector2 screenPos)
	{
		return new Vector2(InverseLerp(MinPos.x, MaxPos.x, screenPos.x),
						   InverseLerp(MinPos.y, MaxPos.y, screenPos.y));
	}
	/// <summary>
	/// Gets the given X and Y size relative to the
	/// size of the region bounded by this instance's MinPos/MaxPos.
	/// </summary>
	public Vector2 GetLerpSize(Vector2 screenSize)
	{
		return new Vector2(InverseLerp(0.0f, MaxPos.x - MinPos.x, screenSize.x),
						   InverseLerp(0.0f, MaxPos.y - MinPos.y, screenSize.y));
	}

	/// <summary>
	/// Draws a GUI.Button with the given parameters and returns whether it was pressed.
	/// </summary>
	/// <param name="posLerp">Values between 0 and 1 representing where on the screen the button is.</param>
	public bool GUIButton(Vector2 posLerp, Vector2 buttonSize, Vector2 border,
							 GUIStyle style, Texture2D tex)
	{
		Vector2 buttonPos = new Vector2(Lerp(MinPos.x + border.x,
											 MaxPos.x - border.x,
											 posLerp.x),
										Lerp(MinPos.y + border.y,
											 MaxPos.y - border.y,
											 posLerp.y));

		buttonSize.x *= ScreenSizeScale.x;
		buttonSize.y *= ScreenSizeScale.y;

		Rect guiArea = new Rect(buttonPos.x - (0.5f * buttonSize.x),
								buttonPos.y - (0.5f * buttonSize.y),
								buttonSize.x, buttonSize.y);

		GUI.DrawTexture(guiArea, tex);
		return GUI.Button(guiArea, "", style);
	}
	/// <summary>
	/// Draws a background for this phone screen.
	/// Can scale in the background to fit inside a border and then
	/// offset the background by a certain amount.
	/// </summary>
	public void GUIBackground(Texture2D tex, Vector2 borderSize, Vector2 offset)
	{
		Vector2 border = new Vector2(ScreenSizeScale.x * borderSize.x,
									 ScreenSizeScale.y * borderSize.y);
		offset = new Vector2(ScreenSizeScale.x * offset.x, ScreenSizeScale.y * offset.y);
		GUI.DrawTexture(new Rect(MinPos.x + border.x + offset.x,
								 MaxPos.y - border.y + offset.y,
								 MaxPos.x - MinPos.x - (2.0f * border.x),
								 -MaxPos.y + MinPos.y + (2.0f * border.y)),
						tex);
	}
	/// <summary>
	/// Draws a texture on this phone screen given the position of the texture's center.
	/// </summary>
	public void GUITexture(Vector2 centerLerp, Texture2D tex)
	{
		Vector2 pos = new Vector2(Lerp(MinPos.x, MaxPos.x, centerLerp.x),
								  Lerp(MinPos.y, MaxPos.y, centerLerp.y));
		Vector2 size = new Vector2(ScreenSizeScale.x * tex.width,
								   ScreenSizeScale.y * tex.height);
		GUI.DrawTexture(new Rect(pos.x - (0.5f * size.x), pos.y - (0.5f * size.y),
								 size.x, size.y),
						tex);
	}
	/// <summary>
	/// Draws a texture on this phone screen given the position of the texture's top-left corner.
	/// </summary>
	public void GUITexture(float minXLerp, float minYLerp, Texture2D tex)
	{
		Vector2 texLerpSize = GetLerpSize(new Vector2(tex.width * ScreenSizeScale.x,
													  tex.height * ScreenSizeScale.y));
		Vector2 centerLerp = new Vector2(minXLerp + (0.5f * texLerpSize.x),
										 minYLerp + (0.5f * texLerpSize.y));
		GUITexture(centerLerp, tex);
	}
	/// <summary>
	/// Draws a label on this phone screen.
	/// </summary>
	public void GUILabel(Vector2 topLeftLerp, Vector2 size, GUIStyle style, string text)
	{
		Vector2 topLeft = new Vector2(Lerp(MinPos.x, MaxPos.x, topLeftLerp.x),
									  Lerp(MinPos.y, MaxPos.y, topLeftLerp.y));

		GUI.Label(new Rect(topLeft.x, topLeft.y,
						   size.x * ScreenSizeScale.x,
						   size.y * ScreenSizeScale.y),
				  text, style);
	}
}