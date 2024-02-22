using UnityEngine;
using UnityEngine.UI;

public class UIInteractions : MonoBehaviour
{
    public Button yourButton; // Reference to your button

    private void Start()
    {
        yourButton.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        // Put your button click logic here
        Debug.Log("Button Clicked!");
    }
}
