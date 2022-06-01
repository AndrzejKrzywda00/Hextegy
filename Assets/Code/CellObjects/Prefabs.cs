using System.Collections.Generic;
using Code.CellObjects.Structures.Passive;
using Code.CellObjects.Structures.StateBuildings;
using Code.CellObjects.Structures.Towers;
using Code.CellObjects.Units.Implementations;
using UnityEngine;
using Random = UnityEngine.Random;
using Tree = Code.CellObjects.Structures.Trees;

namespace Code.CellObjects {
    public abstract class Prefabs {
        
        private static readonly Farm Farm = Resources.Load<Farm>("Farm");
        private static readonly TowerTier1 TowerTier1 = Resources.Load<TowerTier1>("TowerTier1");
        private static readonly TowerTier2 TowerTier2 = Resources.Load<TowerTier2>("TowerTier2");
        private static readonly NoElement NoElement = Resources.Load<NoElement>("NoElement");
        private static readonly UnitTier1 UnitTier1 = Resources.Load<UnitTier1>("UnitTier1");
        private static readonly UnitTier2 UnitTier2 = Resources.Load<UnitTier2>("UnitTier2");
        private static readonly UnitTier3 UnitTier3 = Resources.Load<UnitTier3>("UnitTier3");
        private static readonly UnitTier4 UnitTier4 = Resources.Load<UnitTier4>("UnitTier4");
        private static readonly Capital Capital = Resources.Load<Capital>("Capital");
        private static readonly Grave Grave = Resources.Load<Grave>("Grave");
        
        // trees
        // in future different script for small tree
        private static readonly Tree.BigTree BigTree = Resources.Load<Tree.BigTree>("BigTree");
        private static readonly Tree.SmallTree SmallTree = Resources.Load<Tree.SmallTree>("SmallTree");
        private static readonly List<Tree.Tree> Trees = new List<Tree.Tree>{BigTree, SmallTree};

        public static Farm GetFarm() {
            return Farm;
        }
        
        public static Tree.Tree GetTree() {
            int randomIndex = Random.Range(0, Trees.Count);
            return Trees[randomIndex];
        }
        
        public static TowerTier1 GetTowerTier1() {
            return TowerTier1;
        }
        
        public static TowerTier2 GetTowerTier2() {
            return TowerTier2;
        }
        
        public static NoElement GetNoElement() {
            return NoElement;
        }
        
        public static UnitTier1 GetUnitTier1() {
            return UnitTier1;
        }

        public static UnitTier2 GetUnitTier2() {
            return UnitTier2;
        }

        public static UnitTier3 GetUnitTier3() {
            return UnitTier3;
        }
        
        public static UnitTier4 GetUnitTier4() {
            return UnitTier4;
        }

        public static Grave GetGrave() {
            return Grave;
        }
        
        public static Capital GetCapital(int playerId) {
            Capital.SetPlayerId(playerId);
            return Capital;
        }
    }
}