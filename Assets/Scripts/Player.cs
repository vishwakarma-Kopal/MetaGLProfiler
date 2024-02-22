using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPunObservable
{
    CharacterController characterController;
    Transform Cam;
    public Animator animator;
    bool chatBubbleSync;
    public float timerDuration = 5;
    public float timerCounter;
    PhotonView view;
    public TMP_Text PlayerNameDisplay;
    public GameObject ChatBubble;
    public TMP_Text messageDisplay;

    float x;
    float z;
    public float speed = 3;
    Vector3 move;

    private bool isMovementEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        timerCounter = timerDuration;
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            PlayerNameDisplay.gameObject.SetActive(false);  // if you want to enable the player name PlayerNameDisplay.text=NetworkScript.playerName; 
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cam = Camera.main.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine)
        {
            PlayerNameDisplay.transform.parent.LookAt(Camera.main.transform);
            return;
        }
        if (NetworkScript.CheckChatBox.isFocused)
            return;

        if (isMovementEnabled)
        {
            Movement();
            CameraSync();
            Animations();
        }

        if (!isMovementEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        //Movement();
        //CameraSync();
        //Animations();
        chatBubbleFunction();
    }

    public void SetMovementEnabled(bool isEnabled)
    {
        isMovementEnabled = isEnabled;
        Debug.Log("Movement Enabled: " + isEnabled);
    }

    public void Movement()
    {
        if (!isMovementEnabled)
        {
            Debug.Log("Movement is disabled.");
            return;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        move = Cam.forward * z + Cam.right * x;
        characterController.Move(move * speed * Time.deltaTime);
        characterController.Move(Vector3.down * 5 * Time.deltaTime);
    }

    void CameraSync()
    {
        Cam.parent.position = transform.position;
        transform.rotation = Quaternion.AngleAxis(Cam.parent.rotation.eulerAngles.y, Vector3.up);
    }

    void Animations()
    {
        if (x != 0 || z != 0)
            animator.SetBool("Motion", true);
        else
            animator.SetBool("Motion", false);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            animator.SetBool("Hi", true);

    }

    void chatBubbleFunction()
    {
        messageDisplay.transform.localRotation = Quaternion.Euler(0, 180, 0);
        if (ChatBubble.activeInHierarchy)
            messageDisplay.text = NetworkScript.message;
        if (NetworkScript.IsChatBubble)
        {
            if (timerCounter <= 0)
            {
                NetworkScript.IsChatBubble = false;
                timerCounter = timerDuration;
            }
            else
            {
                timerCounter = timerCounter - Time.deltaTime;
            }
        }
        chatBubbleSync = NetworkScript.IsChatBubble;
        ChatBubble.SetActive(chatBubbleSync);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(NetworkScript.playerName);
            stream.SendNext(NetworkScript.message);
            stream.SendNext(chatBubbleSync);
        }

        else if (stream.IsReading)
        {
            PlayerNameDisplay.text = (string)stream.ReceiveNext();
            messageDisplay.text = (string)stream.ReceiveNext();
            ChatBubble.SetActive((bool)stream.ReceiveNext());
        }
    }
}
