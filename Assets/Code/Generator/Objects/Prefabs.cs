using System.Collections.Generic;
using UnityEngine;

namespace Code.Generator {
    public class Prefabs {
        
        private static House _house = Resources.Load<House>("House");
        private static NormalTower _normalTower = Resources.Load<NormalTower>("NormalTower");
        private static SuperTower _superTower = Resources.Load<SuperTower>("SuperTower");
        private static NoElement _noElement = Resources.Load<NoElement>("NoElement");
        private static CommonKnight _commonKnight = Resources.Load<CommonKnight>("CommonKnight");
        private static Capital _capital = Resources.Load<Capital>("Capital");
        
        // trees
        // in future different script for small tree
        private static Tree _basicTree = Resources.Load<Tree>("Tree");
        private static Tree _smallTree = Resources.Load<Tree>("SmallTree");
        private static List<Tree> _trees = new List<Tree>{_basicTree, _smallTree};

        public static House GetHouse() {
            return _house;
        }
        
        public static Tree GetTree() {
            int randomIndex = Random.Range(0, _trees.Count);
            return _trees[randomIndex];
        }
        
        public static NormalTower GetNormalTower() {
            return _normalTower;
        }
        
        public static SuperTower GetSuperTower() {
            return _superTower;
        }
        
        public static NoElement GetNoElement() {
            return _noElement;
        }
        
        public static CommonKnight GetCommonKnight() {
            return _commonKnight;
        }
        
        public static Capital GetCapital(int playerId) {
            _capital.SetPlayerId(playerId);
            return _capital;
        }
    }
}