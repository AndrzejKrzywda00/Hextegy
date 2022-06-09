using UnityEngine;

namespace Code.Audio {
    public class Sound {
        public readonly string name;
        public readonly AudioClip clip;
        public readonly float volume;
        public readonly float pitch;
        public readonly bool loop;
        public AudioSource source;

        public Sound(string name, AudioClip clip, float volume, float pitch, bool loop) {
            this.name = name;
            this.clip = clip;
            this.volume = volume;
            this.pitch = pitch;
            this.loop = loop;
        }
    }
}