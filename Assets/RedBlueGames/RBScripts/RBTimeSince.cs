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
    /// Simple timer that does NOT update itself. The classes using
    /// this need to check back periodically to see how much time
    /// has gone by.
    /// </summary>
    [System.Serializable]
    public class RBTimeSince
    {
        private const float UNSET = float.NaN;
        private float timeStarted;

        /// <summary>
        /// Initializes a new instance of the <see cref="RBTimeSince"/> class with invalid values
        /// </summary>
        public RBTimeSince()
        {
            this.Stop();
        }

        /// <summary>
        /// Gets a value indicating whether this instance has been started.
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the amount of time in seconds the timer has been running.
        /// </summary>
        /// <value>The elapsed.</value>
        public float Elapsed
        {
            get
            {
                this.WarnIfUnSet();
                return Time.time - this.timeStarted;
            }
        }

        /// <summary>
        /// Stop tracking time and clear the timestamps
        /// </summary>
        public void Stop()
        {
            this.timeStarted = UNSET;
            this.IsRunning = false;
        }

        /// <summary>
        /// Starts the timesince
        /// </summary>
        public void Start()
        {
            this.timeStarted = Time.time;
            this.IsRunning = true;
        }

        /// <summary>
        /// Warns to tell coder they called a method that needs duration set.
        /// </summary>
        private void WarnIfUnSet()
        {
            if (!this.IsRunning || this.timeStarted == UNSET)
            {
                Debug.LogWarning("Tried to check time left on stopped or unset timesince.");
            }
        }
    }
}