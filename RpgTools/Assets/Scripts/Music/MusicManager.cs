using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource audioSource; 

    private AudioClip currentClip;
    private AudioClip previousClip;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        audioSource.loop = true;
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (newClip == currentClip) return;
        if (newClip == null)
        {
            StopMusic();
            return;
        }
         

        previousClip = currentClip;
        currentClip = newClip;

        audioSource.clip = currentClip;
        audioSource.Play();
    }

    public void PlayMusic(string clipName)
    {
        AudioClip loadedClip = Resources.Load<AudioClip>("Music/" + clipName);
        if (loadedClip == null)
        {
            Debug.LogWarning($"MusicManager: Nie znaleziono klipu o nazwie '{clipName}' w Resources/Music/");
            return;
        }

        PlayMusic(loadedClip);
    }

    public void PlayPreviousMusic()
    {
        if (previousClip == null)
            return;

        AudioClip temp = currentClip;
        currentClip = previousClip;
        previousClip = temp;

        audioSource.clip = currentClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void ResumeMusic()
    {
        if(currentClip != null)audioSource.Play();
    }
}
