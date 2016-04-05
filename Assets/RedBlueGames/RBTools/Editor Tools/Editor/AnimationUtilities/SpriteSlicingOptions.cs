namespace RedBlueGames.Tools
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Sprite slicing options used with SpriteSlicer utility
    /// </summary>
    [System.Serializable]
    public struct SpriteSlicingOptions
    {
        public GridSlicingMethod GridSlicing;
        public Vector2 CellSize;
        public SpriteAlignment Pivot;
        public bool OverridePivot;
        public Vector2 CustomPivot;
        public int Frames;
        public SpriteImportMode ImportMode;
        private const char DelimeterChar = ',';

        /// <summary>
        /// Method to use when Grid Slicing. This could be extended to allow for alternate methods, like Unity's default style,
        /// which is to skip a slice if there is no RGB.
        /// </summary>
        public enum GridSlicingMethod
        {
            /// <summary>
            /// Slice All cells, regardless of if they are empty or not
            /// </summary>
            SliceAll = 0,
        }

        /// <summary>
        /// Deserializes the options from a serialized string
        /// </summary>
        /// <returns>The serializable string.</returns>
        /// <param name="serializedOptions">String that represents serialized SlicingOptions.</param>
        public static SpriteSlicingOptions FromSerializableString(string serializedOptions)
        {
            var options = new SpriteSlicingOptions();
            string[] entries = serializedOptions.Split(DelimeterChar);

            // Default ImportMode to Multiple for versioned options
            options.ImportMode = UnityEditor.SpriteImportMode.Multiple;

            options.GridSlicing = GridSlicingMethod.SliceAll;

            options.CellSize = new Vector2(int.Parse(entries[0]), int.Parse(entries[1]));
            if (entries.Length >= 3)
            {
                int.TryParse(entries[2], out options.Frames);
                if (entries.Length >= 9)
                {
                    options.ImportMode = (SpriteImportMode)int.Parse(entries[3]);
                    options.GridSlicing = (GridSlicingMethod)int.Parse(entries[4]);
                    options.OverridePivot = bool.Parse(entries[5]);
                    options.Pivot = (SpriteAlignment)int.Parse(entries[6]);
                    options.CustomPivot = new Vector2(float.Parse(entries[7]), float.Parse(entries[8]));
                }
            }

            return options;
        }

        /// <summary>
        /// Converts the object to a displayable string
        /// </summary>
        /// <returns>The display string.</returns>
        public override string ToString()
        {
            var displayString = string.Empty;
            if (this.ImportMode == SpriteImportMode.None)
            {
                displayString = "Default Settings";
            }
            else if (this.ImportMode == SpriteImportMode.Multiple)
            {
                displayString = string.Concat("Cell Size: ", this.CellSize.x, ",", this.CellSize.y, " Frames: ", this.Frames);
            }
            else if (this.ImportMode == SpriteImportMode.Single)
            {
                displayString = "Single Sprite";
            }

            return displayString;
        }

        /// <summary>
        /// Converts the Options to a string that serializes all data
        /// </summary>
        /// <returns>The serializable string.</returns>
        public string ToSerializableString()
        {
            string delimeterSpace = DelimeterChar + " ";
            string serialized = string.Concat(
                                    this.CellSize.x,
                                    delimeterSpace,
                                    this.CellSize.y,
                                    delimeterSpace,
                                    this.Frames,
                                    delimeterSpace, 
                                    (int)this.ImportMode, 
                                    delimeterSpace, 
                                    (int)this.GridSlicing, 
                                    delimeterSpace,
                                    this.OverridePivot,
                                    delimeterSpace,
                                    (int)this.Pivot,
                                    delimeterSpace,
                                    this.CustomPivot.x,
                                    delimeterSpace,
                                    this.CustomPivot.y);
            return serialized;
        }

        /// <summary>
        /// Determines whether the options have been set up correctly.
        /// </summary>
        /// <returns><c>true</c> if the options would create valid slices; otherwise, <c>false</c>.</returns>
        public bool IsValid()
        {
            if (this.ImportMode == SpriteImportMode.Multiple)
            {
                if (this.CellSize.x == 0 || this.CellSize.y == 0)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}