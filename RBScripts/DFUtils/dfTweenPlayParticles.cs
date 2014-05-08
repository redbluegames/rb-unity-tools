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
using System.Collections;

namespace RedBlueTools
{
/// <summary>
/// A custom Tween written on top of Diakon Forge GUI that increments a label's
/// numeric text across a provided amount of time. Can be used like other
/// dfGUI tweens.
/// </summary>
	public class dfTweenPlayParticles : dfTweenPlayableBase
	{
		public string tweenName;
		public ParticleSystem particleSystemToTween;
		bool isTweening;

		void PlayParticles ()
		{
			if (particleSystemToTween != null) {
				particleSystemToTween.Play ();
			} else {
				Debug.LogError ("StarburstSystem not wired up in editor. " +
					"Could not show particle effect.");
			}
		}

	#region dfTweenPlayableBase abstract methods

		/// <summary>
		/// Starts the tween animation
		/// </summary>
		public override void Play ()
		{
			PlayParticles ();
		}

		/// <summary>
		/// Stops the tween animation
		/// </summary>
		public override void Stop ()
		{
			particleSystemToTween.Clear ();
			particleSystemToTween.Stop ();
			isTweening = false;
		}

		/// <summary>
		/// Resets the tween animation to the beginning
		/// </summary>
		public override void Reset ()
		{
			particleSystemToTween.Clear ();
			particleSystemToTween.Stop ();
			isTweening = false;
		}

		/// <summary>
		/// Returns TRUE if the tween animation is currently playing
		/// </summary>
		/// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
		public override bool IsPlaying {
			get {
				return isTweening;
			}
		}

		/// <summary>
		/// Gets or sets the user-defined name of the Tween, which is 
		/// useful to the developer at design time when there are 
		/// multiple tweens on a single GameObject
		/// </summary>
		/// <value>The name of the tween.</value>
		public override string TweenName {
			get {
				return tweenName;
			}
			set {
				tweenName = value;
			}
		}

	#endregion

	#region Tweening Code

	#endregion
	}
}