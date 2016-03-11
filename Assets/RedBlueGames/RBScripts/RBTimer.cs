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

    /* Timer adapted from Adam Winkels (Lovers in a Dangerous Spacetime
 * https://gist.githubusercontent.com/winkels/779ef0bdba4f88edeceb/raw/CoroutineTimer.cs
 */
    [System.Serializable]
    public class RBTimer
    {
        ///public fields
        public float Duration;
        public bool Repeats;
        public bool UsesFixedUpdate;

        ///private fields
        private float TimeRemaining;
        private bool _isRunning;

        private IEnumerator timerCoroutine;
        private MonoBehaviour targetMonobehaviour;
        private System.Action timerFinishedAction;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        public RBTimer()
        {
            this.Duration = float.NaN;
            this.Repeats = false;
        }

        public RBTimer(float timerDuration, bool repeats = false)
        {
            this.Duration = timerDuration;
            this.Repeats = repeats;
        }

        public void Start(MonoBehaviour targetMonobehaviour, System.Action timerFinishedAction = null)
        {
            if (IsUnset())
            {
                throw new System.InvalidOperationException("Trying to start a timer without setting its duration.");
            }

            if (timerCoroutine != null)
            {
                throw new System.InvalidOperationException(
                    "This timer has already been started. If you are trying to Start it during timerAction use Repeats flag.");
            }

            this.timerFinishedAction = timerFinishedAction;
            this.targetMonobehaviour = targetMonobehaviour;

            DoStart();
        }

        public bool IsUnset()
        {
            return float.IsNaN(Duration);
        }

        void DoStart()
        {
            if (Duration <= 0.0f)
            {
                throw new System.InvalidOperationException("Timer duration cannot be less than or equal to zero");
            }

            StartTimerCoroutine(Duration);

            _isRunning = true;
        }

        void TimerFinished()
        {
            _isRunning = false;
            // Fire time finished event
            if (timerFinishedAction != null)
            {
                timerFinishedAction();
            }

            //null check to make sure the timer has not been stopped in the timerFinishedDelegate
            if (Repeats && timerCoroutine != null)
            {
                DoStart();
            }
            else
            {
                Stop();
            }
        }

        public float GetTimeRemaining()
        {
            return TimeRemaining;
        }

        public void Stop()
        {
            _isRunning = false;

            if (timerCoroutine != null)
            {
                StopTimerCoroutine();
            }

            // Clean up references
            TimeRemaining = 0.0f;
            targetMonobehaviour = null;
            timerFinishedAction = null;
        }

        #region TimerCoroutine

        private void StartTimerCoroutine(float waitSeconds)
        {
            timerCoroutine = CountdownForDuration(waitSeconds);
            targetMonobehaviour.StartCoroutine(timerCoroutine);
        }

        private IEnumerator CountdownForDuration(float desiredDuration)
        {
            /* Note: If we use a ton of timers, we may need to use this simpler version (shown)
			 * But for now, let's use a version that we can see how much time is remaining.
			 * yield return new WaitForSeconds (waitSeconds); */
            TimeRemaining += desiredDuration;
            while (true)
            {
                // yield at the start of the loop to always wait at least one frame.
                // Only a <= 0 second timer would finish immediately, and that's not allowed.
                if (UsesFixedUpdate)
                {
                    // FixedUpdate timers are used to guarantee precision on physics objects like projectiles.
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    yield return null;
                }

                TimeRemaining -= Time.deltaTime;
                if (TimeRemaining <= 0.0f)
                {
                    break;
                }
            }

            TimerFinished();
        }

        private void StopTimerCoroutine()
        {
            targetMonobehaviour.StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        #endregion
    }
}