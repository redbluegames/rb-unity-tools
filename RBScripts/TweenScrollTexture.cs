/*****************************************************************************
 *  Red Blue Tools are Unity Editor utilities. Some utilities require 3rd party tools.
 *  Copyright (C) 2014 Red Blue Games, LLC
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

namespace RedBlueTools
{
/// <summary>
/// Move a texture across a plane at a given linear speed.
/// </summary>
	public class TweenScrollTexture : MonoBehaviour
	{
		public Vector2 speed;
		public bool IsPaused;
		Material textureToScroll;
	
		void Awake ()
		{
			textureToScroll = renderer.material;
		}
	
		void Update ()
		{
			if (!IsPaused) {
				float xOffset = (Time.time * speed.x) % 1;
				float yOffset = (Time.time * speed.y) % 1;
				textureToScroll.mainTextureOffset = new Vector2 (xOffset, yOffset);
			}
		}
	
	}
}