using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckPoint : MonoBehaviour
{
    public Vector3 checkpointPosition;

    [Header("Audio")]
    public AudioSource audioSource;

    public List<AudioClip> checkpointClips;
    public List<AudioClip> spikeAudioClips;
    public List<AudioClip> torchAudioClips;

    [Header("Spike Settings")]
    public float spikeSoundCooldown = 0.5f;
    private bool canPlaySpikeSound = true;

    private bool isNearTorch = false;

    void Start()
    {
        checkpointPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            if (canPlaySpikeSound)
            {
                PlayRandomSpikeSound();
                StartCoroutine(SpikeSoundCooldown());
            }

            transform.position = checkpointPosition;
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            checkpointPosition = collision.transform.position;

            CheckpointAudio checkpointAudio = collision.GetComponent<CheckpointAudio>();
            if (checkpointAudio != null && !checkpointAudio.alreadyPlayedOnce)
            {
                PlayRandomCheckpointSound();
                checkpointAudio.alreadyPlayedOnce = true;
            }
        }
        else if (collision.CompareTag("Torch"))
        {
            if (isNearTorch)
            {
                PlayRandomTorchSound();
                isNearTorch = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Torch"))
        {
            isNearTorch = true;
        }
    }

    IEnumerator SpikeSoundCooldown()
    {
        canPlaySpikeSound = false;
        yield return new WaitForSeconds(spikeSoundCooldown);
        canPlaySpikeSound = true;
    }

    void PlayRandomCheckpointSound()
    {
        if (checkpointClips.Count == 0) return;
        audioSource.PlayOneShot(
            checkpointClips[Random.Range(0, checkpointClips.Count)]
        );
    }

    void PlayRandomSpikeSound()
    {
        if (spikeAudioClips.Count == 0) return;
        audioSource.PlayOneShot(
            spikeAudioClips[Random.Range(0, spikeAudioClips.Count)]
        );
    }

    void PlayRandomTorchSound()
    {
        if (torchAudioClips.Count == 0) return;
        audioSource.PlayOneShot(
            torchAudioClips[Random.Range(0, torchAudioClips.Count)]
        );
    }
}
