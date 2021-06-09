using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private List<TabGroupButton> tabButtons;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;

    private TabGroupButton selectedTab;

    public List<GameObject> objectsToSwap;

    public void subscribe(TabGroupButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabGroupButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabGroupButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void OnTabExit(TabGroupButton button)
    {
        ResetTabs();
        button.background.sprite = tabIdle;
    }

    public void OnTabSelected(TabGroupButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
    }

    public void ResetTabs()
    {
        foreach(TabGroupButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.sprite = tabIdle;
        }
    }
}
