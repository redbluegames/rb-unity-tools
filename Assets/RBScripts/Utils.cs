using UnityEngine;
using System.Collections;

namespace RedBlueTools
{
	/// <summary>
	/// Ideal for static methods that could be called anywhere.
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Opens the URL in a new window if webplayer build, otherwise uses the behavior
		/// built into Application.OpenURL which depends on the platform.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="windowTitle">Window title.</param>
		public static void OpenURL(string url)
		{
			if (Application.isWebPlayer) {
				string evalString = string.Format ("window.open('{0}')", url);
				Application.ExternalEval (evalString);
			} else {
				Application.OpenURL (url);
			}
		}
	}
}