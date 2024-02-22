using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    private static bool linkOpened = false;
    private static float resetDelay = 1f; // Adjust this delay as needed
    public string LinkOpen;

    private void OnTriggerEnter(Collider other)
    {
        if (!linkOpened && other.CompareTag("Player"))
        {
            Debug.Log("Player hit successfully");

            // Open the link when the player hits the collider
            OpenLink();

            // Start coroutine to reset the linkOpened flag after a delay
            StartCoroutine(ResetLinkOpened());
        }
    }

    private void OpenLink()
    {
        if (!linkOpened)
        {
            // Open the link when the player hits the collider
            Application.OpenURL(LinkOpen); // Replace with your actual link
            linkOpened = true;
        }
    }

    private System.Collections.IEnumerator ResetLinkOpened()
    {
        yield return new WaitForSeconds(resetDelay);

        // Reset the linkOpened flag after the delay
        linkOpened = false;
    }
}