using System;
using UnityEngine;


/// <summary>
/// The sign in the first/demo scene.
/// </summary>
public class PhotographableSign : PhotographableObject
{
	public override void OnPhotographed()
	{
		if (!CellPhone.Instance.TriggerNewMessage)
		{
			CellPhone.Instance.TriggerNewMessage = true;
			Destroy(this);
		}
	}

	public override bool IsPhotographable()
	{
		return CPState_Messenger.CurrentMessage == 5;
	}
}