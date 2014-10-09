using System;
using UnityEngine;


/// <summary>
/// Defines the ground height for this map.
/// There should only be one of these in each scene.
/// </summary>
public class GroundHeight : MonoBehaviour
{
	public static GroundHeight Instance { get; private set; }


	/// <summary>
	/// A single point on the ground heightmap.
	/// </summary>
	[Serializable]
	public class GroundVertex
	{
		public enum InterpolationToNext
		{
			Linear,
			Power,
			Smoothstep,
			SuperSmoothstep,
			BLOCKER,
		}
		public InterpolationToNext SlopeSmoothness;

		public float LerpX = 0.5f;
		public float HeightValue = 0.0f;

		/// <summary>
		/// Only applicable if using "Power" interpolation method.
		/// </summary>
		public float LerpPower = 1.0f;
	}
	public GroundVertex[] Vertices = new GroundVertex[0];

	public Transform MainBackground = null;


	/// <summary>
	/// Gets the height at the given world-space X position.
	/// Returns NaN is the given postion is in a blocker.
	/// </summary>
	public float GetHeightAt(float worldX)
	{
		float xSize = MainBackground.lossyScale.x,
			  xCenter = MainBackground.position.x;
		float xMin = xCenter - (xSize * 0.5f),
			  xMax = xCenter + (xSize * 0.5f);

		return GetHeightAtLerp(Mathf.InverseLerp(xMin, xMax, worldX));
	}
	private float GetHeightAtLerp(float lerpX)
	{
		for (int i = 0; i < Vertices.Length; ++i)
		{
			if (Vertices[i].LerpX > lerpX)
			{
				if (i == 0) return Vertices[i].HeightValue;
				else
				{
					lerpX = Mathf.InverseLerp(Vertices[i - 1].LerpX, Vertices[i].LerpX, lerpX);

					switch (Vertices[i - 1].SlopeSmoothness)
					{
						case GroundVertex.InterpolationToNext.BLOCKER:
							return Single.NaN;

						case GroundVertex.InterpolationToNext.Linear:
							//Don't do anything to lerpX.
							break;
						case GroundVertex.InterpolationToNext.Power:
							//Raise lerpX to a power.
							lerpX = Mathf.Pow(lerpX, Vertices[i - 1].LerpPower);
							break;
						case GroundVertex.InterpolationToNext.Smoothstep:
							//Smoothstep the lerpX value.
							lerpX = Mathf.SmoothStep(0.0f, 1.0f, lerpX);
							break;
						case GroundVertex.InterpolationToNext.SuperSmoothstep:
							//Supersmoothstep the lerpX value.
							lerpX = lerpX * lerpX * lerpX * (10.0f + (lerpX * (-15.0f + (lerpX * 6.0f))));
							break;

						default:
							Debug.LogError("Unknown interpolation type '" + Vertices[i].SlopeSmoothness);
							return 0.0f;
					}

					return Mathf.Lerp(Vertices[i - 1].HeightValue, Vertices[i].HeightValue, lerpX);
				}
			}
		}

		return Vertices[Vertices.Length - 1].HeightValue;
	}

	void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There are two instances of the 'GroundHeight' component! One in the '" +
							 Instance.gameObject.name + "' object and another in the '" +
							 gameObject.name + "' object");
			return;
		}
		Instance = this;

		if (MainBackground == null)
		{
			Debug.LogError("'MainBackground' field of 'GroundHeight' component in '" +
						       gameObject.name + "' object isn't set!");
			return;
		}
	}
	void OnDrawGizmos()
	{
		if (MainBackground == null) return;

		Gizmos.color = Color.white;

		
		float xSize = MainBackground.lossyScale.x,
			  xCenter = MainBackground.position.x;
		float xMin = xCenter - (xSize * 0.5f),
			  xMax = xCenter + (xSize * 0.5f);

		if (Vertices.Length == 1)
		{
			Gizmos.DrawLine(new Vector3(xMin, Vertices[0].HeightValue, 0.0f),
							new Vector3(xMax, Vertices[0].HeightValue, 0.0f));
		}
		else if (Vertices.Length > 1)
		{
			const int nSegments = 200;
			const float increment = 1.0f / (float)nSegments;
			for (int i = 0; i < nSegments; ++i)
			{
				float lerpX = (float)i * increment,
					  nextLerpX = (float)(i + 1) * increment;

				float startY = GetHeightAtLerp(lerpX),
					  endY = GetHeightAtLerp(nextLerpX);

				if (Single.IsNaN(startY) && Single.IsNaN(endY)) continue;

				Gizmos.DrawLine(new Vector3(Mathf.Lerp(xMin, xMax, lerpX), GetHeightAtLerp(lerpX), 0.0f),
								new Vector3(Mathf.Lerp(xMin, xMax, nextLerpX), GetHeightAtLerp(nextLerpX), 0.0f));
			}

			for (int i = 0; i < Vertices.Length; ++i)
			{
				if (Vertices[i].SlopeSmoothness == GroundVertex.InterpolationToNext.BLOCKER)
					Gizmos.color = Color.red;
				else Gizmos.color = Color.white;

				Gizmos.DrawSphere(new Vector3(Mathf.Lerp(xMin, xMax, Vertices[i].LerpX),
											  Vertices[i].HeightValue, 0.0f),
								  20.0f);
			}
		}
	}
}