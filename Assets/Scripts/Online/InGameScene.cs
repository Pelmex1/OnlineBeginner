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
    private Transform[] Toches_of_spawn;
    private EventBus _eventBus;
    private void Init(){
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(new GetPointsOfSpawn(ref Toches_of_spawn));
    } 
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(StringConstants.NAME_MAIN_MENU);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient){
            Instantiate(_playerPrefab, Toches_of_spawn[0].position, Quaternion.identity);
        } else {
            Instantiate(_playerPrefab, Toches_of_spawn[1].position, Quaternion.identity);
        }
    }
    private IEnumerator StartOcklock(){
        yield return new WaitForSecondsRealtime(1);
        //минус счет 1 сек
        //событие начинается
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        
    }
}
