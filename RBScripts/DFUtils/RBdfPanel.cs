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
using System;

namespace RedBlueTools
{
// RBdfPanel adapted from DemoMenuPanel in Daikon Forge examples
	[Serializable]
	public class RBdfPanel : MonoBehaviour
	{
	#region Public fields

		public dfTweenPlayableBase showTween;
		public dfTweenPlayableBase hideTween;

	#endregion

	#region Private variables

		protected bool isHiding;
		protected dfControl owner;
	
	#endregion
	
	#region Public methods 
	
		public virtual void Show ()
		{
			isHiding = false;
			StopAllTweens ();
		
			owner.Show ();
			owner.BringToFront ();
			owner.IsEnabled = true;

			if (showTween != null) {
				showTween.Play ();
			} else {
				//owner.Opacity = 1.0f;
			}
		}
	
		public virtual void Hide ()
		{
			if (!isHiding) {
				isHiding = true;
				StopAllTweens ();

				owner.Unfocus ();
				owner.IsEnabled = false;

				if (hideTween != null) {
					hideTween.Play ();
				} else {
					//owner.Opacity = 0.0f;
					owner.Hide ();
				}
			}
		}
	
	#endregion
	
	#region Unity events
	
		protected virtual void Awake ()
		{
			Initialize ();
		}

		protected virtual void OnEnabled ()
		{
		}
	
		protected virtual void Start ()
		{
		}
	
		protected virtual void Update ()
		{
		}
	
	#endregion
	
	#region Component events
	
		protected virtual void TweenCompleted (dfTweenPlayableBase tween)
		{
			if (tween.Equals (hideTween)) {
				owner.Hide ();
			}
		}
	
	#endregion
	
	#region Private utility methods 
	
		protected virtual void Initialize ()
		{
			// All menus start out invisible
			owner = GetComponent<dfControl> ();
			owner.Hide ();
			//owner.Opacity = 0.0f;
		}
	
		private void StopAllTweens ()
		{
			var tweenGroups = GetComponents<dfTweenGroup> ();
			for (int i = 0; i < tweenGroups.Length; i++) {
				tweenGroups [i].Stop ();
			}
		}
	
	#endregion
	
	}
}