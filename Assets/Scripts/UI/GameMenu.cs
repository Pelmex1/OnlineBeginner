using System;
using CustomEventBus;
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
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private TMP_Text _timeText;

    private bool isSoundActive = false;
    private bool isEnd = false;
    private float time = 0;
    private EventBus eventBus;
    public void Init()
    {
        InitializeAudioSettings();
        Time.timeScale = 1f;
        eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Subscribe<bool>(wasEnd => isEnd = wasEnd);
    }
    private void Update() {
        while(isEnd == false)
        {
            time += Time.deltaTime;
            _timeText.text = $"{(int)time}";
        }
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
            for(int i = 0; i < audioSources.Length; i++){
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
        audioSources[1].Play();
        foreach (var audio in audioSources)
        {
            audio.enabled = isSoundActive;
        }
        mainAudioMixer.SetFloat("MasterVolume", isSoundActive ? VolumeOn : VolumeOff);
        PlayerPrefs.SetInt(SoundPreference, isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }
    public void OpenSettings()
    {
        audioSources[1].Play();
        exitPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        audioSources[1].Play();
        exitPanel.SetActive(false);
    }
    public void ReturnToMainMenu()
    {
        audioSources[1].Play();
        SceneManager.LoadScene("MainMenu");
    }
    public void Again()
    {
        audioSources[1].Play();
        SceneManager.LoadScene("GameScene");
    }
}
