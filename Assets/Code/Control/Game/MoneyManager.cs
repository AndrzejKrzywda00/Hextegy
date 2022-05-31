using System.Collections.Generic;

namespace Code.Control.Game {
    public static class MoneyManager {
    
        private const int InitialMoney = 100;
        private static Dictionary<int, int> _playersBalances;
        private static Dictionary<int, int> _playersWallets;

        public static int GetCurrentCoins(int playerId) {
            return _playersWallets[playerId];
        }

        public static void IncrementBalance(int playerId) {
            _playersBalances[playerId] += 1;
        }

        private static void IncrementBalanceOfPlayer(int playerId) {
            if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] += 1;
        }

        public static void DecrementBalanceOfPlayer(int playerId) {
            if(_playersBalances.ContainsKey(playerId)) _playersBalances[playerId] -= 1;
        }

        public static void TransferBalanceOfFieldFromPlayerToPlayer(int playerId1, int playerId2) {
            DecrementBalanceOfPlayer(playerId1);
            IncrementBalanceOfPlayer(playerId2);
        }

        public static void SetInitialBalanceOfPlayers(Dictionary<int, int> initialBalanceOfPlayers) {
            _playersBalances = initialBalanceOfPlayers;
            InitializeWallets();
        }

        private static void InitializeWallets() {
            _playersWallets = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> item in _playersBalances) {
                _playersWallets.Add(item.Key, InitialMoney);
            }
        }

        public static void CalculateWalletOnTurnEnd() {
            int currentPlayerBalance = _playersBalances[PlayerController.CurrentPlayerId];
            _playersWallets[PlayerController.CurrentPlayerId] += currentPlayerBalance;
        }

        public static int GetBalance(int playerId) {
            return _playersBalances[playerId];
        }

        public static void Buy(CellObject cellObject, int playerId) {
            _playersBalances[playerId] -= cellObject.GetMaintenanceCost();
            _playersWallets[playerId] -= cellObject.GetPrice();
        }

        public static bool HasEnoughMoneyToBuy(CellObject entity, int playerId) {
            return entity.GetPrice() <= _playersWallets[playerId];
        }
    }
}
