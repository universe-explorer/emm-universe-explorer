using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseUIEvents : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
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
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            if (MouseEnterHandler != null) MouseEnterHandler();
        });
    }

    /// <summary> 
    ///   Detects Mouse Exit Events
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        if (MouseExitHandler != null) MouseExitHandler();
    }
}
