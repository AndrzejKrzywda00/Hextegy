using Code.CellObjects;
using Code.CellObjects.Structures.Passive;
using Code.CellObjects.Structures.StateBuildings;
using Code.CellObjects.Structures.Towers;
using Code.CellObjects.Units;
using Code.Control.Game;
using Code.DataAccess;
using UnityEngine;
using Tree = Code.CellObjects.Structures.Trees.Tree;

namespace Code.Hexagonal {
    public class HexCell : MonoBehaviour {
    
        public HexCoordinates coordinates;
        public CellObject prefabInstance;
        public int playerId;

        public void PutOnCell(CellObject prefab) {
            prefabInstance = Instantiate(prefab);
            AlignPrefabInstancePositionWithCellPosition();
        }

        public void AlignPrefabInstancePositionWithCellPosition() {
            Vector3 position = transform.localPosition;
            position.y += 2; // Here content is raised above grid level to be visible
            prefabInstance.transform.position = position;
        }

        public Color GetCellColor() {
            return ColorPalette.GetColorOfPlayer(playerId);
        }

        public HexCoordinates[] NeighborsCoordinates() {
            return coordinates.Neighbors();
        }
    
        public bool IsEmpty() {
            return prefabInstance is NoElement;
        }

        public bool IsNeutral() {
            return playerId.Equals(0);
        }

        public bool HasTree() {
            return prefabInstance is Tree;
        }

        public bool HasFarm() {
            return prefabInstance is Farm;
        }

        public bool HasTower() {
            return prefabInstance is TowerTier1 || prefabInstance is TowerTier2;
        }

        public bool HasProtectiveInstance() {
            return prefabInstance is ActiveObject;
        }
    
        public bool HasUnit() {
            return prefabInstance is Unit;
        }

        public bool IsEnemyCell() {
            return playerId != PlayerController.CurrentPlayerId && playerId != 0;
        }

        public int HexDistanceTo(HexCell hexCell) {
            return coordinates.HexDistanceTo(hexCell.coordinates);
        }

        public float GaussianDistanceTo(HexCell hexCell) {
            return coordinates.GaussianDistanceTo(hexCell.coordinates);
        }

        public bool IsFriendlyCell() {
            return playerId.Equals(PlayerController.CurrentPlayerId);
        }
    }
}
