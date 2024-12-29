using System.Collections;
using System.Collections.Generic;
using OnlineBeginner.Consts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    public static InGameScene instance;
    public Transform[] Toches_of_spawn;
    private void Awake()
    {
            
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
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
