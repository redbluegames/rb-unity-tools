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

namespace RedBlueTools
{
/// <summary>
/// Extends dfButton to add default Down and Up tweens.
/// </summary>
	public class RBdfButton : dfButton
	{
		dfTweenVector2 onDownTween;
		dfTweenVector2 onUpTween;
		AudioSource clickSoundSource;
	
		/// <summary>
		/// Awake is called by the Unity engine when the script instance is being loaded.
		/// </summary>
		public override void Awake ()
		{
			base.Awake ();
		
			if (Application.isPlaying) {
				AddAndInitializeTweens ();
				this.Pivot = dfPivotPoint.MiddleCenter;
				AddAudioSource ();
			}
		}
	
		/// <summary>
		/// This function is called by the Unity engine when the control will be destroyed.
		/// </summary>
		public override void OnDestroy ()
		{
			base.OnDestroy ();
		
			if (Application.isPlaying) {
				DestroyTweens ();
				DestroyClickSoundAudioSource ();
			}
		}

		/// <summary>
		/// Raises the mouse down event. Plays the down tween and resets the up tween.
		/// </summary>
		/// <param name="args">Arguments.</param>
		internal protected override void OnMouseDown (dfMouseEventArgs args)
		{
			base.OnMouseDown (args);
			onDownTween.Play ();
			onUpTween.Reset ();
		}

		/// <summary>
		/// Raises the mouse up event. Plays the up tween.
		/// </summary>
		/// <param name="args">Arguments.</param>
		internal protected override void OnMouseUp (dfMouseEventArgs args)
		{
			base.OnMouseUp (args);
			onUpTween.Play ();
			onDownTween.Reset ();
		}

		/// <summary>
		/// Play audio feedback only on successful click.
		/// </summary>
		/// <param name="args">Arguments.</param>
		internal protected override void OnClick (dfMouseEventArgs args)
		{
			base.OnClick (args);
			//AudioManager.Instance.PlayUIClick (clickSoundSource);
		}

		/// <summary>
		/// Add and initialize the up and down tweens to the button.
		/// </summary>
		void AddAndInitializeTweens ()
		{
			AddTweens ();
			InitiatilizeTweens ();
		}

		/// <summary>
		/// Adds the up and down tweens to the game object.
		/// </summary>
		void AddTweens ()
		{
			// These must be added as components so that the tween base script behaves properly.
			onDownTween = (dfTweenVector2)gameObject.AddComponent<dfTweenVector2> ();
			onUpTween = (dfTweenVector2)gameObject.AddComponent<dfTweenVector2> ();
		}

		/// <summary>
		/// Sets all the initial values for the up and down tweens
		/// </summary>
		void InitiatilizeTweens ()
		{
			Vector2 startingSize = size;
			onDownTween.TweenName = "ScaleDown";
			onDownTween.Target = new dfComponentMemberInfo ();
			onDownTween.Target.Component = this;
			onDownTween.Target.MemberName = "Size";
			onDownTween.Function = dfEasingType.SineEaseOut;
			onDownTween.Length = 0.1f;
			onDownTween.StartValue = size;
			float scaleDownPercent = 0.9f;
			Vector2 endSize = size * scaleDownPercent;
			onDownTween.EndValue = endSize;

			onUpTween.TweenName = "ScaleUp";
			onUpTween.Target = new dfComponentMemberInfo ();
			onUpTween.Target.Component = this;
			onUpTween.Target.MemberName = "Size";
			onUpTween.Function = dfEasingType.SineEaseOut;
			onUpTween.Length = 0.1f;
			onUpTween.StartValue = endSize;
			onUpTween.EndValue = startingSize;
		}

		/// <summary>
		/// Destroy both the up and down tweens so as not to leak them.
		/// </summary>
		void DestroyTweens ()
		{
			if (onDownTween != null) {
				Destroy (onDownTween);
			}
			if (onUpTween != null) {
				Destroy (onUpTween);
			}
		}
	
		/// <summary>
		/// Adds the audio source.
		/// </summary>
		void AddAudioSource ()
		{
			if (clickSoundSource == null) {
				clickSoundSource = gameObject.AddComponent <AudioSource> ();
				clickSoundSource.playOnAwake = false;
			}
		}
	
		/// <summary>
		/// Destroies the audio source.
		/// </summary>
		void DestroyClickSoundAudioSource ()
		{
			Destroy (clickSoundSource);
		}
	}
}