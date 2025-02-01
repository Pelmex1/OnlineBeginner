using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class GameMenu : MonoBehaviour, ITimeEnd
{
    private const float VolumeOn = 0f;
    private const float VolumeOff = -80f;
    [SerializeField] private AudioSource audioSources;
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

    public void Awake()
    {
        InitializeAudioSettings();
        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (isEnd)
        {
            time += Time.deltaTime;
            _timeText.text = $"{(int)time}";
        }
    }
    [PunRPC]
    private void PlayBttSound()
    {
        audioSources.Play();
    }
    public void SetTime()
    {
        isEnd = true;
        int Itime = (int)time;
        if (time <= PlayerPrefs.GetInt("BestTime", 200))
            PlayerPrefs.SetInt("BestTime", Itime);
        _bestTimeText.text = $"{PlayerPrefs.GetInt("BestTime", 200)}";
        _yourTimeText.text = $"{Itime}";
        PlayerPrefs.Save();
    }

    private void InitializeAudioSettings()
    {
        int soundStatus = PlayerPrefs.GetInt("isSoundOn", 1);
        if (soundStatus == 1)
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOn);
            audioSources.Play();
        }
        else
        {
            mainAudioMixer.SetFloat("MasterVolume", VolumeOff);
        }
    }
    public void ToggleSound()
    {
        PlaySoundForSingleClient();
        isSoundActive = !isSoundActive;
        audioSources.enabled = isSoundActive;
        mainAudioMixer.SetFloat("MasterVolume", isSoundActive ? VolumeOn : VolumeOff);
        PlayerPrefs.SetInt("isSoundOn", isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }
    public void ReturnToMainMenu()
    {
        PlaySoundForSingleClient();
        PhotonNetwork.LoadLevel("MainMenu");
    }
    private void PlaySoundForSingleClient()
    {
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("PlayBttSound", PhotonNetwork.CurrentRoom.GetPlayer(playerId));
    }
}
