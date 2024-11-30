using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverText; // Reference to the Text GameObject

    void Start()
    {
        // Ensure the hover text is hidden at the start
        if (hoverText != null)
        {
            hoverText.SetActive(false);
        }
    }

    // When the pointer enters the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.SetActive(true); // Show the text
        }
    }

    // When the pointer exits the button
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.SetActive(false); // Hide the text
        }
    }
}
