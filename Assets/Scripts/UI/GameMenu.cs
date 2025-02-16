using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class GameMenu : MonoBehaviourPunCallbacks, ITimeEnd
{
    private const float VolumeOn = -20f;
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
        if (isEnd == false)
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
            mainAudioMixer.SetFloat("MainVolume", VolumeOn);
            audioSources.Play();
        }
        else
        {
            mainAudioMixer.SetFloat("MainVolume", VolumeOff);
        }
    }
    public void ToggleSound()
    {
        PlaySoundForSingleClient();
        isSoundActive = !isSoundActive;
        mainAudioMixer.SetFloat("MainVolume", isSoundActive ? VolumeOn : VolumeOff);
        PlayerPrefs.SetInt("isSoundOn", isSoundActive ? 1 : 0);
        PlayerPrefs.Save();
        soundToggleButton.image.sprite = isSoundActive ? soundOnSprite : soundOffSprite;
    }
    public void ReturnToMainMenu()
    {
        PlaySoundForSingleClient();
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            Debug.LogError("Photon client is not connected or not ready.");
        }
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Successfully left the room.");
        SceneManager.LoadScene("MainMenu");
    }
    private void PlaySoundForSingleClient()
    {
        //audioSources.Play();
        int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("PlayBttSound", PhotonNetwork.CurrentRoom.GetPlayer(playerId));
    }
}
