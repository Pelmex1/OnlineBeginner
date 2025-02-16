using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using OnlineBeginner.Consts;
using OnlineBeginner.EventBus.Signals;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _timeObject;
    [SerializeField] private GameObject _startTextObject;
    public static InGameScene instance;
    private EventBus _eventBus;
    private List<IPlayerWalk> _playerWalk;
    private TMP_Text _timer;
    IStartGame startGame;

    public void Init(){
        _timer = _timeObject.GetComponent<TMP_Text>();
        startGame = _startTextObject.GetComponent<IStartGame>();
        _playerWalk = new List<IPlayerWalk>();
        GetPointsOfSpawn getPointsOfSpawn = new();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(getPointsOfSpawn);
        _playerWalk.Add(PhotonNetwork.Instantiate(_playerPrefab.name, getPointsOfSpawn.Points.position, Quaternion.identity, 0).GetComponent<IPlayerWalk>());
        Debug.Log("Player Created");
        StartCoroutine(StartOcklock());
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
    private IEnumerator StartOcklock(){
        int time = 5;
        while(time > 0){
            _timer.text = $"{time}";
            yield return new WaitForSecondsRealtime(1);
            time--;
            if(time == 0){
                _timeObject.SetActive(false);
                _startTextObject.SetActive(true);
                startGame.StartAnimation();
            }
            Debug.Log(time);
        }
        foreach (var player in _playerWalk)
        {
            player.Speed = 1f;
        }
    }
}
