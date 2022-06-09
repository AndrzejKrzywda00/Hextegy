using UnityEngine;

public class MenuMusic : MonoBehaviour {
    private static MenuMusic _instance;
    
    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
