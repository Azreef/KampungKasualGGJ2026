using UnityEngine;
using UnityEngine.Audio;

public class CheckpointAudio : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource mAudioSource;
    public bool alreadyPlayedOnce=false;

    public GameObject activatedSprite;
    public GameObject idleSprite;

    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();

        //activatedSprite.SetActive(false);
        //idleSprite.SetActive(true);

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!alreadyPlayedOnce)
    //    {
    //        PlayRandomAudio();
    //        alreadyPlayedOnce = true;

    //        activatedSprite.SetActive(true);
    //        idleSprite.SetActive(false);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Correct: check the collider's tag, don't call SetActive() inside the condition.
        if (other.CompareTag("Player"))
        {
            // Play audio once when first triggered (optional, mirrors collision behavior)
            if (!alreadyPlayedOnce)
            {
                PlayRandomAudio();
                alreadyPlayedOnce = true;
            }

            activatedSprite?.SetActive(true);
            idleSprite?.SetActive(false);
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
