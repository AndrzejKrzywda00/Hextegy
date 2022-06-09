using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Cheats {
    public class CheatCodeManager : MonoBehaviour {
        [SerializeField]
        private bool playerTyping = false;
        [SerializeField]
        private string currentString = "";
        [SerializeField]
        private List<CheatCodeInstance> cheatCodeList = new List<CheatCodeInstance>();

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                if (playerTyping) CheckCheat(currentString);
                playerTyping = !playerTyping;
            }

            if (!playerTyping) return;
            foreach (char c in Input.inputString) {
                switch (c) {
                    case '\b': {
                        if (currentString.Length > 0) currentString = currentString.Substring(0, currentString.Length - 1);
                        break;
                    }
                    case '\n':
                    case '\r':
                        currentString = "";
                        break;
                    default:
                        currentString += c;
                        break;
                }
            }
        }

        private bool CheckCheat(string input) {
            foreach (CheatCodeInstance cheatCode in cheatCodeList) {
                if (input != cheatCode.code) continue;
                cheatCode.cheatEvent?.Invoke();
                return true;
            }
            return false;
        }
    }
}