using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RedBlueGames.Tools
{
    public static class SpriteSlicer
    {
        public static SpriteMetaData[] CreateSpriteSheetForTexture (Texture2D texture, SpriteSlicingOptions slicingOptions)
        {
            List<SpriteMetaData> sprites = new List<SpriteMetaData> ();
            Rect[] gridRects = GetAllSliceRectsForTexture (texture, slicingOptions.CellSize);
            for (int i = 0; i < gridRects.Length; i++) {
                SpriteMetaData spriteMetaData = new SpriteMetaData ();
                spriteMetaData.rect = gridRects[i];
                spriteMetaData.alignment = (int) slicingOptions.Pivot;
                spriteMetaData.pivot = slicingOptions.CustomPivot;
                spriteMetaData.name = texture.name + "_" + i;
                sprites.Add (spriteMetaData);
            }
            return sprites.ToArray ();
        }
        static Rect[] GetAllSliceRectsForTexture (Texture2D texture, Vector2 cellSize)
        {
            int numSpritesTall = Mathf.FloorToInt (texture.height / cellSize.y);
            int numSpritesWide = Mathf.FloorToInt (texture.width / cellSize.x);
            float remainderY = texture.height - (numSpritesTall * cellSize.y);
            int i = 0;
            Rect[] rects = new Rect[numSpritesWide * numSpritesTall];
            for (int y = numSpritesTall - 1; y >= 0; y--) {
                for (int x = 0; x < numSpritesWide; x++) {
                    Rect rect = new Rect (x * cellSize.x, y * cellSize.y + remainderY, cellSize.x, cellSize.y);
                    rects[i++] = rect;
                }
            }
    
            return rects;
        }
    
        public static SpriteMetaData[] CreateSpriteSheetForTextureBogdan (Texture2D texture, SpriteSlicingOptions slicingOptions)
        {
            Rect[] gridRects = InternalSpriteUtility.GenerateGridSpriteRectangles(texture, Vector2.zero,
                slicingOptions.CellSize, Vector2.zero);
            
            string path = AssetDatabase.GetAssetPath(texture);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
    
            var spriteSheet = importer.spritesheet ?? new SpriteMetaData[gridRects.Length];
    
            // Add new sprite meta data to the end for all the newly parsed grid rects?
            if (importer.spritesheet != null)
                spriteSheet = spriteSheet.Concat(new SpriteMetaData[Mathf.Max(0, gridRects.Length - importer.spritesheet.Length)]).ToArray();
            
            for (var i = 0; i < spriteSheet.Length; i++)
            {
                bool sliceExists = importer.spritesheet != null && i < importer.spritesheet.Length;
                bool changePivot = !sliceExists || slicingOptions.OverridePivot;
                spriteSheet[i] = new SpriteMetaData
                {
                    alignment = changePivot ? (int) slicingOptions.Pivot: spriteSheet[i].alignment,
                    pivot = changePivot ? slicingOptions.CustomPivot : spriteSheet[i].pivot,
                    name = sliceExists ? spriteSheet[i].name : texture.name + "_" + i,
                    rect = gridRects[i]
                };
            }
            
            if (slicingOptions.Frames > 0)
                spriteSheet = spriteSheet.Take((int) slicingOptions.Frames).ToArray();
    
            return spriteSheet;
        }

		public static Sprite[] GetSortedSpritesInTexture (Texture2D texture)
		{
			string path = AssetDatabase.GetAssetPath (texture);
			if (string.IsNullOrEmpty (path)) {
				Debug.LogWarning ("Can't find sprites from Texture at path: " + path);
				return null;
			}
			Sprite[] spriteArray = AssetDatabase.LoadAllAssetsAtPath (path).OfType<Sprite> ().ToArray ();

			List<Sprite> sortedSprites = new List<Sprite> (spriteArray);
			sortedSprites.Sort (delegate(Sprite x, Sprite y) {
				return EditorUtility.NaturalCompare (x.name, y.name);
			});

			return sortedSprites.ToArray ();
		}
    }
}