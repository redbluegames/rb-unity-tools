/*****************************************************************************
 *  Copyright (C) 2014-2015 Red Blue Games, LLC
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ****************************************************************************/
using UnityEngine;

namespace RedBlueGames.Tools
{
	[RequireComponent (typeof(TextureShifter))]
	/// <summary>
	/// Move a texture across a plane at a given linear speed.
	/// </summary>
	public class TweenScrollTexture : MonoBehaviour
	{
		public Vector2 speed;
		public bool IsPaused;
		TextureShifter textureShifter;
		Vector2 currentOffset;
		public bool MoveAsSinWave;
		public int SinCycleSeconds = 60;

		void Awake ()
		{
			currentOffset = Vector2.zero;
			textureShifter = GetComponent<TextureShifter> ();
		}

		void Update ()
		{
			if (!IsPaused) {
				float xOffset = (Time.deltaTime * speed.x);
				float yOffset = (Time.deltaTime * speed.y);
				if (MoveAsSinWave) {
					float sinBasedOffset = Mathf.Sin ((Time.timeSinceLevelLoad * Mathf.PI * 2) / SinCycleSeconds);
					yOffset = sinBasedOffset * (Time.deltaTime * speed.y);
				}
				currentOffset = new Vector2 ((currentOffset.x + xOffset) % 1, (currentOffset.y + yOffset) % 1);
				textureShifter.ShiftTexture (currentOffset);
			}
		}
	}
}