using System;
using UnityEngine;


/// <summary>
/// Cellphone state for apps that just display a simple, static texture when selected.
/// </summary>
public class CPState_Static : CPState_Base
{
	public Texture2D Tex;

	public CPState_Static(Texture2D tex) { Tex = tex; }

	public override CPState_Base OnGUI(CellPhone.ButtonPositioningData data)
	{
		GUIBackground(data, Tex);
		return this;
	}
}