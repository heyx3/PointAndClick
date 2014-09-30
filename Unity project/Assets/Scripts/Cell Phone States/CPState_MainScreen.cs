using System;
using UnityEngine;


public class CPState_MainScreen : CPState_Base
{
	/// <summary>
	/// Draws and returns the value of a GUI.Button.
	/// </summary>
	/// <param name="pos">The position of the button as an interpolant between 0 and 1.</param>
	/// <returns>Whether the button was pressed.</returns>
	private bool MainScreenButton(Vector2 posLerp, Texture2D tex, CellPhone.ButtonPositioningData data)
	{
		return GUIButton(posLerp, Cellphone.MainScreenButtonSize, data,
						 Cellphone.MainScreenSpriteBorderSize, Cellphone.ButtonStyle, tex);
	}
	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		//The screen buttons are arranged on a 3x4 grid.
		float[] Xs = { 0.0f, 0.5f, 1.0f },
				Ys = { 1.0f, 0.33333f, 0.666666f, 0.0f };

		if (MainScreenButton(new Vector2(Xs[0], Ys[0]), Cellphone.CallButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[1], Ys[0]), Cellphone.ContactsButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[2], Ys[0]), Cellphone.InternetButtonTex, data))
			return new CPState_Offline();
		if (MainScreenButton(new Vector2(Xs[0], Ys[1]), Cellphone.MessengerButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[1], Ys[1]), Cellphone.ChatButtonTex, data))
			return new CPState_Offline();
		if (MainScreenButton(new Vector2(Xs[2], Ys[1]), Cellphone.MapsButtonTex, data))
			return new CPState_Offline();
		if (MainScreenButton(new Vector2(Xs[0], Ys[2]), Cellphone.FilesButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[1], Ys[2]), Cellphone.FlashlightButtonTex, data))
		{
			PlayerInputController.Instance.IsUsingFlashlight = !PlayerInputController.Instance.IsUsingFlashlight;
			return null;
		}
		if (MainScreenButton(new Vector2(Xs[2], Ys[2]), Cellphone.WeatherButtonTex, data))
			return new CPState_Offline();
		if (MainScreenButton(new Vector2(Xs[0], Ys[3]), Cellphone.DatingButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[1], Ys[3]), Cellphone.CalendarButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[2], Ys[3]), Cellphone.SettingsButtonTex, data))
			;

		return this;
	}
}