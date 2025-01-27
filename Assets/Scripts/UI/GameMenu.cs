using System;
using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    private const string SoundPreference = "isSoundOn";
    private const float VolumeOn = 0f;
    private const float VolumeOff = -80f;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] TMP_Text _bestTimeText;
    [SerializeField] TMP_Text _yourTimeText;

    private bool isSoundActive = false;
    private bool isEnd = false;
    private float time = 0;
    private EventBus eventBus;
    public void Init()
    {
        InitializeAudioSettings();
        Time.timeScale = 1f;
        eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Subscribe<TimeSignal>(SetTime);
    }
    private void Update()
    {
        if (isEnd == false)
        {
            time += Time.deltaTime;
            _timeText.text = $"{(int)time}";
        }
    }
    private void SetTime(TimeSignal signal)
    {
        isEnd = signal.wasEnd;
        int Itime = (int)time;
        if (time <= PlayerPrefs.GetInt("BestTime", 200))
            PlayerPrefs.SetInt("BestTime", Itime);
        _bestTimeText.text = $"{PlayerPrefs.GetInt("BestTime", 200)}";
        _yourTimeText.text = $"{Itime}";
        PlayerPrefs.Save();
    }
    public void OnMessageReceived(string message)
    {
        Console.WriteLine($"Received message: {message}");
    }

    private void InitializeAudioSettings()
    {
        int soundStatus = PlayerPrefs.GetInt(SoundPreference, 1);
        if (soundStatus == 1)
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOn);
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].Play();
            }
        }
        else
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOff);
        }
    }
    public void ToggleSound()
    {
        isSoundActive = !isSoundActive;
        foreach (var audio in audioSources)
        {
            audio.enabled = isSoundActive;
        }
        mainAudioMixer.SetFloat("MasterVolume", isSoundActive ? VolumeOn : VolumeOff);
        PlayerPrefs.SetInt(SoundPreference, isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
