using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public bool SoundEnabled { get; private set; }
    public bool MusicEnabled { get; private set; }

    [SerializeField] private string Sound = "Sound";
    [SerializeField] private string Music = "Music";

    [SerializeField] private AudioMixer _audioMixer;

    public AudioClip SlotDown;
    public AudioClip SlotWin;
    public AudioClip Spin;
    public AudioClip Scroll;
    public AudioClip SlotPostWin;
    public AudioClip LevelUp;
    public AudioClip TubeSelected;
    public AudioClip TubeSet;

    [SerializeField] private GameObject _soundPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        SoundEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("SoundEnabled", 1));
        MusicEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("MusicEnabled", 0));

        UpdateSoundAndMusic();
    }
    public void ToggleSound()
    {
        SoundEnabled = !SoundEnabled;
        UpdateSoundAndMusic();
        Save();
    }
    public void ToggleMusic() 
    {
        MusicEnabled = !MusicEnabled;
        UpdateSoundAndMusic();
        Save();
    }
    private void UpdateSoundAndMusic()
    {
        if (SoundEnabled)
            _audioMixer.SetFloat(Sound, 0f);
        else
            _audioMixer.SetFloat(Sound, -80f);
        if(MusicEnabled)
            _audioMixer.SetFloat(Music, 0f);
        else
            _audioMixer.SetFloat(Music, -80f);
    }
    public void PlaySound(AudioClip clip, float pitch, float volume = 1f)
    {
        if(SoundEnabled)
        Instantiate(_soundPrefab).GetComponent<Sound>().PlaySound(clip, pitch, volume);
    }
    public void Save()
    {
        PlayerPrefs.SetInt("SoundEnabled", Convert.ToInt32(SoundEnabled));
        PlayerPrefs.SetInt("MusicEnabled", Convert.ToInt32(MusicEnabled));
    }
}
