using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Generator.Util { 
    public abstract class PerlinNoise {
        public static Boolean[,] Generate(int height, int width, float scale, float fulfil) {
            
            float offsetX = Random.value * scale; 
            float offsetY = Random.value * scale; 
            bool[,] noiseMap = new bool[height, width];
            
            for (int zIndex = 0; zIndex < height; zIndex ++) { 
                for (int xIndex = 0; xIndex < width; xIndex++) {
            
                    // calculate sample indices based on the coordinates and the scale
                    float sampleX = xIndex / scale;
                    float sampleZ = zIndex / scale;
                    
                    // generate noise value using PerlinNoise
                    float noise = Mathf.PerlinNoise(sampleX + offsetX, sampleZ + offsetY);
                    
                    noiseMap[zIndex, xIndex] = noise < fulfil;
                }
            }
            return noiseMap;
        }
    }
}
