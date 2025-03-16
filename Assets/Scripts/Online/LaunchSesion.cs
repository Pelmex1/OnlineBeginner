using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System.Collections;
using OnlineBeginner.Consts;
using UnityEngine.UI;
namespace OnlineBeginner.Multiplayer
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] TMP_Text _startText;
        [SerializeField] Button _onlineButton;
        [SerializeField] GameObject _createRoomPanel;
        [SerializeField] GameObject _loadingScene;
        [SerializeField] TMP_InputField _createRoom;
        [SerializeField] TMP_Text _infoText;
        private bool _isTwoPlayersInRoom;
        private bool _isCreatedRoom = false;
        private bool _isConnected;
        private GameObject _startTextObjext;
        private RoomOptions _roomOptions;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _startTextObjext = _startText.gameObject;
        }
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
        }
        [PunRPC]
        void TextForStart(int i) => _startText.text = $"{i}";
        public void JoinRandomRoom()
        {
            _loadingScene.SetActive(true);
            if (!_isCreatedRoom)
            {
                if (_isConnected)
                {
                    PhotonNetwork.JoinRandomRoom();
                    _isConnected = false;
                }
                else
                {
                    _isConnected = PhotonNetwork.ConnectUsingSettings();
                    PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
                }
            }
        }
        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                Wait();
            }
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to load any room");
            _createRoomPanel.SetActive(true);
            _infoText.text = "Faild To Join Game. Try to crate new";
            CreateRoom();
        }
        private void CreateRoom()
        {
            _roomOptions = new()
            {
                MaxPlayers = 2,
                IsOpen = true,
                IsVisible = true
            };
            PhotonNetwork.CreateRoom(_createRoom.text, _roomOptions);
            _isCreatedRoom = true;
            _createRoomPanel.SetActive(false);
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Seccesufuly create a room");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create room:" + message);
        }
        public override void OnConnectedToMaster()
        {
            _isConnected = true;
            _onlineButton.interactable = true;
            Debug.Log("Succefully conected to server!");
        }
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log("Player entered on room");
            _isTwoPlayersInRoom = true;
            _roomOptions.IsOpen = false;
            _roomOptions.IsVisible = false;
            StartCoroutine(Wait());
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            _isTwoPlayersInRoom = false;
            _roomOptions.IsOpen = true;
            _roomOptions.IsVisible = true;
        }
        private IEnumerator Wait()
        {
            _loadingScene.SetActive(true);
            _startTextObjext.SetActive(true);
            for (int i = 5; i > 0; i--)
            {
                if (_isTwoPlayersInRoom)
                {
                    photonView.RPC("TextForStart", RpcTarget.All, i);
                    //_startText.text = $"{i}";
                    yield return new WaitForSecondsRealtime(1);
                    if (_isTwoPlayersInRoom && i == 1)
                    {
                        PhotonNetwork.LoadLevel("GameScene");
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            _isConnected = false;
        }
    }
}
