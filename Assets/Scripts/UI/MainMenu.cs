using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon;
using Photon.Pun;

public class MainMenu : MonoBehaviourPunCallbacks
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
        //PlayerPrefs.DeleteAll();

        int bestTime = PlayerPrefs.GetInt("BestTime", 200);
        bestTimeText.text = bestTime != 200 ? $"{bestTime}" : $"{0}";
        Time.timeScale = 1f;
        InitializeAudioSettings();
    }

    private void InitializeAudioSettings()
    {
        isSoundActive = PlayerPrefs.GetInt("isSoundOn", 0) == 1;
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
        SetMasterVolume(isSoundActive ? -20f : -80f);
    }

    private void SetMasterVolume(float volume)
    {
        masterAudioMixer.SetFloat("MainVolume", volume);
    }

    public void Play()
    {
        PhotonNetwork.OfflineMode = true;
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
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;

        SetMasterVolume(isSoundActive ? -20f : -80f);

        PlayerPrefs.SetInt("isSoundOn", isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        audioClips[0].Play();
    }

    public void ExitGame()
    {
        if (audioClips.Length > 1)
        {
            audioClips[1].Play();
        }
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
