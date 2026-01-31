using UnityEngine;

public class TorchAudio : MonoBehaviour
{
    public AudioClip audioClips;
    private AudioSource mAudioSource;


    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mAudioSource.PlayOneShot(audioClips);
        }
    }
    private void OnDestroy()
    {
        PlayRandomAudio();
    }
    void PlayRandomAudio()
    {
        //if (audioClips.Length == 0)
        //{
        //    Debug.LogWarning("Audio Clips array is empty!");
        //    return;
        //}

        //// Pick a random index from the array
        //int randomIndex = Random.Range(0, audioClips.Length);

        //mAudioSource.PlayOneShot(audioClips[randomIndex]);
    }
}
