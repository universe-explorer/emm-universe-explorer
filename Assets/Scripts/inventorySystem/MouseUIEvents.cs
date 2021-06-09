using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUIEvents : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action RightClickHandler = null;
    public Action MouseEnterHandler = null;
    public Action MouseExitHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            if (RightClickHandler != null) RightClickHandler();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MouseEnterHandler != null) MouseEnterHandler();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MouseExitHandler != null) MouseExitHandler();
    }
}
