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
	public static int CurrentMessage = 0;

	private enum ScreenState
	{
		/// <summary>
		/// Nothing's going on.
		/// </summary>
		Idle,
		/// <summary>
		/// The player is typing a reply.
		/// </summary>
		TypingReply,
		/// <summary>
		/// She is about to send a message.
		/// </summary>
		WaitingForReply,
	}
	private ScreenState CurrentState = ScreenState.Idle;

	private int typedLetterIndex = -1;
	private float nextTypedLetter = -1.0f;

	private float currentReplyWait = -1.0f;

	private Vector2 scrollViewPos = Vector2.zero;
	


	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		//Render messages.

		Vector2 range = data.MaxPos - data.MinPos;
		GUIBackground(data, ScreenDat.Background);

		//Figure out spacing stuff for the scrollable view that messages are rendered in.
		const float extraMsgSpace = 1000.0f;
		Vector2 screenMsgAreaSize = range;
		float borderSize = screenMsgAreaSize.x * ScreenDat.MessageXBorderLerp;
		screenMsgAreaSize.x -= 2.0f * borderSize;
		screenMsgAreaSize.y *= -1.0f;
		screenMsgAreaSize.y -= (data.ScreenSizeScale.y * Cellphone.BackgroundSpriteOffset.y) +
							   (screenMsgAreaSize.y * (1.0f - ScreenDat.FirstMessageYLerp));
		Vector2 screenCellTopLeft = new Vector2(data.MinPos.x + borderSize,
												Mathf.Lerp(data.MinPos.y, data.MaxPos.y,
														   Cellphone.BackgroundSpriteOffset.y * data.ScreenSizeScale.y));
		Vector2 screenCellBottomRight = screenCellTopLeft + screenMsgAreaSize;
		scrollViewPos = GUI.BeginScrollView(new Rect(screenCellTopLeft.x, screenCellTopLeft.y,
													 screenMsgAreaSize.x, screenMsgAreaSize.y),
											scrollViewPos,
											new Rect(screenCellTopLeft.x, screenCellTopLeft.y - extraMsgSpace,
													 screenMsgAreaSize.x, screenMsgAreaSize.y + extraMsgSpace));
		//Now render the messages into the scrollable view.
		float y = 1.0f - ScreenDat.FirstMessageYLerp;
		for (int i = CurrentMessage; i >= 0; --i)
		{
			CellPhone.MessengerScreenData.Message msg = ScreenDat.Messages[i];

			Vector2 texSize = new Vector2(msg.Image.width, msg.Image.height);
			Vector2 texLerpSize = new Vector2(Mathf.InverseLerp(0.0f, data.MaxPos.x - data.MinPos.x,
																(texSize.x * data.ScreenSizeScale.x)),
												 Mathf.InverseLerp(0.0f, data.MinPos.y - data.MaxPos.y,
																(texSize.y * data.ScreenSizeScale.y)));
			if (msg.FromPlayer)
			{
				GUITexture(new Vector2(1.0f - ScreenDat.MessageXBorderLerp - (0.5f * texLerpSize.x),
									   y + (0.5f * texLerpSize.y)),
						   data, msg.Image);
			}
			else
			{
				GUITexture(new Vector2(ScreenDat.MessageXBorderLerp + (0.5f * texLerpSize.x),
									   y + (0.5f * texLerpSize.y)),
						   data, msg.Image);
			}

			float heightLerp = Mathf.InverseLerp(0.0f, data.MinPos.y - data.MaxPos.y, (texSize.y * data.ScreenSizeScale.y));
			y += ScreenDat.MessageSeparationLerp + heightLerp;
		}

		GUI.EndScrollView();


		//Render player-typed text.
		if (CurrentMessage < ScreenDat.Messages.Length - 1 &&
			ScreenDat.Messages[CurrentMessage + 1].MessageText.Length > 0 &&
			CurrentState == ScreenState.TypingReply)
		{
			Vector2 dims = ScreenDat.MessageBoxBottomRightLerp - ScreenDat.MessageBoxTopLeftLerp;
			dims.y = -dims.y;

			dims.x *= 320.0f;
			dims.y *= 200.0f;

			GUILabel(ScreenDat.MessageBoxTopLeftLerp, dims, data, Cellphone.SmallTextStyle,
					 ScreenDat.Messages[CurrentMessage + 1].MessageText.Substring(0, typedLetterIndex));
		}


		//Render message button if player needs to type text.
		if (CurrentState == ScreenState.Idle &&
			ScreenDat.Messages.Length > CurrentMessage + 1 &&
			ScreenDat.Messages[CurrentMessage + 1].FromPlayer)
		{
			if (GUIButton(ScreenDat.MessageButtonCenterLerp,
						  new Vector2(ScreenDat.NewSendMessage.width, ScreenDat.NewSendMessage.height),
						  data, new Vector2(), Cellphone.ButtonStyle, ScreenDat.NewSendMessage))
			{
				typedLetterIndex = 0;
				nextTypedLetter = ScreenDat.PlayerTypeInterval;
				CurrentState = ScreenState.TypingReply;
			}
		}
		else GUITexture(ScreenDat.MessageButtonCenterLerp, data, ScreenDat.NoSendMessage);


		//Update screen state.
		switch (CurrentState)
		{
			case ScreenState.Idle: break;

			case ScreenState.TypingReply:
				nextTypedLetter -= Time.deltaTime;
				if (nextTypedLetter <= 0.0f)
				{
					typedLetterIndex += 1;
					nextTypedLetter += Cellphone.MessengerScreen.PlayerTypeInterval;
					if (typedLetterIndex >= ScreenDat.Messages[CurrentMessage + 1].MessageText.Length)
					{
						NextMessage();
					}
				}
				break;

			case ScreenState.WaitingForReply:
				currentReplyWait -= Time.deltaTime;
				if (currentReplyWait <= 0.0f)
				{
					NextMessage();
				}
				break;

			default:
				Debug.LogError("Unknown messenger screen state '" + CurrentState.ToString() + "'");
				break;
		}

		return this;
	}
	private void NextMessage()
	{
		CurrentMessage += 1;
		typedLetterIndex = -1;
		currentReplyWait = -1.0f;
		nextTypedLetter = -1.0f;

		if (CurrentMessage >= ScreenDat.Messages.Length - 1)
		{
			CurrentMessage = ScreenDat.Messages.Length - 1;
			CurrentState = ScreenState.Idle;
		}
		else if (ScreenDat.Messages[CurrentMessage + 1].FromPlayer)
		{
			CurrentState = ScreenState.Idle;
		}
		else switch (ScreenDat.Messages[CurrentMessage].MessageType)
		{
			case CellPhone.MessengerScreenData.Message.MessageKind.SendNextImmediately:
				CurrentState = ScreenState.WaitingForReply;
				currentReplyWait = ScreenDat.WaitTimeBeforeReply;
				break;
			case CellPhone.MessengerScreenData.Message.MessageKind.WaitForStory:
				CurrentState = ScreenState.Idle;
				break;
			default:
				Debug.LogError("Unexpected message type '" +
							       ScreenDat.Messages[CurrentMessage].MessageType);
				break;
		}
	}

	public override bool OnInterrupt()
	{
		return CurrentState != ScreenState.Idle;
	}
}