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
    /// Timer adapted from Adam Winkels (Lovers in a Dangerous Spacetime)
    /// https://gist.githubusercontent.com/winkels/779ef0bdba4f88edeceb/raw/CoroutineTimer.cs
    /// </summary>
    [System.Serializable]
    public class RBTimer
    {
        [SerializeField]
        private float duration;
        [SerializeField]
        private bool repeats;
        [SerializeField]
        private bool usesFixedUpdate;

        private float timeRemaining;
        private bool isRunning;

        private IEnumerator timerCoroutine;
        private MonoBehaviour targetMonobehaviour;
        private System.Action timerFinishedAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="RBTimer"/> class without a Duration
        /// </summary>
        public RBTimer()
        {
            this.duration = float.NaN;
            this.repeats = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RBTimer"/> class with a duration.
        /// </summary>
        /// <param name="timerDuration">Timer duration.</param>
        /// <param name="repeats">If set to <c>true</c>, timer will repeat after it completes.</param>
        public RBTimer(float timerDuration, bool repeats = false)
        {
            this.duration = timerDuration;
            this.repeats = repeats;
        }

        /// <summary>
        /// Gets a value indicating whether this timer is currently counting down.
        /// </summary>
        /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        /// <summary>
        /// Start the timer, attached to the specified targetMonobehaviour. Raises timerFinishedAction when complete.
        /// </summary>
        /// <param name="targetMonobehaviour">Target monobehaviour to attach Coroutine to.</param>
        /// <param name="timerFinishedAction">Action invoked on timer complete.</param>
        public void Start(MonoBehaviour targetMonobehaviour, System.Action timerFinishedAction = null)
        {
            if (this.IsUnset())
            {
                throw new System.InvalidOperationException("Trying to start a timer without setting its duration.");
            }

            if (this.timerCoroutine != null)
            {
                throw new System.InvalidOperationException(
                    "This timer has already been started. If you are trying to Start it during timerAction use Repeats flag.");
            }

            this.timerFinishedAction = timerFinishedAction;
            this.targetMonobehaviour = targetMonobehaviour;

            this.DoStart();
        }

        /// <summary>
        /// Determines whether this instance is unset.
        /// </summary>
        /// <returns><c>true</c> if this instance is unset; otherwise, <c>false</c>.</returns>
        public bool IsUnset()
        {
            return float.IsNaN(this.duration);
        }

        /// <summary>
        /// Gets the time remaining.
        /// </summary>
        /// <returns>The time remaining.</returns>
        public float GetTimeRemaining()
        {
            return this.timeRemaining;
        }

        /// <summary>
        /// Stop this timer, without calling the Finished action.
        /// </summary>
        public void Stop()
        {
            this.isRunning = false;

            if (this.timerCoroutine != null)
            {
                this.StopTimerCoroutine();
            }

            // Clean up references
            this.timeRemaining = 0.0f;
            this.targetMonobehaviour = null;
            this.timerFinishedAction = null;
        }

        private void DoStart()
        {
            if (this.duration <= 0.0f)
            {
                throw new System.InvalidOperationException("Timer duration cannot be less than or equal to zero");
            }

            this.StartTimerCoroutine(this.duration);

            this.isRunning = true;
        }

        private void TimerFinished()
        {
            this.isRunning = false;

            // Fire time finished event
            if (this.timerFinishedAction != null)
            {
                this.timerFinishedAction();
            }

            // Null check to make sure the timer has not been stopped in the timerFinishedDelegate
            if (this.repeats && this.timerCoroutine != null)
            {
                this.DoStart();
            }
            else
            {
                this.Stop();
            }
        }

        #region TimerCoroutine

        private void StartTimerCoroutine(float waitSeconds)
        {
            this.timerCoroutine = this.CountdownForDuration(waitSeconds);
            this.targetMonobehaviour.StartCoroutine(this.timerCoroutine);
        }

        private IEnumerator CountdownForDuration(float desiredDuration)
        {
            /* Note: If we use a ton of timers, we may need to use this simpler version (shown)
             * But for now, let's use a version that we can see how much time is remaining.
             * yield return new WaitForSeconds (waitSeconds);
             */
            this.timeRemaining += desiredDuration;
            while (true)
            {
                // yield at the start of the loop to always wait at least one frame.
                // Only a <= 0 second timer would finish immediately, and that's not allowed.
                if (this.usesFixedUpdate)
                {
                    // FixedUpdate timers are used to guarantee precision on physics objects like projectiles.
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    yield return null;
                }

                this.timeRemaining -= Time.deltaTime;
                if (this.timeRemaining <= 0.0f)
                {
                    break;
                }
            }

            this.TimerFinished();
        }

        private void StopTimerCoroutine()
        {
            this.targetMonobehaviour.StopCoroutine(this.timerCoroutine);
            this.timerCoroutine = null;
        }

        #endregion
    }
}