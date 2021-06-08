using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUIEvents : MonoBehaviour, IPointerClickHandler
{
    public Action RightClickHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            if (RightClickHandler != null) RightClickHandler();
    }
}
