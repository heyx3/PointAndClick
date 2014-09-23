using System;
using UnityEngine;


public class CPState_MainScreen : CPState_Base
{
	/// <summary>
	/// Draws and returns the value of a GUI.Button.
	/// </summary>
	/// <param name="pos">The position of the button as an interpolant between 0 and 1.</param>
	/// <returns>Whether the button was pressed.</returns>
	private bool Button(Vector2 pos, Texture2D tex)
	{
		Vector3 phoneScreenPos = WorldCam.WorldToScreenPoint(Cellphone.MyTransform.position);
		Vector2 phoneHalfSize = Cellphone.MySprite.bounds.extents;
		Vector2 minPos = new Vector2(phoneScreenPos.x - phoneHalfSize.x + Cellphone.SpriteBorderSize.x,
									 phoneScreenPos.y - phoneHalfSize.y + Cellphone.SpriteBorderSize.y),
				maxPos = new Vector2(phoneScreenPos.x + phoneHalfSize.x - Cellphone.SpriteBorderSize.x,
									 phoneScreenPos.y - phoneHalfSize.y - Cellphone.SpriteBorderSize.y);
		Vector2 buttonPos = new Vector2(Mathf.Lerp(minPos.x, maxPos.x, pos.x),
										Mathf.Lerp(minPos.y, maxPos.y, pos.y));
		Rect guiArea = new Rect(buttonPos.x - (0.5f * Cellphone.MainScreenButtonSize.x),
								buttonPos.y - (0.5f * Cellphone.MainScreenButtonSize.y),
								Cellphone.MainScreenButtonSize.x, Cellphone.MainScreenButtonSize.y);

		return GUI.Button(guiArea, tex, Cellphone.ButtonStyle);
	}
	public override CPState_Base Update()
	{
		//The screen buttons are arranged on a 3x4 grid.
		float[] Xs = { 0.0f, 0.5f, 1.0f },
				Ys = { 0.0f, 0.33333f, 0.666666f, 1.0f };
		if (Button(new Vector2(Xs[0], Ys[0]), Cellphone.CallButtonTex))
			;
		if (Button(new Vector2(Xs[1], Ys[0]), Cellphone.ContactsButtonTex))
			;
		if (Button(new Vector2(Xs[2], Ys[0]), Cellphone.InternetButtonTex))
			;
		if (Button(new Vector2(Xs[0], Ys[1]), Cellphone.MessengerButtonTex))
			;
		if (Button(new Vector2(Xs[1], Ys[1]), Cellphone.ChatButtonTex))
			;
		if (Button(new Vector2(Xs[2], Ys[1]), Cellphone.MapsButtonTex))
			;
		if (Button(new Vector2(Xs[0], Ys[2]), Cellphone.FilesButtonTex))
			;
		if (Button(new Vector2(Xs[1], Ys[2]), Cellphone.FlashlightButtonTex))
			;
		if (Button(new Vector2(Xs[2], Ys[2]), Cellphone.WeatherButtonTex))
			;
		if (Button(new Vector2(Xs[0], Ys[3]), Cellphone.DatingButtonTex))
			;
		if (Button(new Vector2(Xs[1], Ys[3]), Cellphone.CalendarButtonTex))
			;
		if (Button(new Vector2(Xs[2], Ys[3]), Cellphone.SettingsButtonTex))
			;

		return this;
	}
}