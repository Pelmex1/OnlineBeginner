using System;
using CustomEventBus;
using ExitGames.Client.Photon;
using OnlineBeginner.Abstraction.Signals;
using OnlineBeginner.Consts;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour, IEndGame
{
    [SerializeField] private GameObject _endPanel;
    [SerializeField] TMP_Text _infoText;
    private EndingPlayerSignal endingPlayerSignal;
    private int _placeOfPlayer = 1;
    private EventBus _eventBus;
    public void Init()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        endingPlayerSignal = new();
    }
    public void OpenUI()
    {
        // запрос обычным еветн басом 
        _eventBus?.Invoke(endingPlayerSignal);
        _placeOfPlayer = endingPlayerSignal.PlaceOfPlayer;
        Debug.Log(_placeOfPlayer);
        _endPanel.SetActive(true);
        switch (_placeOfPlayer)
        {
            case 0:
                _infoText.text = "You won";
                break;
            case 2:
                _infoText.text = "You lost";
                break;
        }
        if(!PhotonNetwork.OfflineMode){
            SendOptions sendOptions = new()
            {
                Reliability = true
            };
            PhotonNetwork.RaiseEvent(StringConstants.ON_END_GAME, null, new RaiseEventOptions { Receivers = ReceiverGroup.All, CachingOption = EventCaching.AddToRoomCacheGlobal }, sendOptions);
        } else {
            
        }
        // изминения в ин Гейм сцене через фотоновский евент
    }
}
