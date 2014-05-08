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
	public class TweenCountToNumberDFLabel : dfTweenPlayableBase
	{
		public string tweenName;
		public dfLabel label;
		public int targetNumber;
		public float secondsToTarget;
		int currentNumber;
		int originalNumber;
		bool isTweening;

	#region dfTweenPlayableBase abstract methods

		/// <summary>
		/// Starts the tween animation
		/// </summary>
		public override void Play ()
		{
			string currentText = GetTextFromObject ();
			if (!int.TryParse (currentText, out currentNumber)) {
				Debug.LogError (string.Format ("Cannot Play CountToNumberTween because label " +
					"text ('{0}') could not be parsed", currentText));
				return;
			}
			if (currentNumber != targetNumber && !isTweening) {
				originalNumber = currentNumber;
				StartCoroutine (CountUpOrDown ());
			}
		}

		/// <summary>
		/// Stops the tween animation
		/// </summary>
		public override void Stop ()
		{
			isTweening = false;
		}

		/// <summary>
		/// Resets the tween animation to the beginning
		/// </summary>
		public override void Reset ()
		{
			SetText (originalNumber.ToString ());
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

		/// <summary>
		/// Iterate through every number up to a target, displaying each. This
		/// takes a provided amount of time to complete.
		/// </summary>
		IEnumerator CountUpOrDown ()
		{
			isTweening = true;
			int originalDifference = Mathf.Abs (currentNumber - targetNumber);
			while (currentNumber != targetNumber) {
				SetText (currentNumber.ToString ());
				if (currentNumber < targetNumber) {
					currentNumber++;
				} else {
					currentNumber--;
				}
				float waitTime = secondsToTarget / originalDifference;
				yield return new WaitForSeconds (waitTime);
			}
			SetText (currentNumber.ToString ());
			isTweening = false;
		}

		/// <summary>
		/// Gets the text from a dfLabel.
		/// </summary>
		/// <returns>The text from object.</returns>
		public string GetTextFromObject ()
		{
			return label.Text;
		}

		/// <summary>
		/// Sets the text on a dfLabel.
		/// </summary>
		/// <param name="textToSet">Text to set.</param>
		public void SetText (string textToSet)
		{
			label.Text = textToSet;
		}
	#endregion
	}
}