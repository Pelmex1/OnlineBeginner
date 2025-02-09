using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using OnlineBeginner.Consts;
using OnlineBeginner.EventBus.Signals;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    public static InGameScene instance;
    private EventBus _eventBus;
    private List<IPlayerWalk> _playerWalk;
    public void Init(){
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
            yield return new WaitForSecondsRealtime(1);
            time--;
            Debug.Log(time);
        }
        foreach (var player in _playerWalk)
        {
            player.Speed = 1f;
        }
    }
}
