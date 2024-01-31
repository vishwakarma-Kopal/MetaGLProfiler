using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResumeLinkOpener : MonoBehaviour
{
    public Canvas linkCanvas;
    public TMP_Text linkText;
    public string linkURL;
        
    private bool playerHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !playerHit)
        {
            Debug.Log("Player hit successfully");
            playerHit = true;
            linkText.text = "Click here to open the link";
            linkCanvas.gameObject.SetActive(true);
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
    }
}
