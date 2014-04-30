using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

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
