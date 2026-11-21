using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip placeBlock;
    public AudioClip fillRow;
    public AudioClip lose;
    private void Start() {
        musicSource.clip = background;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
