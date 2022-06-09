using System.Collections.Generic;
using Code.CellObjects;
using Code.CellObjects.Structures.StateBuildings;

namespace Code.Control.Game {
    public static class MoneyManager {
    
        private const int InitialMoney = 10;
        private static Dictionary<int, int> _playersBalances;
        private static Dictionary<int, int> _playersWallets;
        private static Dictionary<int, int> _playersFarmsAmounts;

        public static int GetCurrentCoins(int playerId) {
            return _playersWallets[playerId];
        }

        public static void IncrementBalance(int playerId) {
            _playersBalances[playerId] += 1;
        }

        private static void IncrementBalanceOfPlayer(int playerId) {
            if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId]++;
        }

        public static void DecrementBalanceOfPlayer(int playerId) {
            if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId]--;
        }

        public static void IncrementBalanceOfPlayerByAmount(int playerId, int amount) {
            if (_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] += amount;
        }

        public static void TransferBalanceOfFieldFromPlayerToPlayer(int playerId1, int playerId2) {
            DecrementBalanceOfPlayer(playerId1);
            IncrementBalanceOfPlayer(playerId2);
        }

        public static void SetInitialBalanceOfPlayers(Dictionary<int, int> initialBalanceOfPlayers) {
            _playersBalances = initialBalanceOfPlayers;
            InitializeWalletsAndFarms();
        }

        private static void InitializeWalletsAndFarms() {
            _playersWallets = new Dictionary<int, int>();
            _playersFarmsAmounts = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> item in _playersBalances) {
                _playersWallets.Add(item.Key, InitialMoney);
                _playersFarmsAmounts.Add(item.Key, 0);
            }
        }

        private static void IncrementPlayerFarms(int playerId) {
            if (_playersFarmsAmounts.ContainsKey(playerId)) _playersFarmsAmounts[playerId]++;
        }

        public static void DecrementPlayerFarms(int playerId) {
            if (_playersFarmsAmounts.ContainsKey(playerId)) _playersFarmsAmounts[playerId]--;
        }

        public static void CalculateWalletOnTurnEnd() {
            int currentPlayerBalance = _playersBalances[PlayerController.CurrentPlayerId];
            _playersWallets[PlayerController.CurrentPlayerId] += currentPlayerBalance;
            if (_playersWallets[PlayerController.CurrentPlayerId] <= 0) {
                _playersWallets[PlayerController.CurrentPlayerId] = 0;
                PlayerController.DestroyAllUnitsOfPlayer();
            }
        }

        public static int GetBalance(int playerId) {
            return _playersBalances[playerId];
        }

        public static void Buy(ActiveObject cellObject, int playerId) {
            _playersBalances[playerId] -= cellObject.MaintenanceCost();
            _playersWallets[playerId] -= GetCalculatedPrice(cellObject, playerId);
        }

        private static int GetCalculatedPrice(ActiveObject activeObject, int playerId) {
            if (!(activeObject is Farm)) return activeObject.Price();
            int price = activeObject.Price() + 2 * _playersFarmsAmounts[playerId];
            IncrementPlayerFarms(playerId);
            return price;
        }

        public static bool HasEnoughMoneyToBuy(ActiveObject activeObject, int playerId) {
            return activeObject.Price() <= _playersWallets[playerId];
        }
    }
}
