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

public class InGameScene : MonoBehaviourPunCallbacks
{
    private const byte CustomManualInstantiationEventCode = 1;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _timeObject;
    [SerializeField] private GameObject _startTextObject;
    public static InGameScene instance;
    private EventBus _eventBus;
    private List<IPlayerWalk> _playerWalk;
    private TMP_Text _timer;
    private IStartGame _startGame;
    private Queue<Vector3> positions;

    public void Init(){
        _timer = _timeObject.GetComponent<TMP_Text>();
        _startGame = _startTextObject.GetComponent<IStartGame>();
        _playerWalk = new List<IPlayerWalk>();
        GetPointsOfSpawn getPointsOfSpawn = new();
        IPlayersPositionsSender playersPositionsSender = new();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(getPointsOfSpawn);
        _eventBus.Invoke(playersPositionsSender);
        for (int i = 0; i < playersPositionsSender.Positions.Length; i++)
        {
            positions.Enqueue(playersPositionsSender.Positions[i]);
        }
        Debug.Log("Player Created");
        StartCoroutine(StartOcklock());
        PhotonNetwork.Instantiate("Player",positions.Peek(),Quaternion.identity);
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
                _startGame.StartAnimation();
            }
            Debug.Log(time);
        }
        foreach (var player in _playerWalk)
        {
            player.Speed = 1f;
        }
    }
   /* public void SpawnPlayer(Vector3 position)
{
    GameObject player = Instantiate(_playerPrefab, position, Quaternion.identity); 
    PhotonView photonView = player.GetComponent<PhotonView>();

    if (PhotonNetwork.AllocateViewID(photonView))
    {
        object[] data = new object[]
        {
            player.transform.position, player.transform.rotation, photonView.ViewID
        };

        RaiseEventOptions raiseEventOptions = new()
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new()
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, data, raiseEventOptions, sendOptions);
        _playerWalk.Add(player.GetComponent<IPlayerWalk>());
    }
    else
    {
        Debug.LogError("Failed to allocate a ViewId.");

        Destroy(player);
    }
}
public void OnEvent(EventData photonEvent)
{
    if (photonEvent.Code == CustomManualInstantiationEventCode)
    {
        object[] data = (object[]) photonEvent.CustomData;

        GameObject player = Instantiate(_playerPrefab, (Vector3) data[0], (Quaternion) data[1]);
        PhotonView photonView = player.GetComponent<PhotonView>();
        photonView.ViewID = (int) data[2];

    }
}*/
public void OnPhotonInstantiate(PhotonMessageInfo info)
{
    _playerWalk.Add(info.photonView.gameObject.GetComponent<IPlayerWalk>());
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
