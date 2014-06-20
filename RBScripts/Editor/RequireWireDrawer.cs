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
		var rRect = new Rect (position.x - 8, position.y, position.width, position.height);
		GUI.Label(rRect, "*", EditorStyles.label);

		int fieldWidth = 400;
		var valueRect = new Rect (position.x, position.y, fieldWidth, position.height);

		EditorGUI.PropertyField (valueRect, property);
		valueRect.y += valueRect.height + 1;

		if (property.objectReferenceValue == null) {
			if (!hasError) {
				Debug.LogError (string.Format ("{0} on object {1} has not been wired in the editor.", 
				                               property.name, property.serializedObject.targetObject.name));
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