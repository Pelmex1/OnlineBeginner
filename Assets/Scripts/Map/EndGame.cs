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
    ParticleSystem[] _fireworks = new ParticleSystem[2];
    private EndingPlayerSignal endingPlayerSignal;
    private int _placeOfPlayer = 1;
    private EventBus _eventBus;
    void Awake()
    {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }
    public void Init(ParticleSystem[] fireworks)
    {
        _fireworks = fireworks;
        endingPlayerSignal = new();
    }
    public void OpenUI()
    {
        // запрос обычным еветн басом 
        _eventBus?.Invoke(endingPlayerSignal);
        _placeOfPlayer = endingPlayerSignal.PlaceOfPlayer;
        _endPanel.SetActive(true);
        _fireworks[0].Play();
        _fireworks[1].Play();
        switch (_placeOfPlayer)
        {
            case 1:
                _infoText.text = "You won";
                break;
            case 2:
                _infoText.text = "You lost";
                break;
        }
        SendOptions sendOptions = new()
        {
            Reliability = true
        };
        PhotonNetwork.RaiseEvent(StringConstants.ON_END_GAME, null, new RaiseEventOptions { Receivers = ReceiverGroup.All,CachingOption = EventCaching.AddToRoomCacheGlobal }, sendOptions);
        // изминения в ин Гейм сцене через фотоновский евент
    }
}
