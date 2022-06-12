using System;
using Code.CellObjects;
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
        private bool IsPreGameScene => _sceneName.Equals("Menu") || _sceneName.Equals("Settings");
        
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
                Play(SoundNames.dzwiekWygranej.ToString());
            }
        }

        private void InitializeSounds() {
            _sounds = new[] {
                new Sound("Building", Resources.Load<AudioClip>("Sounds/Building"), 0.5f, 1f, false),
                new Sound("CapitalLost", Resources.Load<AudioClip>("Sounds/CapitalLost"), 0.5f, 1f, false),
                new Sound("Conquest", Resources.Load<AudioClip>("Sounds/Conquest"), 0.5f, 1f, false),
                new Sound("Death", Resources.Load<AudioClip>("Sounds/Death"), 0.5f, 1f, false),
                new Sound("DestroyBuilding", Resources.Load<AudioClip>("Sounds/DestroyBuilding"), 0.5f, 1f, false),
                new Sound("DestroyTree", Resources.Load<AudioClip>("Sounds/DestroyTree"), 0.3f, 1f, false),
                new Sound("Move", Resources.Load<AudioClip>("Sounds/Move"), 0.5f, 1f, false),
                new Sound("ReadyToFight", Resources.Load<AudioClip>("Sounds/ReadyToFight"), 0.5f, 1f, false),
                new Sound("Budowanie", Resources.Load<AudioClip>("Sounds/other/Budowanie"), 0.5f, 1f, false),
                new Sound("CosTrzebaZrobic", Resources.Load<AudioClip>("Sounds/other/CosTrzebaZrobic"), 0.5f, 1f, false),
                new Sound("Czego", Resources.Load<AudioClip>("Sounds/other/Czego"), 0.4f, 1f, false),
                new Sound("dzwiekDrzwi", Resources.Load<AudioClip>("Sounds/other/dzwiekDrzwi"), 0.6f, 1f, false),
                new Sound("dzwiekPrzegranej", Resources.Load<AudioClip>("Sounds/other/dzwiekPrzegranej"), 0.2f, 1f, false),
                new Sound("dzwiekSmierciEeehg", Resources.Load<AudioClip>("Sounds/other/dzwiekSmierciEeehg"), 0.1f, 1f, false),
                new Sound("dzwiekSmierciSssmierc", Resources.Load<AudioClip>("Sounds/other/dzwiekSmierciSssmierc"), 0.6f, 1f, false),
                new Sound("dzwiekSmierciUuu", Resources.Load<AudioClip>("Sounds/other/dzwiekSmierciUuu"), 0.4f, 1f, false),
                new Sound("dzwiekUmieraniaNoWKoncu", Resources.Load<AudioClip>("Sounds/other/dzwiekUmieraniaNoWKoncu"), 0.6f, 1f, false),
                new Sound("dzwiekWygranej", Resources.Load<AudioClip>("Sounds/other/dzwiekWygranej"), 0.35f, 1f, false),
                new Sound("dzwiekZdychaniaAla", Resources.Load<AudioClip>("Sounds/other/dzwiekZdychaniaAla"), 0.1f, 1f, false),
                new Sound("Hdrz", Resources.Load<AudioClip>("Sounds/other/Hdrz"), 0.4f, 1f, false),
                new Sound("JuzIde", Resources.Load<AudioClip>("Sounds/other/JuzIde"), 0.5f, 1f, false),
                new Sound("NaRozkaz", Resources.Load<AudioClip>("Sounds/other/NaRozkaz"), 0.5f, 1f, false),
                new Sound("PracaPraca", Resources.Load<AudioClip>("Sounds/other/PracaPraca"), 0.4f, 1f, false),
                new Sound("ScinanieDrzewa", Resources.Load<AudioClip>("Sounds/other/ScinanieDrzewa"), 0.5f, 1f, false),
                new Sound("Slucham", Resources.Load<AudioClip>("Sounds/other/Slucham"), 0.4f, 1f, false),
                new Sound("TyJestesKrolem", Resources.Load<AudioClip>("Sounds/other/TyJestesKrolem"), 0.55f, 1f, false),
                new Sound("WitamyWKoloni", Resources.Load<AudioClip>("Sounds/other/WitamyWKoloni"), 0.6f, 1f, false),
                new Sound("Zarombie", Resources.Load<AudioClip>("Sounds/other/Zarombie"), 0.4f, 1f, false),
                new Sound("ZnowuBlysnal", Resources.Load<AudioClip>("Sounds/other/ZnowuBlysnal"), 0.5f, 1f, false)
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
                Play(SoundNames.ScinanieDrzewa.ToString());
                return;
            }
            if (!(prefabFromUI is Unit)) {
                Play(SoundNames.Budowanie.ToString());
            } else {
                PlaySoundWhenBuyingUnit(prefabFromUI);
            }
        }

        public static void PlaySoundWhenBuyingOnEnemyOrNeutralCell(HexCell hexCell, ActiveObject prefabFromUI) {
            switch (hexCell.prefabInstance) {
                case Farm _:
                case TowerTier1 _:
                case TowerTier2 _: Play(SoundNames.DestroyBuilding.ToString()); break;
                case Tree _: Play(SoundNames.ScinanieDrzewa.ToString()); break;
                case UnitTier1 _: Play(SoundNames.dzwiekZdychaniaAla.ToString()); break;
                case UnitTier2 _: Play(SoundNames.dzwiekUmieraniaNoWKoncu.ToString()); break;
                case UnitTier3 _: Play(SoundNames.dzwiekSmierciUuu.ToString()); break;
                case UnitTier4 _: Play(SoundNames.dzwiekSmierciSssmierc.ToString()); break;
                case Capital _: Play(SoundNames.CapitalLost.ToString()); break;
                default: {
                    PlaySoundWhenBuyingUnit(prefabFromUI);
                } break;
            }
        }

        private static void PlaySoundWhenBuyingUnit(ActiveObject prefabFromUI) {
            switch (prefabFromUI) {
                case UnitTier1 _: Play(SoundNames.CosTrzebaZrobic.ToString()); break;
                case UnitTier2 _: Play(SoundNames.PracaPraca.ToString()); break;
                case UnitTier3 _: Play(SoundNames.TyJestesKrolem.ToString()); break;
                case UnitTier4 _: Play(SoundNames.NaRozkaz.ToString()); break;
            }
        }

        public static void PlaySoundWhenSelectingUnit(HexCell selectedCellWithUnit) {
            switch (selectedCellWithUnit.prefabInstance) {
                case UnitTier1 _: Play(SoundNames.dzwiekDrzwi.ToString()); break;
                case UnitTier2 _: Play(SoundNames.Czego.ToString()); break;
                case UnitTier3 _: Play(SoundNames.Slucham.ToString()); break;
                case UnitTier4 _: Play(SoundNames.JuzIde.ToString()); break;
            }
        }
        
        public static void PlaySoundWhenMovingOnEnemyOrNeutralCell(HexCell hexCell) {
            switch (hexCell.prefabInstance) {
                case Farm _:
                case TowerTier1 _:
                case TowerTier2 _: Play(SoundNames.DestroyBuilding.ToString()); break;
                case Tree _: Play(SoundNames.ScinanieDrzewa.ToString()); break;
                case UnitTier1 _: Play(SoundNames.dzwiekZdychaniaAla.ToString()); break;
                case UnitTier2 _: Play(SoundNames.dzwiekUmieraniaNoWKoncu.ToString()); break;
                case UnitTier3 _: Play(SoundNames.dzwiekSmierciUuu.ToString()); break;
                case UnitTier4 _: Play(SoundNames.dzwiekSmierciSssmierc.ToString()); break;
                case Capital _: Play(SoundNames.CapitalLost.ToString()); break;
            }
        }
        
        public static void PlaySoundWhenMovingOnFriendlyCells(HexCell hexCell) {
            if (hexCell.prefabInstance is Tree) {
                Play(SoundNames.ScinanieDrzewa.ToString());
            }
        }

    }
}
