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
	[RequireComponent(typeof(dfTextureSprite))]
	public class ScreenFade : MonoBehaviour
	{

		dfTweenFloat fadeInTween;
		dfTweenFloat fadeOutTween;
		dfTextureSprite texture;
		
		// Events to send to any subscribers
		public event FadeCompleteHandler FadeComplete;
		public delegate void FadeCompleteHandler ();

		bool isFading;
		
		/// <summary>
		/// Awake is called by the Unity engine when the script instance is being loaded.
		/// </summary>
		void Awake ()
		{
			texture = GetComponent<dfTextureSprite> ();

			if (Application.isPlaying) {
				AddAndInitializeTweens ();
			}
		}
		
		/// <summary>
		/// This function is called by the Unity engine when the control will be destroyed.
		/// </summary>
		void OnDestroy ()
		{
			if (Application.isPlaying) {
				DestroyTweens ();
			}
		}

		void Start ()
		{
			texture.Opacity = 0.0f;

			dfGUIManager manager = transform.parent.gameObject.GetComponent<dfGUIManager> ();
			texture.Size = manager.GetScreenSize ();
		}

		public void FadeToWhite (float fadeTime)
		{
			FadeIntoColor (Color.white, fadeTime);
		}
	
		public void FadeToBlack (float fadeTime)
		{
			FadeIntoColor (Color.black, fadeTime);
		}

		void FadeIntoColor (Color fadeColor, float duration)
		{
			texture.Color = fadeColor;

			if(isFading) {
				StopCurrentFade ();
			}

			isFading = true;
			fadeInTween.Length = duration;
			fadeInTween.Play ();
		}

		public void FadeOut (float duration)
		{	
			if(isFading) {
				StopCurrentFade ();
			}

			isFading = true;
			fadeOutTween.Length = duration;
			fadeOutTween.Play ();
		}
	
		void StopCurrentFade ()
		{
			fadeInTween.Stop ();
			fadeOutTween.Stop ();

			CompleteFade ();
		}
		
		void CompleteFade ()
		{
			isFading = false;
			
			if (FadeComplete != null) {
				FadeComplete ();
			}
		}

		void AddAndInitializeTweens ()
		{
			AddTweens ();
			InitiatilizeTweens ();
		}

		void AddTweens ()
		{
			// These must be added as components so that the tween base script behaves properly.
			fadeInTween = (dfTweenFloat)gameObject.AddComponent<dfTweenFloat> ();
			fadeOutTween = (dfTweenFloat)gameObject.AddComponent<dfTweenFloat> ();
		}

		void InitiatilizeTweens ()
		{
			fadeInTween.TweenName = "FadeIn";
			fadeInTween.Target = new dfComponentMemberInfo ();
			fadeInTween.Target.Component = texture;
			fadeInTween.Target.MemberName = "Opacity";
			fadeInTween.Function = dfEasingType.SineEaseOut;
			fadeInTween.Length = 2.0f;
			float startOpacity = 0.0f;
			fadeInTween.StartValue = startOpacity;
			float endOpacity = 1.0f;
			fadeInTween.EndValue = endOpacity;
			
			fadeInTween.TweenCompleted += HandleTweenCompleted;

			fadeOutTween.TweenName = "FadeOut";
			fadeOutTween.Target = new dfComponentMemberInfo ();
			fadeOutTween.Target.Component = texture;
			fadeOutTween.Target.MemberName = "Opacity";
			fadeOutTween.Function = dfEasingType.SineEaseOut;
			fadeOutTween.Length = 2.0f;
			fadeOutTween.StartDelay = 0.2f;
			startOpacity = 1.0f;
			fadeOutTween.StartValue = startOpacity;
			endOpacity = 0.0f;
			fadeOutTween.EndValue = endOpacity;
			fadeOutTween.TweenCompleted += HandleTweenCompleted;
		}
		
		void HandleTweenCompleted (dfTweenPlayableBase sender)
		{
			CompleteFade ();
		}
		
		/// <summary>
		/// Destroy both the up and down tweens so as not to leak them.
		/// </summary>
		void DestroyTweens ()
		{
			if (fadeOutTween != null) {
				Destroy (fadeOutTween);
			}
			if (fadeInTween != null) {
				Destroy (fadeInTween);
			}
		}

		void LateUpdate ()
		{
			// DF shifts z-order all the time. I wish I could just move this screenfader
			// to a high z-order and it work, but it keeps getting pushed back.
			// This is admitedly a workaround.
			if(isFading) {
				SendToFront ();
			}
		}
		
		/// <summary>
		/// Forces the fader to be the frontmost widget.
		/// </summary>
		void SendToFront ()
		{
			texture.ZOrder = 100;
		}

	}
}