using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using ExitGames.Client.Photon;
using OnlineBeginner.Abstraction.Signals;
using OnlineBeginner.Consts;
using OnlineBeginner.EventBus.Signals;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviourPunCallbacks, IOnEventCallback
{    private EventBus _eventBus;
    private int _players = 0;
    private int _endingPLayers = 0;


    public void Init()
    {
        GetPointsOfSpawn getPointsOfSpawn = new();
        IPlayersPositionsSender playersPositionsSender = new();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(getPointsOfSpawn);
        _eventBus.Invoke(playersPositionsSender);
        _eventBus.Subscribe<EndingPlayerSignal>(ChangePosition);
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
    private void ChangePosition(EndingPlayerSignal endingPlayerSignal) => endingPlayerSignal.PlaceOfPlayer = _endingPLayers;
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
        if (photonEvent.Code == StringConstants.OnPhotonPlayerSpawned)
        {
            _players += 1;
            if (_players == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartCoroutine(StartOcklock());
            }
        }
        if(photonEvent.Code == StringConstants.ON_END_GAME)
        {
            _endingPLayers++;
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
