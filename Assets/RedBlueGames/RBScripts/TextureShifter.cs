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

    /// <summary>
    /// Component that can be added to GameObjects with Renderers to let them offset the texture at runtime.
    /// </summary>
    public class TextureShifter : MonoBehaviour
    {
        private Material materialToShift;
        private Vector2 aggregatedOffset;

        /// <summary>
        /// Shift the texture by the supplied offset
        /// </summary>
        /// <param name="offsetToApply">Offset to apply.</param>
        public void ShiftTexture(Vector2 offsetToApply)
        {
            this.aggregatedOffset += offsetToApply;
        }

        private void Awake()
        {
            this.aggregatedOffset = Vector2.zero;
            this.materialToShift = GetComponent<Renderer>().material;
        }

        /// <summary>
        /// Ensure this is called last after any other scripts calling ShiftTexture
        /// </summary>
        private void LateUpdate()
        {
            this.materialToShift.mainTextureOffset = this.aggregatedOffset;
            this.aggregatedOffset = Vector2.zero;
        }
    }
}