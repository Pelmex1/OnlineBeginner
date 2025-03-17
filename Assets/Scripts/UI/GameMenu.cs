using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using ExitGames.Client.Photon;
using OnlineBeginner.Consts;
using System.Collections;
using Photon.Realtime;

public class GameMenu : MonoBehaviourPunCallbacks, ITimeEnd, IOnEventCallback
{
    private const float VolumeOn = -20f;
    private const float VolumeOff = -80f;
    private const float _startScale = 0.1f;
    private const float _endScale = 1f;
    private const float _duration = 1f;
    [SerializeField] private AudioSource audioSources;
    [SerializeField] private AudioMixer mainAudioMixer;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] TMP_Text _bestTimeText;
    [SerializeField] TMP_Text _yourTimeText;
    [SerializeField] private TMP_Text _startTimer;
    [SerializeField] private GameObject _timeObject;
    [SerializeField] private GameObject _startTextObject;
    [SerializeField] private Transform _startTextTransform;
    private EventBus _eventBus;
    private bool isSoundActive = false;
    private bool isEnd = false;
    private float time = 0;

    public void Awake()
    {
        InitializeAudioSettings();
        Time.timeScale = 1f;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<IStartTimer>(AccountingTime);
    }
    private void Update()
    {
        if (isEnd == false)
        {
            time += Time.deltaTime;
            _timeText.text = $"{(int)time}";
        }
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
    private IEnumerator StartTextAnimation()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.one * _startScale;
        Vector3 finalScale = Vector3.one * _endScale;

        while (elapsedTime < _duration)
        {
            _startTextTransform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / _duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _startTextTransform.localScale = finalScale;
        gameObject.SetActive(false);
    }
    public void AccountingTime(IStartTimer value)
    {
        _startTimer.text = $"{value.time}";
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == StringConstants.ON_MATCH_START)
        {
            if (time == 0)
            {
                _timeObject.SetActive(false);
                _startTextObject.SetActive(true);
                StartCoroutine(StartTextAnimation());
            }
        }
    }
    void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }
    void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
