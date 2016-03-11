namespace RedBlueGames.Tools.NotNull.Tests
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Test utitlity to Mock MB reflection.
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
        public int IntMultipleFieldsPublic;

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