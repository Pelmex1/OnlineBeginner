using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    private const string SoundPreference = "isSoundOn";
    private const float VolumeOn = 0f;
    private const float VolumeOff = -80f;
    [SerializeField] private GameObject panelOfButtons;
    [SerializeField] private Image[] allImages = new Image[3];
    [SerializeField] private TMP_Text[] _textOfButtons = new TMP_Text[2];
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private GameObject gameDisplayPanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private TMP_Text _moneyText;

    private bool isSoundActive = false;
    private Image[] _buttonsImage = new Image[16];
    private TMP_Text[] _buttonsText = new TMP_Text[16];

    public float totalTime = 30f;

    private void Start()
    {
        InitializeAudioSettings();
        Time.timeScale = 1f;
        _buttonsImage = panelOfButtons.GetComponentsInChildren<Image>();
        _buttonsText = panelOfButtons.GetComponentsInChildren<TMP_Text>();
    }
    private void InitializeAudioSettings()
    {
        int soundStatus = PlayerPrefs.GetInt(SoundPreference, 1);
        if (soundStatus == 1)
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOn);
            audioSources[0].Play();
            audioSources[1].Play();
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
        ChangeColor(true);
    }

    public void CloseSettings()
    {
        audioSources[1].Play();
        exitPanel.SetActive(false);
        ChangeColor(false);
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
    public void ShowExitPanel()
    {
        Time.timeScale = 0f;
        audioSources[1].Play();
        gameDisplayPanel.SetActive(false);
        exitPanel.SetActive(true);
    }

    public void HideExitPanel()
    {
        Time.timeScale = 1f;
        audioSources[1].Play();
        exitPanel.SetActive(false);
        if (!endGamePanel.activeSelf)
            gameDisplayPanel.SetActive(true);
    }
    private void ChangeColor(bool Condition)
    {
        if (Condition == true)
        {
            for (int i = 0; i < _buttonsImage.Length; i++)
            {
                Color color = Color.gray;
                _buttonsImage[i].color = color;
                if (i < _buttonsImage.Length - 1)
                    _buttonsText[i].color = color;
                if (i < allImages.Length)
                    allImages[i].color = color;
                if (i < _textOfButtons.Length)
                    _textOfButtons[i].color = color;
            }
        }
        else
        {
            for (int i = 0; i < _buttonsImage.Length; i++)
            {
                Color color = Color.white;
                _buttonsImage[i].color = color;
                if (i < _buttonsImage.Length - 1)
                    _buttonsText[i].color = color;
                if (i < allImages.Length)
                    allImages[i].color = color;
                if (i < _textOfButtons.Length)
                    _textOfButtons[i].color = color;
            }
        }
    }
}
