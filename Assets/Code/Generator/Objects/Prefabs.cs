using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Generator.Objects {
    public abstract class Prefabs {
        
        private static readonly House House = Resources.Load<House>("House");
        private static readonly NormalTower NormalTower = Resources.Load<NormalTower>("NormalTower");
        private static readonly SuperTower SuperTower = Resources.Load<SuperTower>("SuperTower");
        private static readonly NoElement NoElement = Resources.Load<NoElement>("NoElement");
        private static readonly CommonKnight CommonKnight = Resources.Load<CommonKnight>("CommonKnight");
        private static readonly ExperiencedKnight ExperiencedKnight = Resources.Load<ExperiencedKnight>("ExperiencedKnight");
        private static readonly LegendaryKnight LegendaryKnight = Resources.Load<LegendaryKnight>("LegendaryKnight");
        private static readonly Capital Capital = Resources.Load<Capital>("Capital");
        private static readonly Grave Grave = Resources.Load<Grave>("Grave");
        
        // trees
        // in future different script for small tree
        private static readonly Tree BasicTree = Resources.Load<Tree>("Tree");
        private static readonly Tree SmallTree = Resources.Load<Tree>("SmallTree");
        private static readonly List<Tree> Trees = new List<Tree>{BasicTree, SmallTree};

        public static House GetHouse() {
            return House;
        }
        
        public static Tree GetTree() {
            int randomIndex = Random.Range(0, Trees.Count);
            return Trees[randomIndex];
        }
        
        public static NormalTower GetNormalTower() {
            return NormalTower;
        }
        
        public static SuperTower GetSuperTower() {
            return SuperTower;
        }
        
        public static NoElement GetNoElement() {
            return NoElement;
        }
        
        public static CommonKnight GetCommonKnight() {
            return CommonKnight;
        }

        public static ExperiencedKnight GetExperiencedKnight() {
            return ExperiencedKnight;
        }

        public static LegendaryKnight GetLegendaryKnight() {
            return LegendaryKnight;
        }
        
        public static Capital GetCapital(int playerId) {
            Capital.SetPlayerId(playerId);
            return Capital;
        }
    }
}