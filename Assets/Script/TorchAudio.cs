using System.Collections.Generic;
using UnityEngine;

public class TorchAudio : MonoBehaviour
{
    [Header("Audio")]
    public List<AudioClip> audioClips;
    private AudioSource mAudioSource;

    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayRandomAudio();
        }
    }

    void PlayRandomAudio()
    {
        if (mAudioSource == null || audioClips == null || audioClips.Count == 0)
            return;

        int randomIndex = Random.Range(0, audioClips.Count);
        mAudioSource.PlayOneShot(audioClips[randomIndex]);
    }
}
