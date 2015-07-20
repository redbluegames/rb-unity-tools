using UnityEngine;
using System.Collections;

public class TextureShifter : MonoBehaviour
{
	Material textureToShift;
	Vector2 aggregatedOffset;

	void Awake()
	{
		aggregatedOffset = Vector2.zero;
		textureToShift = GetComponent<Renderer>().material;
	}
	
	/// <summary>
	/// Ensure this is called last after any other scripts calling ShiftTexture
	/// </summary>
	void LateUpdate ()
	{
		textureToShift.mainTextureOffset = aggregatedOffset;
		aggregatedOffset = Vector2.zero;
	}

	public void ShiftTexture (Vector2 offsetToApply)
	{
		aggregatedOffset += offsetToApply;
	}
}
