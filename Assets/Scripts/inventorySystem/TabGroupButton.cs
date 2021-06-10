using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Represents a single Button with TabGroup, not used any more.
 */
[RequireComponent(typeof(Image))]
public class TabGroupButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabGroup tabGroup;

    public Image background;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.subscribe(this);
    }
}
