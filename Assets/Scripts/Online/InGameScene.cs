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
        _eventBus.Subscribe<IRaiseEventSimulator>(OnEventSim);
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
            object[] data1 = new object[]
            {
                time
            };  
            if(!PhotonNetwork.OfflineMode)
            {
                PhotonNetwork.RaiseEvent(StringConstants.SEND_TIME, data1, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
            } else {
                var Eventcode = new EventData
                {
                    Code = StringConstants.SEND_TIME,
                };
                IRaiseEventSimulator raiseEvent = new(Eventcode,data1);
                _eventBus.Invoke(raiseEvent);
            }
            //_eventBus.Invoke(new IStartTimer(time));
            yield return new WaitForSecondsRealtime(1);
            time--;
        }
        object[] data = new object[]
        {
            
        }; 
        if(!PhotonNetwork.OfflineMode){
 
            RaiseEventOptions raiseEventOptions = new()
            {
                Receivers = ReceiverGroup.All,
                CachingOption = EventCaching.AddToRoomCacheGlobal
            };

            SendOptions sendOptions = new()
            {
                Reliability = true
            };
            PhotonNetwork.RaiseEvent(StringConstants.ON_MATCH_START, data, raiseEventOptions, sendOptions);
        } else {
            var Eventcode = new EventData
            {
                Code = StringConstants.ON_MATCH_START
            };
            IRaiseEventSimulator raiseEvent = new(Eventcode,data);
            _eventBus.Invoke(raiseEvent);
        }
    }

    private void Processor(int Code){
        if (Code == StringConstants.OnPhotonPlayerSpawned)
        {
            _players += 1;
            if (_players == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                StartCoroutine(StartOcklock());
            }
        }
        if(Code == StringConstants.ON_END_GAME)
        {
            _endingPLayers++;
        }
    }
    public void OnEvent(EventData photonEvent)
    {
        Processor(photonEvent.Code);
    }
    private void OnEventSim(IRaiseEventSimulator raiseEventSimulator){
        Processor(raiseEventSimulator.eventData.Code);
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
