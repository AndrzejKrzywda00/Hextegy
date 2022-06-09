using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Audio {
    public class AudioManager : MonoBehaviour {
        private static AudioManager _instance;

        public Sound[] sounds;

        private bool _specialSounds;
        private string _sceneName;
        private bool IsPreGameScene => _sceneName.Equals("Menu") || _sceneName.Equals("Settings");
        
        private void Awake() {
            if (_instance == null) _instance = this;
            
            InitializeSounds();

            _sceneName = SceneManager.GetActiveScene().name;

            GameObject gObject = GameObject.FindGameObjectWithTag("MenuMusic");
            if (gObject == null || IsPreGameScene) return;
            AudioSource menuMusic = gObject.GetComponent<AudioSource>();
            menuMusic.Stop();
            Destroy(menuMusic.gameObject);
        }

        private void InitializeSounds() {
            sounds = new[] {
                new Sound("Building", Resources.Load<AudioClip>("Sounds/Building"), 0.5f, 1f, false),
                new Sound("CapitalLost", Resources.Load<AudioClip>("Sounds/CapitalLost"), 0.5f, 1f, false),
                new Sound("Conquest", Resources.Load<AudioClip>("Sounds/Conquest"), 0.5f, 1f, false),
                new Sound("Death", Resources.Load<AudioClip>("Sounds/Death"), 0.5f, 1f, false),
                new Sound("DestroyBuilding", Resources.Load<AudioClip>("Sounds/DestroyBuilding"), 0.5f, 1f, false),
                new Sound("DestroyTree", Resources.Load<AudioClip>("Sounds/DestroyTree"), 0.5f, 1f, false),
                new Sound("Move", Resources.Load<AudioClip>("Sounds/Move"), 0.5f, 1f, false),
                new Sound("ReadyToFight", Resources.Load<AudioClip>("Sounds/ReadyToFight"), 0.5f, 1f, false)
            };
            
            AddAudioSourceForEachSound();
        }
        
        private void AddAudioSourceForEachSound() {
            foreach (Sound sound in sounds) {
                if (sound.source != null) return;
                sound.source = gameObject.AddComponent<AudioSource>();
                
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        public static void Play(string name) {
            if (!name.Equals("Greensleeves") && _instance._specialSounds == false) return;
            
            Sound sound = Array.Find(_instance.sounds, sound => sound.name == name);
            if (sound == null) {
                Debug.LogWarning($"Sound: {name} not found!");
                return;
            }
            sound.source.Play();
        }

        public void ChangeSoundsActivationStatus() {
            _specialSounds = !_specialSounds;
        }
    }
}
