using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Audio {
    public class AudioManager : MonoBehaviour {
        private static AudioManager _instance;
        
        public Sound[] sounds;
        public bool specialSounds;

        private string _sceneName;
        private bool IsPreGameScene => _sceneName.Equals("Menu") || _sceneName.Equals("Settings");
        
        private void Awake() {
            _sceneName = SceneManager.GetActiveScene().name;
            AddAudioSourceForEachSound();

            if (IsPreGameScene) {
                if (_instance == null) {
                    _instance = this;
                } else {
                    Destroy(gameObject);
                    return;
                }
                DontDestroyOnLoad(gameObject);
            } else {
                if (_instance == null) return;
                Destroy(_instance.gameObject);
                _instance = null;
            }
        }

        private void Start() {
            if (_sceneName.Equals("Menu") || _sceneName.Equals("Settings")) {
                Play("Greensleeves");
            } 
        }

        private void AddAudioSourceForEachSound() {
            foreach (Sound sound in sounds) {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        public void Play(string name) {
            if (!name.Equals("Greensleeves") && !specialSounds) return;
            
            Sound sound = Array.Find(sounds, sound => sound.name == name);
            if (sound == null) {
                Debug.LogWarning($"Sound: {name} not found!");
                return;
            }

            sound.source.Play();
        }

        public void ChangeSoundsActivationStatus() {
            specialSounds = !specialSounds;
        }
    }
}
