using UnityEngine;

namespace Code.Generator {
    public class Prefabs {
        private static House _house = Resources.Load<House>("House");
        private static Tree _tree = Resources.Load<Tree>("Tree");
        private static NormalTower _normalTower = Resources.Load<NormalTower>("NormalTower");
        private static SuperTower _superTower = Resources.Load<SuperTower>("SuperTower");
        private static NoElement _noElement = Resources.Load<NoElement>("NoElement");
        private static CommonKnight _commonKnight = Resources.Load<CommonKnight>("CommonKnight");
        private static Capital _capital = Resources.Load<Capital>("Capital");

        public static House getHouse() {
            return _house;
        }
        
        public static Tree getTree() {
            return _tree;
        }
        
        public static NormalTower getNormalTower() {
            return _normalTower;
        }
        
        public static SuperTower getSuperTower() {
            return _superTower;
        }
        
        public static NoElement getNoElement() {
            return _noElement;
        }
        
        public static CommonKnight getCommonKnight() {
            return _commonKnight;
        }
        
        public static Capital getCapital(int playerId) {
            _capital.SetPlayerId(playerId);
            return _capital;
        }
    }
}