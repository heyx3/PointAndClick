using System;
using UnityEngine;


public class CPState_Camera : CPState_Base
{
	private CellPhone.CameraScreenData ScreenDat { get { return Cellphone.CameraScreen; } }


	public override CPState_Base OnGUI(ScreenPositioningData data)
	{
		data.GUIBackground(ScreenDat.Background, Cellphone.BackgroundSpriteBorderSize,
						   Cellphone.BackgroundSpriteOffset);

		PhotographableObject obj = PhotographableObject.Instance;
		bool canBePhotographed = (obj != null && obj.IsPhotographable() && obj.IsInCamera(MainCamera.Instance));

		if (canBePhotographed)
			data.GUITexture(new Vector2(0.5f, ScreenDat.DisplayTexYOffsetLerp), ScreenDat.DisplayTex);

		if (data.GUIButton(ScreenDat.PhotoButtonPosLerp,
						   new Vector2(ScreenDat.PhotoButton.width,
									   ScreenDat.PhotoButton.height),
						   new Vector2(), Cellphone.ButtonStyle, ScreenDat.PhotoButton))
		{
			if (canBePhotographed)
			{
				obj.OnPhotographed();
				return null;
			}
		}


		return this;
	}
}