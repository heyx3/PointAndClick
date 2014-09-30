using System;
using UnityEngine;


/// <summary>
/// Cellphone state for apps that just display "you are offline" when selected.
/// </summary>
public class CPState_Offline : CPState_Base
{
	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		GUI.DrawTexture(new Rect(data.MinPos.x + Cellphone.OfflineSpriteBorderSize.x,
								 data.MaxPos.y - Cellphone.OfflineSpriteBorderSize.y,
								 data.MaxPos.x - data.MinPos.x - (2.0f * Cellphone.OfflineSpriteBorderSize.x),
								 -(data.MaxPos.y - data.MinPos.y - (2.0f * Cellphone.OfflineSpriteBorderSize.y))),
						Cellphone.OfflineTex);

		return this;
	}
}