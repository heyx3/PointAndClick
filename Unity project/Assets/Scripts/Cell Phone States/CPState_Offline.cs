using System;
using UnityEngine;


/// <summary>
/// Cellphone state for apps that just display "you are offline" when selected.
/// </summary>
public class CPState_Offline : CPState_Base
{
	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		GUIBackground(data, Cellphone.OfflineBackground);
		return this;
	}
}