using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Cellphone state for the messenger app.
/// </summary>
public class CPState_Messenger : CPState_Base
{
	private static CellPhone.MessengerScreenData ScreenDat { get { return CellPhone.Instance.MessengerScreen; } }


	/// <summary>
	/// The most recent message that was sent. Set to -1 if no messages have been sent yet.
	/// </summary>
	public int CurrentMessage = -1;


	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		Vector2 range = data.MaxPos - data.MinPos;

		GUIBackground(data, ScreenDat.Background);

		float y = ScreenDat.FirstMessageYLerp;
		for (int i = CurrentMessage; i >= 0; --i)
		{
			CellPhone.MessengerScreenData.Message msg = ScreenDat.Messages[i];

			float x = ScreenDat.MessageXOffsetLerp;
			if (msg.FromPlayer) x = 0.5f;

			GUI.Box(new Rect(Mathf.Lerp(data.MinPos.x, data.MaxPos.x, x),
							 Mathf.Lerp(data.MinPos.y, data.MaxPos.y, y),
							 msg.Image.width, msg.Image.height),
					msg.Image);

			y += (msg.Image.height / range.y) + ScreenDat.MessageSeparationLerp;
		}

		return this;
	}
}