using UnityEngine;
using UnityEditor;

namespace RedBlueGames.Tools
{
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
        const char delimeterChar = ',';
    
        public enum GridSlicingMethod
        {
            SliceAll = 0,
            Bogdan = 1,
        };
        
        public string ToDisplayString ()
        {
            
            var displayString = "";
            if (ImportMode == SpriteImportMode.None) 
            {
                displayString = "Default Settings";
            } else if (ImportMode == SpriteImportMode.Multiple)
            {
                displayString = string.Concat ("Cell Size: ", CellSize.x, ",", CellSize.y, " Frames: ", Frames );
            } else if (ImportMode == SpriteImportMode.Single )
            {
                displayString = "Single Sprite";
            }
            return displayString;
        }
        
        public override string ToString ()
        {
            string delimeterSpace = delimeterChar + " ";
            string serialized = string.Concat (CellSize.x, delimeterSpace, CellSize.y, delimeterSpace, Frames,
                delimeterSpace, (int) ImportMode, delimeterSpace, (int)GridSlicing, delimeterSpace,
                OverridePivot, delimeterSpace, (int) Pivot, delimeterSpace, CustomPivot.x,
                delimeterSpace, CustomPivot.y);
            return serialized;
        }
        
        public static SpriteSlicingOptions FromString (string serialized)
        {
            var options = new SpriteSlicingOptions ();
            string[] entries = serialized.Split (delimeterChar);
            
            // Default ImportMode to Multiple for versioned options
            options.ImportMode = UnityEditor.SpriteImportMode.Multiple;
            // Default GridSlicing to Bogdan method
            options.GridSlicing = GridSlicingMethod.Bogdan;
            
            options.CellSize = new Vector2 (int.Parse (entries[0]), int.Parse (entries[1]));
            if (entries.Length >= 3)
            {
                int.TryParse (entries[2], out options.Frames);
                if (entries.Length >= 9)
                {
                    options.ImportMode = (SpriteImportMode) int.Parse (entries[3]);
                    options.GridSlicing = (GridSlicingMethod) int.Parse (entries[4]);
                    options.OverridePivot = bool.Parse (entries[5]);
                    options.Pivot = (SpriteAlignment) int.Parse (entries[6]);
                    options.CustomPivot = new Vector2 (float.Parse (entries[7]), float.Parse (entries[8]));
                }
            }
        
            return options;
        }
        
        public bool IsValid ()
        {
            if (ImportMode == SpriteImportMode.Multiple)
            {
                if (CellSize.x == 0 || CellSize.y == 0) {
                    return false;
                }
            }
            
            return true;
        }
    }
}