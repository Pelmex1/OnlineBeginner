using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TMP_Text bestTimeText;
    [Header("Audio Settings")]
    [SerializeField] private AudioSource[] audioClips;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private AudioMixer masterAudioMixer;
    private bool isSoundActive;
    private void Start()
    {
        int bestTime = PlayerPrefs.GetInt("BestTime", 200);
        if(bestTime != 200)
        {
            bestTimeText.text = $"{bestTime}";
        }
        else
        {
            bestTimeText.text = $"{0}";
        }
        Time.timeScale = 1f;
        InitializeAudioSettings();
    }
    private void InitializeAudioSettings()
    {
        isSoundActive = PlayerPrefs.GetInt("isSoundOn", 0) == 1;
        audioClips[1].Play();
        foreach (var audio in audioClips)
        {
            audio.enabled = isSoundActive;
        }
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OpenSettings()
    {
        audioClips[1].Play();
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        audioClips[1].Play();
        settingsPanel.SetActive(false);
    }

    public void ToggleSound()
    {
        isSoundActive = !isSoundActive;
        audioClips[1].Play();
        foreach (var audio in audioClips)
        {
            audio.enabled = isSoundActive;
        }
        audioClips[0].Play();
        masterAudioMixer.SetFloat("MasterVolume", isSoundActive ? 0f : -80f);
        PlayerPrefs.SetInt("isSoundOn", isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }
    public void ExitGame()
    {
        audioClips[1].Play();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
