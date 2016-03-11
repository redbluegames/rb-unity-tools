namespace RedBlueGames.Tools.NotNull.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using RedBlueGames;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ReflectionUtilitiesTests : MonoBehaviour
    {
        public MonoBehaviour testBehaviour;

        [ContextMenu("Run Tests")]
        private void RunTests()
        {
            List<FieldInfo> fieldsWithAttribute = ReflectionUtilities.GetFieldsWithAttributeFromType<SerializeField>(this.testBehaviour.GetType());
            this.LogFieldInfoList(fieldsWithAttribute);
        }

        private void LogFieldInfoList(List<FieldInfo> list)
        {
            Debug.Log("Logging List:");
            foreach (FieldInfo field in list)
            {
                Debug.Log("Field: " + field.Name);
            }
        }
    }
}