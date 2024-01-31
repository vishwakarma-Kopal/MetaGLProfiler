using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LinkOpener : MonoBehaviour
{
    public Canvas linkCanvas;
    public TMP_Text linkText;
    public string linkURL;
    public GameObject playerPrefab; // Reference to the player prefab

    private bool playerHit = false;
    private Player playerController; // Reference to the PlayerController script
    public UIInteractions uiInteractions;
    
    //private MouseHandler mouseHandler; // Reference to the MouseHandler script

    //public Vector3 initialPosition; // Store the initial position of the LinkOpener object

    private void Start()
    {
        linkCanvas.gameObject.SetActive(false); // Start with the canvas disabled.

        //initialPosition = transform.position; // Store the initial position

        // Attempt to instantiate the player prefab
        if (playerPrefab != null)
        {
            //GameObject playerObject = Instantiate(playerPrefab, initialPosition, Quaternion.identity);

            // Get references to PlayerController and MouseHandler scripts from the player GameObject
            if (playerPrefab != null)
            {
                playerController = playerPrefab.GetComponent<Player>();
                //mouseHandler = playerPrefab.GetComponent<MouseHandler>();
            }
            else
            {
                Debug.LogError("Failed to instantiate the player prefab.");
            }
        }
        else
        {
            Debug.LogError("Player prefab reference not set in the LinkOpener script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerHit)
        {
            Debug.Log("Player hit successfully");
            playerHit = true;
            linkText.text = "Click here to open the link";
            linkCanvas.gameObject.SetActive(true);
            playerController.GetComponent<CharacterController>().enabled = false;

            // Disable PlayerController and MouseHandler scripts
            //DisablePlayerScripts();

            if (playerController != null)
            {
                Debug.Log("Player movement is stopped");
                
                playerController.SetMovementEnabled(false);
            }

            if (uiInteractions != null)
            {
                uiInteractions.enabled = true;
            }

            GetComponent<UIInteractions>().enabled = true;
        }
    }

    public void OpenLink()
    {
        if (playerHit)
        {
            // Open the link when the player clicks on it.
            Application.OpenURL(linkURL);
        }
    }

    public void CloseCanvas()
    {
        playerHit = false; // Reset the playerHit flag when the canvas is closed.
        linkCanvas.gameObject.SetActive(false);

        if (playerController != null)
        {
            //playerController.GetComponent<CharacterController>().enabled = true;
            playerController.SetMovementEnabled(true);
        }

        GetComponent<UIInteractions>().enabled = false;

        // Enable PlayerController and MouseHandler scripts
        //EnablePlayerScripts();
    }

    /*private void DisablePlayerScripts()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        //if (mouseHandler != null)
        //{
          //  mouseHandler.enabled = false;
        //}
    }*/

    /*private void EnablePlayerScripts()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        //if (mouseHandler != null)
        //{
          //  mouseHandler.enabled = true;
        //}
    }*/
}
