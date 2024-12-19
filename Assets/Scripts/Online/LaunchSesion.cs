using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Runtime.InteropServices;
namespace OnlineBeginner.Multiplayer
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject _createRoomPanel;
        [SerializeField] GameObject _loadingScene;
        [SerializeField] TMP_InputField _createRoom;
        [SerializeField] TMP_Text _infoText;
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
        }
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnJoinedRoom()
        {
            StartCoroutine(Wait());
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            _createRoomPanel.SetActive(true);
            _infoText.text = "Faild To Join Game. Try to crate new";
        }
        public override void OnCreatedRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(_createRoom.text,roomOptions);
            Debug.Log("Seccesufuly crteate a room");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create room:" + message);
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Succefully conected to server!");
        }
        private IEnumerator Wait()
        {
            _loadingScene.SetActive(true);
            yield return new WaitForSecondsRealtime(5);    
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
