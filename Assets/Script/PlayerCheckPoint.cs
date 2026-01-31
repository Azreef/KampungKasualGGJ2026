using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    public Vector3 checkpointPosition;

    [Header("Audio")]
    public AudioSource audioSource;
    public List<AudioClip> checkpointClips;
    public List<AudioClip> spikeAudioClips;

    void Start()
    {
        checkpointPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            PlayRandomSpikeSound();
            transform.position = checkpointPosition;
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            checkpointPosition = collision.transform.position;

            if (!collision.GetComponent<CheckpointAudio>().alreadyPlayedOnce)
            {
                PlayRandomCheckpointSound();
                collision.GetComponent<CheckpointAudio>().alreadyPlayedOnce = true;
            }
        }
    }

    void PlayRandomCheckpointSound()
    {
        if (audioSource == null || checkpointClips.Count == 0)
            return;

        int randomIndex = Random.Range(0, checkpointClips.Count);
        audioSource.PlayOneShot(checkpointClips[randomIndex]);
    }

    void PlayRandomSpikeSound()
    {
        if (audioSource == null || spikeAudioClips.Count == 0)
            return;

        int randomIndex = Random.Range(0, spikeAudioClips.Count);
        audioSource.PlayOneShot(spikeAudioClips[randomIndex]);
    }
}
