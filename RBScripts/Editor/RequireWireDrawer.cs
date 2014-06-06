using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RequireWireAttribute))]
public class RequireWireDrawer : PropertyDrawer
{
	bool hasError;

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Draw label
		GUI.Label(position, label.text + " (Required)", EditorStyles.label);
		position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

		int fieldWidth = 250;
		var prefixRect = new Rect (position.x, position.y, fieldWidth, position.height);
		var valueRect = new Rect (position.x, prefixRect.y, fieldWidth, position.height);

		property.objectReferenceValue = EditorGUI.ObjectField(valueRect, property.objectReferenceValue, typeof(GameObject), true);
		prefixRect.y += prefixRect.height + 1;
		valueRect.y += valueRect.height + 1;

		if (property.objectReferenceValue == null) {
			if (!hasError) {
				Debug.LogError (property.name + " has not been wired in the editor.");
			}
			hasError = true;
		} else {
			hasError = false;
		}

		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty ();
	}
}