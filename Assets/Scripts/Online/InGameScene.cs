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
    private int _players = 0;


    public void Init()
    {
        GetPointsOfSpawn getPointsOfSpawn = new();
        IPlayersPositionsSender playersPositionsSender = new();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(getPointsOfSpawn);
        _eventBus.Invoke(playersPositionsSender);
        PhotonNetwork.Instantiate("Player", playersPositionsSender.Positions[1], Quaternion.identity);
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
            _eventBus.Invoke(new IStartTimer(time));
            yield return new WaitForSecondsRealtime(1);
            time--;
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


    public void OnEvent(EventData photonEvent)
    {
        Debug.Log("GET PLAYER");
        Debug.Log(photonEvent.Code);
        if (photonEvent.Code == StringConstants.OnPhotonPlayerSpawned)
        {
            Debug.Log("GET PLAYER1");
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
