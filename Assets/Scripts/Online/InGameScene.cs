using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using ExitGames.Client.Photon;
using OnlineBeginner.Consts;
using OnlineBeginner.EventBus.Signals;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] private GameObject _timeObject;
    [SerializeField] private GameObject _startTextObject;
    private EventBus _eventBus;
    private List<IPlayerWalk> _playerWalk;
    private TMP_Text _timer;
    private IStartGame _startGame;
    private int _players = 0;
    private PhotonView _photonView;


    public void Init()
    {
        _timer = _timeObject.GetComponent<TMP_Text>();
        _startGame = _startTextObject.GetComponent<IStartGame>();
        _playerWalk = new List<IPlayerWalk>();
        GetPointsOfSpawn getPointsOfSpawn = new();
        IPlayersPositionsSender playersPositionsSender = new();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(getPointsOfSpawn);
        _eventBus.Invoke(playersPositionsSender);
        PhotonNetwork.Instantiate("Player", playersPositionsSender.Positions[1], Quaternion.identity);
        _photonView = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PhotonView>();
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(StringConstants.NAME_MAIN_MENU);
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player Entered scene");

    }
    private IEnumerator StartOcklock()
    {
        int time = 5;
        while (time > 0)
        {
            _photonView.RPC("AccountingTime", RpcTarget.All, time);
            yield return new WaitForSecondsRealtime(1);
            time--;
            if (time == 0)
            {
                _timeObject.SetActive(false);
                _startTextObject.SetActive(true);
                _startGame.StartAnimation();
            }
            Debug.Log(time);
        }
        object[] data = new object[]
        {

        };

        RaiseEventOptions raiseEventOptions = new()
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCacheGlobal
        };

        SendOptions sendOptions = new()
        {
            Reliability = true
        };
        Debug.Log("start?");
        PhotonNetwork.RaiseEvent(StringConstants.ON_MATCH_START, data, raiseEventOptions, sendOptions);
    }

    [PunRPC]
    public void AccountingTime(int time)
    {
        _timer.text = $"{time}";
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("GET PLAYER");
        if (photonEvent.Code == StringConstants.OnPhotonPlayerSpawned)
        {
            _players += 1;
            if (_players == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartCoroutine(StartOcklock());
            }
        }
    }


    private void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
