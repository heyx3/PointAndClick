using System;
using UnityEngine;


/// <summary>
/// Cellphone state for the messenger app.
/// </summary>
public class CPState_Messenger : CPState_Base
{
	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		GUIBackground(data, Cellphone.MessengerBackground);
		return this;
	}
}