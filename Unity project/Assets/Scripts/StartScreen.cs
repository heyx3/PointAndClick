using System;
using UnityEngine;


public class StartScreen : MonoBehaviour
{
	public GUIStyle ButtonStyle;
	public string StartText = "Start",
				  QuitText = "Quit";
	public Vector2 ButtonDims = new Vector2(75.0f, 25.0f);
	public float ButtonY = 180.0f,
				 ButtonXOffset = 50.0f;

	public string NextScene = "TestPointClickScene";


	void Start()
	{
		Screen.SetResolution(1600, 1000, true);
	}
	void OnGUI()
	{
		if (GUI.Button(new Rect(ButtonXOffset - (ButtonDims.x * 0.5f),
								ButtonY - (ButtonDims.y * 0.5f),
								ButtonDims.x, ButtonDims.y),
					   StartText, ButtonStyle))
		{
			Application.LoadLevel(NextScene);
		}
		else if (GUI.Button(new Rect(Screen.width - ButtonXOffset - (ButtonDims.x * 0.5f),
									 ButtonY - (ButtonDims.y * 0.5f),
									 ButtonDims.x, ButtonDims.y),
							QuitText, ButtonStyle))
		{
			Application.Quit();
		}
	}
}