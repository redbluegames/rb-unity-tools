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

    /*
 * Simple timer that does NOT update itself. The classes using
 * this need to check back periodically to see how much time
 * has gone by.
 */
    [System.Serializable]
    public class RBTimeSince
    {
        private float timeStarted;

        public bool IsRunning { get; private set; }

        private const float UNSET = float.NaN;

        /// <summary>
        /// The amount of time in seconds the timer has been running.
        /// </summary>
        /// <value>The elapsed.</value>
        public float Elapsed
        {
            get
            {
                WarnIfUnSet();
                return Time.time - timeStarted;
            }
        }

        /// <summary>
        /// Null constructor. Set our duration to a known invalid value.
        /// </summary>
        public RBTimeSince()
        {
            Stop();
        }

        /// <summary>
        /// Starts the timesince
        /// </summary>
        public void Start()
        {
            timeStarted = Time.time;
            IsRunning = true;
        }

        /// <summary>
        /// Stop tracking time and clear the timestamps
        /// </summary>
        public void Stop()
        {
            timeStarted = UNSET;
            IsRunning = false;
        }

        /// <summary>
        /// Warns to tell coder they called a method that needs duration set.
        /// </summary>
        void WarnIfUnSet()
        {
            if (!IsRunning || timeStarted == UNSET)
            {
                Debug.LogWarning("Tried to check time left on stopped or unset timesince.");
            }
        }
    }
}