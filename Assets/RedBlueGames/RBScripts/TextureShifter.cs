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
namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEngine;

    public class TextureShifter : MonoBehaviour
    {
        private Material textureToShift;
        private Vector2 aggregatedOffset;

        private void Awake()
        {
            aggregatedOffset = Vector2.zero;
            textureToShift = GetComponent<Renderer>().material;
        }

        /// <summary>
        /// Ensure this is called last after any other scripts calling ShiftTexture
        /// </summary>
        private void LateUpdate()
        {
            textureToShift.mainTextureOffset = aggregatedOffset;
            aggregatedOffset = Vector2.zero;
        }

        public void ShiftTexture(Vector2 offsetToApply)
        {
            aggregatedOffset += offsetToApply;
        }
    }
}