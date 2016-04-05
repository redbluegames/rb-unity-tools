namespace RedBlueGames.Tools
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// This tool replaces the currently selected GameObjects with instances of the specified Prefab.
    /// </summary>
    public class ReplaceWithPrefab : EditorWindow
    {
        private GameObject prefabObject;

        [MenuItem(RBToolsMenuPaths.ReplaceWithPrefab, false, 101)]
        private static void ReplaceSelectionWithPrefab()
        {
            EditorWindow.GetWindow<ReplaceWithPrefab>(true, "Replace with Prefab", true);
        }

        private static bool IsGameObjectPrefab(GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }

            var prefabType = PrefabUtility.GetPrefabType(obj);
            if (prefabType == PrefabType.None)
            {
                return false;
            }

            return prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.Prefab;
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "This tool replaces the currently selected GameObjects with instances of the specified Prefab.",
                MessageType.None);
            EditorGUILayout.Space();

            GUILayout.Label("Selections:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();
            foreach (var selectedObject in Selection.gameObjects)
            {
                GUILayout.Label(selectedObject.name);
            }

            EditorGUILayout.EndVertical();

            GUILayout.Label("Prefab Object", EditorStyles.boldLabel);
            this.prefabObject = EditorGUILayout.ObjectField(this.prefabObject, typeof(GameObject), true) as GameObject;

            if (!IsGameObjectPrefab(this.prefabObject))
            {
                EditorGUILayout.HelpBox("GameObject is not a Prefab or Prefab Instance", MessageType.Error);
                return;
            }

            if (Selection.activeTransform == null)
            {
                EditorGUILayout.HelpBox("No GameObjects selected", MessageType.Warning);
                return;
            }

            if (GUILayout.Button("Replace Selected"))
            {
                if (this.prefabObject != null)
                {
                    Transform transformToSelect = null;
                    foreach (Transform transformToCopy in Selection.transforms)
                    {
                        GameObject instanceObj = null;
                        GameObject prefab = PrefabUtility.GetPrefabParent(this.prefabObject) as GameObject;
                        var prefabType = PrefabUtility.GetPrefabType(this.prefabObject);

                        if (prefabType == PrefabType.PrefabInstance)
                        {
                            // Copy instance settings into selected object if the selection is a prefab instance
                            instanceObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                            PrefabUtility.SetPropertyModifications(
                                instanceObj, 
                                PrefabUtility.GetPropertyModifications(this.prefabObject));
                        }
                        else if (prefabType == PrefabType.Prefab)
                        {
                            // Instantiate a new prefab
                            instanceObj = (GameObject)PrefabUtility.InstantiatePrefab(this.prefabObject);
                        }
                        else
                        {
                            Debug.LogError("Supplied GameObject is of an unrecognized type.");
                            return;
                        }

                        Undo.RegisterCreatedObjectUndo(instanceObj, "created prefab");

                        // Apply previous transform settings to the new one
                        Transform createdTransform = instanceObj.transform;
                        createdTransform.position = transformToCopy.position;
                        createdTransform.rotation = transformToCopy.rotation;
                        createdTransform.localScale = transformToCopy.localScale;
                        createdTransform.parent = transformToCopy.parent;

                        // Keep the old name
                        createdTransform.name = transformToCopy.name;

                        // Restore sibling index
                        createdTransform.SetSiblingIndex(transformToCopy.GetSiblingIndex());

                        // Flag transform to reselect
                        if (transformToCopy == Selection.activeTransform)
                        {
                            transformToSelect = createdTransform;
                        }
                    }

                    // Destroy old game objects
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        Undo.DestroyObjectImmediate(go);
                    }

                    if (transformToSelect != null)
                    {
                        Selection.activeTransform = transformToSelect;
                    }
                }
            }
        }
    }
}