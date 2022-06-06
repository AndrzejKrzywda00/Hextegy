using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EndgameSceneControll: MonoBehaviour {
    private TextMeshProUGUI text;

    private void Start() {
        text = GameObject.FindGameObjectWithTag("EndgameText").GetComponent<TextMeshProUGUI>();
        text.text = "The winner is Player NOTSET";
    }
}
