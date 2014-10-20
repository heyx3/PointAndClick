using UnityEngine;


/// <summary>
/// Performs a lot of calculations to get the min/max position available
/// for a collider in GUI space.
/// </summary>
public class ScreenPositioningData
{
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
}