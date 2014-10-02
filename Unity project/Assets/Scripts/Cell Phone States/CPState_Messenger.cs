using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Cellphone state for the messenger app.
/// </summary>
public class CPState_Messenger : CPState_Base
{
	public const int MaxMessages = 4;
	private static CellPhone.MessengerScreenData ScreenDat { get { return CellPhone.Instance.MessengerScreen; } }

	[Serializable]
	public struct Message
	{
		public string Text;
		public bool FromYou;
		public Message(string text, bool fromYou) { Text = text; FromYou = fromYou; }
	}
	public static List<Message> Messages = new List<Message>();


	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		GUIBackground(data, ScreenDat.Background);

		for (int i = MaxMessages - 1; i >= 0; --i) if (i < Messages.Count)
		{
			int index = Messages.Count - 1 - i;

			float x = ScreenDat.MessageXBorderLerp + ScreenDat.MessageXBorderLerp;
			float width = 0.5f - x;
			if (Messages[index].FromYou)
			{
				width = 0.5f - ScreenDat.MessageXBorderLerp - ScreenDat.MessageXOffsetLerp;
				x = 0.5f;
			}

			float y = ScreenDat.FirstMessageYLerp + (i * ScreenDat.MessageSeparationLerp);

			GUI.Box(new Rect(Mathf.Lerp(data.MinPos.x, data.MaxPos.x, x),
							 Mathf.Lerp(data.MinPos.y, data.MaxPos.y, y),
							 width * (data.MaxPos.x - data.MinPos.x),
							 ScreenDat.MessageHeightLerp * (data.MaxPos.y - data.MinPos.y)),
					Messages[index].Text,
					(Messages[index].FromYou ? ScreenDat.MessageBoxYou : ScreenDat.MessageBoxThem));
		}

		return this;
	}
}