using UnityEditor;
using RedBlueTools;

[CustomEditor (typeof(dfTweenCountToNumber))]
public class dfTweenCountToNumberEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		dfTweenCountToNumber tweenCount = (dfTweenCountToNumber) target;
		DrawDefaultInspector ();

		// Show and Hide two different fields depending on the Duration Type
		if (tweenCount.tweenDurationType == dfTweenCountToNumber.DurationType.fixedDuration) {
			tweenCount.fixedDurationSecs = EditorGUILayout.FloatField ("Seconds", tweenCount.fixedDurationSecs);
		} else if (tweenCount.tweenDurationType == dfTweenCountToNumber.DurationType.pointsPerSecond) {
			tweenCount.pointsPerSec = EditorGUILayout.FloatField ("Points Per Second", tweenCount.pointsPerSec);
		}

	}
}