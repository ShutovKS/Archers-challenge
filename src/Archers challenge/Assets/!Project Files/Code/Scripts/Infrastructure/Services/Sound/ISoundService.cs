using UnityEngine;

namespace Infrastructure.Services.Sound
{
    public interface ISoundService
    {
        void PlaySound(AudioClip clip);

        void Stop();
    }

    public class SoundBackgroundService : ISoundService
    {
        private readonly AudioSource _audioSource;

        public SoundBackgroundService()
        {
            var gameObject = new GameObject("SoundService");

            Object.DontDestroyOnLoad(gameObject);

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.playOnAwake = false;
            _audioSource.volume = 0.25f;
        }

        public void PlaySound(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}