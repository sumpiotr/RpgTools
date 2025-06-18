using UnityEngine;

public class MusicOnLoadPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip musicClip;
    void Start()
    {
        MusicManager.Instance.PlayMusic(musicClip);
    }

}
