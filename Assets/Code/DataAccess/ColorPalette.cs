using System;
using UnityEngine;

namespace Code.DataAccess {
    
    public abstract class ColorPalette {

        // Neutral
        private static readonly Color32 Gray = Color.gray;
        
        // Standard palette
        private static readonly Color32 Olive = new Color32(95, 181, 94, 255);
        private static readonly Color32 Pink = new Color32(182, 92, 120, 255);
        private static readonly Color32 Sky = new Color32(93, 182, 176, 255);
        private static readonly Color32 Tomato = new Color32(166, 78, 64, 255);
        private static readonly Color32 DarkYellow = new Color32(180, 189, 100, 255);
        private static readonly Color32 Violet = new Color32(114, 91, 179, 255);
        
        // Alternative, darker palette
        private static readonly Color32 DarkOlive = new Color32(95 - 40, 181 - 40, 94 - 40, 255);
        private static readonly Color32 DarkPink = new Color32(182 - 40, 92 - 40, 120 - 40, 255);
        private static readonly Color32 DarkSky = new Color32(93 - 40, 182 - 40, 176 - 40, 255);
        private static readonly Color32 DarkTomato = new Color32(166 - 40, 78 - 40, 64 - 40, 255);
        private static readonly Color32 VeryDarkYellow = new Color32(180 - 40, 189 - 40, 100 - 40, 255);
        private static readonly Color32 DarkViolet = new Color32(114-40, 91-40, 179-40, 255);

        public static Color GetColorOfPlayer(int playerId) {

            return playerId switch {
                
                0 => Gray,
                
                1 => Olive,
                2 => Pink,
                3 => Sky,
                4 => Tomato,
                5 => DarkYellow,
                6 => Violet,
                
                101 => DarkOlive,
                102 => DarkPink,
                103 => DarkSky,
                104 => DarkTomato,
                105 => VeryDarkYellow,
                106 => DarkViolet,
                
                _ => throw new ArgumentException()
            };

        }

    }
}