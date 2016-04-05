namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    /// <summary>
    /// Sprite Slicing utility helps to slice sprites in alternate methods than Unity's auto slicer
    /// </summary>
    public static class SpriteSlicer
    {
        /// <summary>
        /// Creates a spritesheet for a texture based on the slicing options
        /// </summary>
        /// <returns>The sprite sheet metadata for the texture.</returns>
        /// <param name="texture">Texture to slice.</param>
        /// <param name="slicingOptions">Slicing options.</param>
        public static SpriteMetaData[] CreateSpriteSheetForTexture(Texture2D texture, SpriteSlicingOptions slicingOptions)
        {
            List<SpriteMetaData> sprites = new List<SpriteMetaData>();
            Rect[] gridRects = GetAllSliceRectsForTexture(texture, slicingOptions.CellSize);
            for (int i = 0; i < gridRects.Length; i++)
            {
                SpriteMetaData spriteMetaData = new SpriteMetaData();
                spriteMetaData.rect = gridRects[i];
                spriteMetaData.alignment = (int)slicingOptions.Pivot;
                spriteMetaData.pivot = slicingOptions.CustomPivot;
                spriteMetaData.name = texture.name + "_" + i;
                sprites.Add(spriteMetaData);
            }

            return sprites.ToArray();
        }

        private static Rect[] GetAllSliceRectsForTexture(Texture2D texture, Vector2 cellSize)
        {
            int numSpritesTall = Mathf.FloorToInt(texture.height / cellSize.y);
            int numSpritesWide = Mathf.FloorToInt(texture.width / cellSize.x);
            float remainderY = texture.height - (numSpritesTall * cellSize.y);
            int i = 0;
            Rect[] rects = new Rect[numSpritesWide * numSpritesTall];
            for (int y = numSpritesTall - 1; y >= 0; y--)
            {
                for (int x = 0; x < numSpritesWide; x++)
                {
                    Rect rect = new Rect(x * cellSize.x, (y * cellSize.y) + remainderY, cellSize.x, cellSize.y);
                    rects[i++] = rect;
                }
            }
    
            return rects;
        }
    }
}