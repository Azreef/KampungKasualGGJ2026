using UnityEngine;
using UnityEngine.Audio;

public class CheckpointAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource mAudioSource;
    public bool alreadyPlayedOnce=false;


    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!alreadyPlayedOnce)
        {
            PlayRandomAudio();
            alreadyPlayedOnce = true;
        }
    }

    void PlayRandomAudio()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("Audio Clips array is empty!");
            return;
        }

        // Pick a random index from the array
        int randomIndex = Random.Range(0, audioClips.Length);

        mAudioSource.PlayOneShot(audioClips[randomIndex]);
    }
}
