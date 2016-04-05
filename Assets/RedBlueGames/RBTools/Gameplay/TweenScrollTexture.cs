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
    using UnityEngine;

    /// <summary>
    /// Move a texture across a plane at a given linear speed.
    /// </summary>
    [RequireComponent(typeof(TextureShifter))]
    public class TweenScrollTexture : MonoBehaviour
    {
        private TextureShifter textureShifter;
        private Vector2 currentOffset;

        [Tooltip("The direction for the scroll")]
        [SerializeField]
        private Vector2 direction;

        [Tooltip("Is the scrolling currently paused?")]
        [SerializeField]
        private bool isPaused;

        /// <summary>
        /// Gets or sets the direction (with magnitude) for the scroll
        /// </summary>
        /// <value>The direction.</value>
        public Vector2 Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = value;
            }
        }

        private void Awake()
        {
            this.currentOffset = Vector2.zero;
            this.textureShifter = this.GetComponent<TextureShifter>();
        }

        private void Update()
        {
            if (!this.isPaused)
            {
                float xOffset = Time.deltaTime * this.direction.x;
                float yOffset = Time.deltaTime * this.direction.y;

                this.currentOffset = new Vector2((this.currentOffset.x + xOffset) % 1, (this.currentOffset.y + yOffset) % 1);
                this.textureShifter.ShiftTexture(this.currentOffset);
            }
        }
    }
}