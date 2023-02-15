using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Photon_Manager : MonoBehaviourPunCallbacks
{
    public static Photon_Manager _PHOTON_MANAGER;

    private void Awake()
    {
        if(_PHOTON_MANAGER != null && _PHOTON_MANAGER != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _PHOTON_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);
            //Realizamos conexion
            PhotonConnect();
        }
    }
    public void PhotonConnect()
    {
        //Sincronizo la carga de la sala para todos los jugadores
        PhotonNetwork.AutomaticallySyncScene = true;

        //Conexion al servidor con la config. establecida
        PhotonNetwork.ConnectUsingSettings();
      
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexion realizada");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("He implosionado porque: " + cause);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Accedido al Lobby");
    }

    public void CreateRoom(string nameRoom)
    {
        PhotonNetwork.CreateRoom(nameRoom,new RoomOptions {MaxPlayers = 2});
    }

    public void JoinRoom(string nameRoom)
    {
        PhotonNetwork.JoinRoom(nameRoom);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Me he unido a la sala: " + PhotonNetwork.CurrentRoom.Name + "con" + PhotonNetwork.CurrentRoom.PlayerCount + "Jugadores");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No he podido conectar a la sala dado el error: " + returnCode + "Que singnifica" + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("ingame");
        }
    }
}
