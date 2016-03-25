namespace RedBlueGames.Tools.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using NUnit.Framework;
    using RedBlueGames.Tools;
    using UnityEngine;

    [TestFixture]
    public class ReflectionUtilityTests
    {
        [Test]
        public void GetFields_NoFieldWithAttribute_ReturnsNone()
        {
            var fieldsWithAttribute = ReflectionUtility.GetFieldsWithAttributeFromType<TooltipAttribute>(typeof(MockMBReflection));

            Assert.AreEqual(
                0,
                fieldsWithAttribute.Count,
                "Expected to find no fields with attribute, but found: " + fieldsWithAttribute.Count
            );
        }

        [Test]
        public void GetFields_OneFieldWithOneAttribute_ReturnsOne()
        {
            // Arrange
            string expectedFieldName = "IntMultipleAttributesPublic";

            // Act
            var fieldsWithAttribute = ReflectionUtility.GetFieldsWithAttributeFromType<HeaderAttribute>(typeof(MockMBReflection));

            // Assert
            Assert.AreEqual(
                1,
                fieldsWithAttribute.Count,
                "Expected to find one fields with attribute, but found: " + fieldsWithAttribute.Count
            );

            Assert.AreEqual(expectedFieldName, fieldsWithAttribute[0].Name);
        }

        [Test]
        public void GetFields_PublicAndPrivateFieldsWithAttribute_ReturnsExpected()
        {
            // Arrange
            var expectedFieldNames = new List<string>()
            {
                "IntMultipleAttributesPublic",
                "intPrivate",
                "IntPublic",
                "customClassPrivate",
                "CustomClassPublic"
            };

            // Act
            var fieldsWithAttribute = ReflectionUtility.GetFieldsWithAttributeFromType<SerializeField>(
                typeof(MockMBReflection),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // Assert
            string foundAttributeNames = string.Empty;
            foreach (var field in fieldsWithAttribute)
            {
                foundAttributeNames = string.Concat(foundAttributeNames, field.Name, ", ");
            }
            Assert.AreEqual(
                expectedFieldNames.Count,
                fieldsWithAttribute.Count,
                string.Format("Expected to find {0} fields with attribute, but found {1}. Fields: {2}",
                    expectedFieldNames.Count,
                    fieldsWithAttribute.Count,
                    foundAttributeNames
                )
            );

            foreach (var field in fieldsWithAttribute)
            {
                string assertWarning = "Could not find field by the name {0} in expected field names.";
                Assert.True(expectedFieldNames.Contains(field.Name), string.Format(assertWarning, field.Name));
            }
        }

        /// <summary>
        /// Test utility to Mock MB reflection.
        /// </summary>
        public class MockMBReflection : MonoBehaviour
        {
            #region int field test

            private int noAttriuteIntPrivate;
            public int noAttributeIntegar;
            [SerializeField]
            private int
                intPrivate;
            [SerializeField]
            public int
                IntPublic;

            [SerializeField]
            [Header("Header Attribute")]
            public int IntMultipleAttributesPublic;

            #endregion

            #region Custom Class field test

            private CustomClass noAttributeCustomClassPrivate;
            public CustomClass noAttributeCustomClassPublic;
            [SerializeField]
            private CustomClass customClassPrivate;
            [SerializeField]
            public CustomClass CustomClassPublic;

            #endregion

            public void PublicMethodNoAttribute()
            {
            }

            public class CustomClass
            {
                private int x;
            }
        }

    }
}