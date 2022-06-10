using UnityEngine;

namespace Code.Audio {
    public class Sound {
        
        public readonly string Name;
        public readonly AudioClip Clip;
        public readonly float Volume;
        public readonly float Pitch;
        public readonly bool Loop;
        public AudioSource Source;

        public Sound(string name, AudioClip clip, float volume, float pitch, bool loop) {
            Name = name;
            Clip = clip;
            Volume = volume;
            Pitch = pitch;
            Loop = loop;
        }
    }
}