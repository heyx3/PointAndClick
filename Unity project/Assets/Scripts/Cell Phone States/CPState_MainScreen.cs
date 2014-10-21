using System;
using UnityEngine;


public class CPState_MainScreen : CPState_Base
{
	/// <summary>
	/// Draws and returns the value of a GUI.Button.
	/// </summary>
	/// <param name="pos">The position of the button as an interpolant between 0 and 1.</param>
	/// <returns>Whether the button was pressed.</returns>
	private bool MainScreenButton(Vector2 posLerp, Texture2D tex, ScreenPositioningData data)
	{
		return data.GUIButton(posLerp, Cellphone.MainScreen.ButtonSize,
							  new Vector2(Cellphone.MainScreen.ScreenBorder.x * data.ScreenSizeScale.x,
							 			  Cellphone.MainScreen.ScreenBorder.y * data.ScreenSizeScale.y),
							  Cellphone.ButtonStyle, tex);
	}
	public override CPState_Base OnGUI(ScreenPositioningData data)
	{
		//The screen buttons are arranged on a 3x4 grid.
		float[] Xs = { 0.0f, 0.5f, 1.0f },
				Ys = { 1.0f, 0.33333f, 0.666666f, 0.0f };

		if (MainScreenButton(new Vector2(Xs[0], Ys[0]), Cellphone.MainScreen.CallButtonTex, data))
			return new CPState_Static(Cellphone.CallsScreen.Background);
		if (MainScreenButton(new Vector2(Xs[1], Ys[0]), Cellphone.MainScreen.ContactsButtonTex, data))
			return new CPState_Static(Cellphone.ContactsScreen.Background);
		if (MainScreenButton(new Vector2(Xs[2], Ys[0]), Cellphone.MainScreen.InternetButtonTex, data))
			return new CPState_Static(Cellphone.OfflineScreen.Background);
		if (MainScreenButton(new Vector2(Xs[0], Ys[1]), Cellphone.MainScreen.MessengerButtonTex, data))
			return new CPState_Messenger();
		if (MainScreenButton(new Vector2(Xs[1], Ys[1]), Cellphone.MainScreen.ChatButtonTex, data))
			return new CPState_Static(Cellphone.OfflineScreen.Background);
		if (MainScreenButton(new Vector2(Xs[2], Ys[1]), Cellphone.MainScreen.MapsButtonTex, data))
			return new CPState_Static(Cellphone.OfflineScreen.Background);
		if (MainScreenButton(new Vector2(Xs[0], Ys[2]), Cellphone.MainScreen.FilesButtonTex, data))
			;
		if (MainScreenButton(new Vector2(Xs[1], Ys[2]), Cellphone.MainScreen.FlashlightButtonTex, data))
		{
			PlayerInputController.Instance.IsUsingFlashlight = !PlayerInputController.Instance.IsUsingFlashlight;
			return null;
		}
		if (MainScreenButton(new Vector2(Xs[2], Ys[2]), Cellphone.MainScreen.WeatherButtonTex, data))
			return new CPState_Static(Cellphone.OfflineScreen.Background);
		if (MainScreenButton(new Vector2(Xs[0], Ys[3]), Cellphone.MainScreen.DatingButtonTex, data))
			return new CPState_Static(Cellphone.DatingScreen.Background);
		if (MainScreenButton(new Vector2(Xs[1], Ys[3]), Cellphone.MainScreen.CameraButtonTex, data))
			return new CPState_Camera();
		if (MainScreenButton(new Vector2(Xs[2], Ys[3]), Cellphone.MainScreen.SettingsButtonTex, data))
			;

		return this;
	}
}