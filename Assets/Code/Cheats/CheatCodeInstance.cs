using UnityEngine.Events;

namespace Code.Cheats {
    [System.Serializable]
    public class CheatCodeInstance {
        public string code;
        public UnityEvent cheatEvent;
    }
}