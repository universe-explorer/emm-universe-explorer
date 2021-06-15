using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUIEvents : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action RightClickHandler = null;
    public Action MouseEnterHandler = null;
    public Action MouseExitHandler = null;

    /// <summary> 
    ///   Detects Mouse Click Events
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            if (RightClickHandler != null) RightClickHandler();
    }

    /// <summary> 
    ///   Detects Mouse Enter Events
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MouseEnterHandler != null) MouseEnterHandler();
    }

    /// <summary> 
    ///   Detects Mouse Exit Events
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (MouseExitHandler != null) MouseExitHandler();
    }
}
