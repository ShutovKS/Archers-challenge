using UnityEngine;

namespace BowAndArrow.ArrowScripts
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip[] clips;

        public void PlayRandomClip()
        {
            if (source)
            {
                source.clip = clips[Random.Range(0, clips.Length)];
                source.Play();
            }
        }
    }
}