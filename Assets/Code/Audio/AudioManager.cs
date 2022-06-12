using System;
using Code.CellObjects;
using Code.CellObjects.Structures.Passive;
using Code.CellObjects.Structures.StateBuildings;
using Code.CellObjects.Structures.Towers;
using Code.CellObjects.Units;
using Code.CellObjects.Units.Implementations;
using Code.DataAccess;
using Code.Hexagonal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = Unity.Mathematics.Random;
using Tree = Code.CellObjects.Structures.Trees.Tree;

namespace Code.Audio {
    public class AudioManager : MonoBehaviour {
        private static AudioManager _instance;
        private static Random _random;
        
        private Sound[] _sounds;
        private string _sceneName;
        private bool IsPreGameScene => _sceneName.Equals("Menu") || _sceneName.Equals("Settings") || _sceneName.Equals("Instruction");
        
        private void Awake() {
            _random.InitState();
            _instance = this;
            _sceneName = SceneManager.GetActiveScene().name;

            Settings.IsSoundEnabled = true;
            InitializeSounds();

            GameObject gObject = GameObject.FindGameObjectWithTag("MenuMusic");
            if (gObject == null || IsPreGameScene) return;
            AudioSource menuMusic = gObject.GetComponent<AudioSource>();
            menuMusic.Stop();
            Destroy(menuMusic.gameObject);
        }

        private void Start() {
            if (_sceneName.Equals("Endgame")) {
                Play(SoundNames.Winning.ToString());
            }
        }

        private void InitializeSounds() {
            _sounds = new[] {
                // Structures
                new Sound("Building", Resources.Load<AudioClip>("Sounds/Structures/Building"), 0.5f, 1f, false),
                new Sound("CapitalLost", Resources.Load<AudioClip>("Sounds/Structures/CapitalLost"), 0.5f, 1f, false),
                new Sound("CuttingDownTree", Resources.Load<AudioClip>("Sounds/Structures/CuttingDownTree"), 0.5f, 1f, false),
                new Sound("DestroyBuilding", Resources.Load<AudioClip>("Sounds/Structures/DestroyBuilding"), 0.5f, 1f, false),

                // UnitTier1
                new Sound("CosTrzebaZrobic", Resources.Load<AudioClip>("Sounds/Units/UnitTier1/Andrzej-CosTrzebaZrobic"), 0.1f, 1f, false),
                new Sound("WedleRozkazu", Resources.Load<AudioClip>("Sounds/Units/UnitTier1/Andrzej-WedleRozkazu"), 0.1f, 1f, false),
                new Sound("DzwiekSmierciOuu", Resources.Load<AudioClip>("Sounds/Units/UnitTier1/Andrzej-DzwiekSmierciOuu"), 0.15f, 1f, false),
                
                // UnitTier2
                new Sound("PracaPraca", Resources.Load<AudioClip>("Sounds/Units/UnitTier2/Szymon-PracaPraca"), 0.4f, 1f, false),
                new Sound("Czego", Resources.Load<AudioClip>("Sounds/Units/UnitTier2/Szymon-Czego"), 0.4f, 1f, false),
                new Sound("DzwiekSmierciNoWKoncu", Resources.Load<AudioClip>("Sounds/Units/UnitTier2/Szymon-DzwiekSmierciNoWKoncu"), 0.6f, 1f, false),

                // UnitTier3
                new Sound("TyJestesKrolem", Resources.Load<AudioClip>("Sounds/Units/UnitTier3/Michal-TyJestesKrolem"), 0.55f, 1f, false),
                new Sound("Slucham", Resources.Load<AudioClip>("Sounds/Units/UnitTier3/Michal-Slucham"), 0.4f, 1f, false),
                new Sound("DzwiekSmierciSssmierc", Resources.Load<AudioClip>("Sounds/Units/UnitTier3/Michal-DzwiekSmierciSssmierc"), 0.7f, 1f, false),

                // UnitTier4
                new Sound("NaRozkaz", Resources.Load<AudioClip>("Sounds/Units/UnitTier4/Krzysiek-NaRozkaz"), 0.5f, 1f, false),
                new Sound("JuzIde", Resources.Load<AudioClip>("Sounds/Units/UnitTier4/Krzysiek-JuzIde"), 0.5f, 1f, false),
                new Sound("DzwiekSmierciOoo", Resources.Load<AudioClip>("Sounds/Units/UnitTier4/Krzysiek-DzwiekSmierciOoo"), 0.3f, 1f, false),

                // Endgame sound
                new Sound("Winning", Resources.Load<AudioClip>("Sounds/Winning"), 0.25f, 1f, false)
            };
            
            AddAudioSourceForEachSound();
        }
        
        private void AddAudioSourceForEachSound() {
            foreach (Sound sound in _sounds) {
                if (sound.Source != null) return;
                sound.Source = gameObject.AddComponent<AudioSource>();
                
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.loop = sound.Loop;
            }
        }

        private static void Play(string name) {
            if (!name.Equals("Greensleeves") && Settings.IsSoundEnabled == false) return;
            
            Sound sound = Array.Find(_instance._sounds, sound => sound.Name == name);
            if (sound == null) {
                Debug.LogWarning($"Sound: {name} not found!");
                return;
            }
            sound.Source.Play();
        }

        public static void ToggleSoundActivationStatus() {
            Settings.IsSoundEnabled = !Settings.IsSoundEnabled;
        }

        public static void PlaySoundWhenBuyingOnFriendlyCell(HexCell hexCell, ActiveObject prefabFromUI) {
            if (hexCell.prefabInstance is Tree) {
                Play(SoundNames.CuttingDownTree.ToString());
            } else {
                PlaySoundWhenBuying(prefabFromUI);
            }
        }

        private static void PlaySoundWhenBuying(ActiveObject prefabFromUI) {
            switch (prefabFromUI) {
                case Farm _:
                case TowerTier1 _: 
                case TowerTier2 _: Play(SoundNames.Building.ToString()); break;
                case UnitTier1 _: Play(SoundNames.CosTrzebaZrobic.ToString()); break;
                case UnitTier2 _: Play(SoundNames.PracaPraca.ToString()); break;
                case UnitTier3 _: Play(SoundNames.TyJestesKrolem.ToString()); break;
                case UnitTier4 _: Play(SoundNames.NaRozkaz.ToString()); break;
            }
        }

        public static void PlaySoundWhenBuyingOnEnemyOrNeutralCell(HexCell hexCell, ActiveObject prefabFromUI) {
            if (hexCell.prefabInstance is NoElement) {
                PlaySoundWhenBuying(prefabFromUI);
                return;
            }
            
            PlayDeathSound(hexCell.prefabInstance);
        }

        private static void PlayDeathSound(CellObject prefabInstance) {
            switch (prefabInstance) {
                case Grave _:
                case Farm _:
                case TowerTier1 _:
                case TowerTier2 _: Play(SoundNames.DestroyBuilding.ToString()); break;
                case Tree _: Play(SoundNames.CuttingDownTree.ToString()); break;
                case UnitTier1 _: Play(SoundNames.DzwiekSmierciOuu.ToString()); break;
                case UnitTier2 _: Play(SoundNames.DzwiekSmierciNoWKoncu.ToString()); break;
                case UnitTier3 _: Play(SoundNames.DzwiekSmierciSssmierc.ToString()); break;
                case UnitTier4 _: Play(SoundNames.DzwiekSmierciOoo.ToString()); break;
                case Capital _: Play(SoundNames.CapitalLost.ToString()); break;
            }
        }

        public static void PlaySoundWhenSelectingUnit(HexCell selectedCellWithUnit) {
            switch (selectedCellWithUnit.prefabInstance) {
                case UnitTier1 _: Play(SoundNames.WedleRozkazu.ToString()); break;
                case UnitTier2 _: Play(SoundNames.Czego.ToString()); break;
                case UnitTier3 _: Play(SoundNames.Slucham.ToString()); break;
                case UnitTier4 _: Play(SoundNames.JuzIde.ToString()); break;
            }
        }
        
        public static void PlaySoundWhenMovingOnEnemyOrNeutralCell(HexCell hexCell) {
            PlayDeathSound(hexCell.prefabInstance);
        }
        
        public static void PlaySoundWhenMovingOnFriendlyCells(HexCell hexCell) {
            if (hexCell.prefabInstance is Tree) {
                Play(SoundNames.CuttingDownTree.ToString());
            }
        }

    }
}
