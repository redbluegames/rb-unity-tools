using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace RedBlueGames.Tools
{
	public class ReplaceWithPrefab : EditorWindow
	{
		GameObject prefabObject;

		[MenuItem ("Tools/Replace Selection with Prefab")]
		public static void ReplaceSelectionWithPrefab ()
		{
			EditorWindow.GetWindow (typeof(ReplaceWithPrefab));
		}

		void OnGUI ()
		{
			EditorGUILayout.LabelField ("This tool replaces the currently selected GameObjects with instances of the specified Prefab.", EditorStyles.centeredGreyMiniLabel);
			EditorGUILayout.Space ();

			GUILayout.Label ("Prefab Object", EditorStyles.boldLabel);
			prefabObject = EditorGUILayout.ObjectField (prefabObject, typeof(GameObject), true) as GameObject;

			if (!IsGameObjectPrefab (prefabObject)) {
				EditorGUILayout.HelpBox ("GameObject is not a Prefab or Prefab Instance", MessageType.Error);
				return;
			}

			if (Selection.activeTransform == null) {
				EditorGUILayout.HelpBox ("No GameObjects selected", MessageType.Warning);
				return;
			}

			if (GUILayout.Button ("Replace Selected")) {
				if (prefabObject != null) {
					Transform transformToSelect = null;
					foreach (Transform transformToCopy in Selection.transforms) {
						GameObject instanceObj = null;
						GameObject prefab = PrefabUtility.GetPrefabParent (prefabObject) as GameObject;
						var prefabType = PrefabUtility.GetPrefabType (prefabObject);

						if (prefabType == PrefabType.PrefabInstance) {
							// Copy instance settings into selected object if the selection is a prefab instance
							instanceObj = (GameObject)PrefabUtility.InstantiatePrefab (prefab);
							PrefabUtility.SetPropertyModifications (instanceObj, PrefabUtility.GetPropertyModifications (prefabObject));
						} else if (prefabType == PrefabType.Prefab) {
							// Instantiate a new prefab
							instanceObj = (GameObject)PrefabUtility.InstantiatePrefab (prefabObject);
						} else {
							Debug.LogError ("Supplied GameObject is of an unrecognized type.");
							return;
						}
					
						Undo.RegisterCreatedObjectUndo (instanceObj, "created prefab");

						// Apply previous transform settings to the new one
						Transform createdTransform = instanceObj.transform;
						createdTransform.position = transformToCopy.position;
						createdTransform.rotation = transformToCopy.rotation;
						createdTransform.localScale = transformToCopy.localScale;
						createdTransform.parent = transformToCopy.parent;

						// Keep the old name
						createdTransform.name = transformToCopy.name;

						// Restore sibling index
						createdTransform.SetSiblingIndex (transformToCopy.GetSiblingIndex ());

						// Flag transform to reselect
						if (transformToCopy == Selection.activeTransform) {
							transformToSelect = createdTransform;
						}
					}

					// Destroy old game objects
					foreach (GameObject go in Selection.gameObjects) {
						Undo.DestroyObjectImmediate (go);
					}
				
					if (transformToSelect != null) {
						Selection.activeTransform = transformToSelect;
					}
				}
			}
		}

		bool IsGameObjectPrefab (GameObject obj)
		{
			if (obj == null) {
				return false;
			}

			var prefabType = PrefabUtility.GetPrefabType (prefabObject);
			if (prefabType == PrefabType.None) {
				return false;
			}

			return prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.Prefab;
		}
	}
}