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
    /// Class that handles setting TimeScale to 0 and back, properly handling multiple requests to pause and unpause.
    /// </summary>
    public static class TimeManager
    {
        // Time and Pause handling members
        private static int pauseRequests;
        private static int lowLevelPauseRequests;

        /// <summary>
        /// Gets a value indicating whether the game is paused
        /// </summary>
        /// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
        public static bool IsPaused { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the game is paused with a low level pause.
        /// </summary>
        /// <value><c>true</c> if is low level paused; otherwise, <c>false</c>.</value>
        public static bool IsLowLevelPaused { get; private set; }

        /// <summary>
        /// Pauses the game, or increments the pause counter if it's already paused.
        /// </summary>
        public static void RequestPause()
        {
            pauseRequests++;
            IsPaused = true;
            ResolveTimeScale();
        }

        /// <summary>
        /// Attempts to unpause the game. Once all requests to pause have been unwound, the game
        /// unpauses.
        /// </summary>
        public static void RequestUnpause()
        {
            pauseRequests--;
            if (pauseRequests == 0)
            {
                IsPaused = false;
            }

            ResolveTimeScale();
        }

        /// <summary>
        /// Requests a low level pause, used for debugging
        /// </summary>
        public static void RequestLowLevelPause()
        {
            lowLevelPauseRequests++;
            IsLowLevelPaused = true;
            ResolveTimeScale();
        }

        /// <summary>
        /// Requests a low level unpause
        /// </summary>
        public static void RequestLowLevelUnpause()
        {
            lowLevelPauseRequests--;
            if (lowLevelPauseRequests == 0)
            {
                IsLowLevelPaused = false;
            }

            ResolveTimeScale();
        }

        private static void ResolveTimeScale()
        {
            if (IsPaused || IsLowLevelPaused)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}