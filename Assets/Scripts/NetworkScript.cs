using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class NetworkScript : MonoBehaviourPunCallbacks
{

    public TMP_InputField CreateCodeInput;
    public TMP_InputField JoinCodeInput;
    public TMP_InputField PlayerNameInput;
    public TMP_InputField ChatInput;
    public TMP_Text JoinCodeDisplay;
    public GameObject MainMenuCanvas;
    public GameObject GameCanvas;
    public GameObject LoadingScreen;
    public GameObject Screen0;
    public GameObject PlayerPrefab;
    public Transform StartPosition;
    public static TMP_InputField CheckChatBox;
    public static string playerName = " player ";
    public static string message = "";
    public static bool IsChatBubble = false;
    public static bool IsInRoom = false;
    string joinCode;

    // Start is called before the first frame update
    void Start()
    {
        CheckChatBox = ChatInput;
        MainMenuCanvas.SetActive(true);
        ChatInput.gameObject.SetActive(false);
        GameCanvas.SetActive(false);
        LoadingScreen.SetActive(true);
        Screen0.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        LoadingScreen.SetActive(false);
        Screen0.SetActive(true);
    }

    public void CreateRoom()
    {
        LoadingScreen.SetActive(true);
        Screen0.SetActive(false);
        joinCode = CreateCodeInput.text;
        PhotonNetwork.CreateRoom(joinCode);
    }

    public void JoinRoom()
    {
        LoadingScreen.SetActive(true);
        Screen0.SetActive(false);
        joinCode = JoinCodeInput.text;
        PhotonNetwork.JoinRoom(joinCode);
    }

    public override void OnJoinedRoom()
    {
        IsInRoom = true;
        LoadingScreen.SetActive(false);
        MainMenuCanvas.SetActive(false);
        GameCanvas.SetActive(true);
        JoinCodeDisplay.text = "Join Code: " + joinCode;
        playerName = PlayerNameInput.text;
        PhotonNetwork.Instantiate(PlayerPrefab.name, StartPosition.position, StartPosition.rotation);
    }

    public void getMessage()
    {
        message = ChatInput.text;
        IsChatBubble = true;
        ChatInput.text = "";
        ChatInput.gameObject.SetActive(false);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        GameCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        LoadingScreen.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        IsInRoom = false;
        LoadingScreen.SetActive(false);
        Screen0.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && IsInRoom)
        {
            ChatInput.gameObject.SetActive(true);
            ChatInput.Select();
            ChatInput.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.E) && !ChatInput.isFocused && IsInRoom)
        {
            LeaveRoom();
        }
    }
}