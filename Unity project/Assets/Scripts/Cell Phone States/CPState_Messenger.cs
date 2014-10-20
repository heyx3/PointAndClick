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
		/// Nothing's going on (no other messages will be sent until something happens in the game).
		/// </summary>
		Idle,
		/// <summary>
		/// Waiting for the player to hit "send".
		/// </summary>
		WaitingForSend,
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

	private Vector2 scrollViewPos = new Vector2(0.0f, 999999.0f);
	


	public CPState_Messenger()
	{
		CurrentMessage -= 1;
		NextMessage();
	}

	public override CPState_Base OnGUI(ScreenPositioningData data)
	{
		if (Cellphone.TriggerNewMessage)
		{
			Cellphone.TriggerNewMessage = false;
			NextMessage();
		}

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
			(CurrentState == ScreenState.TypingReply || CurrentState == ScreenState.WaitingForSend))
		{
			Vector2 dims = ScreenDat.MessageBoxBottomRightLerp - ScreenDat.MessageBoxTopLeftLerp;
			dims.y = -dims.y;

			dims.x *= 320.0f;
			dims.y *= 200.0f;

			GUILabel(ScreenDat.MessageBoxTopLeftLerp, dims, data, Cellphone.SmallTextStyle,
					 ScreenDat.Messages[CurrentMessage + 1].MessageText.Substring(0, typedLetterIndex));
		}


		//Render message button if player needs to type text.
		if (CurrentState == ScreenState.WaitingForSend)
		{
			if (GUIButton(ScreenDat.MessageButtonCenterLerp,
						  new Vector2(ScreenDat.NewSendMessage.width, ScreenDat.NewSendMessage.height),
						  data, new Vector2(), Cellphone.ButtonStyle, ScreenDat.NewSendMessage))
			{
				NextMessage();
			}
		}
		else GUITexture(ScreenDat.MessageButtonCenterLerp, data, ScreenDat.NoSendMessage);


		//Update screen state.
		switch (CurrentState)
		{
			case ScreenState.Idle:
			case ScreenState.WaitingForSend:
				break;

			case ScreenState.TypingReply:
				nextTypedLetter -= Time.deltaTime;
				if (nextTypedLetter <= 0.0f)
				{
					typedLetterIndex += 1;
					nextTypedLetter += Cellphone.MessengerScreen.PlayerTypeInterval;
					if (typedLetterIndex >= ScreenDat.Messages[CurrentMessage + 1].MessageText.Length)
					{
						CurrentState = ScreenState.WaitingForSend;
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
	public void NextMessage()
	{
		CurrentMessage += 1;
		typedLetterIndex = -1;
		currentReplyWait = -1.0f;
		nextTypedLetter = -1.0f;
		
		//If no messages are visible yet, or the last message is already visible, don't do anything.
		if (CurrentMessage >= ScreenDat.Messages.Length - 1 || CurrentMessage < 0)
		{
			CurrentState = ScreenState.Idle;
		}
		//Otherwise, either:
		//   1) Wait for the next message to be triggered,
		//   2) Type out the player's next message, or
		//   3) Wait a second or two and then show her next message.
		else
		{
			CellPhone.MessengerScreenData.Message thisMsg = ScreenDat.Messages[CurrentMessage],
												  nextMsg = ScreenDat.Messages[CurrentMessage + 1];
			switch (thisMsg.NextMessageType)
			{
				case CellPhone.MessengerScreenData.Message.NextMessageKinds.Continue:

					//If the next message is the player's, show him typing out the message on the screen.
					if (nextMsg.FromPlayer)
					{
						CurrentState = ScreenState.TypingReply;
						typedLetterIndex = 0;
						nextTypedLetter = ScreenDat.PlayerTypeInterval;
					}
					//Otherwise, wait a second or two and then send her next message.
					else
					{
						CurrentState = ScreenState.WaitingForReply;
						currentReplyWait = ScreenDat.WaitTimeBeforeReply;
					}

					break;

				case CellPhone.MessengerScreenData.Message.NextMessageKinds.WaitForTrigger:
					CurrentState = ScreenState.Idle;
					break;

				default:
					Debug.LogError("Unknown message transition '" + thisMsg.NextMessageType.ToString());
					break;
			}
		}
	}

	public override bool OnInterrupt()
	{
		return CurrentState != ScreenState.Idle;
	}
}