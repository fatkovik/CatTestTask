using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    //[SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip musicClip;
    //[SerializeField] private AudioClip correctClickSFX; 
    //same with other sfx if needed

    [SerializeField] private Button audioToggleButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }

        audioToggleButton?.onClick.AddListener(ToggleMusic);
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    private void OnDisable()
    {
        audioToggleButton?.onClick.RemoveListener(ToggleMusic);
    }

    //same with other sfx if needed
    //public void PlayCorrectClick() => PlaySFX(correctClickSFX);

    //private void PlaySFX(AudioClip clip)
    //{
    //    if (clip != null)
    //        sfxSource.PlayOneShot(clip);
    //}
}
