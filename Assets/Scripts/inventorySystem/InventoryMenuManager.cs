using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> 
/// Inventory Menu Manager, NOT used any more.
/// </summary>
public class InventoryMenuManager : MonoBehaviour
{
    public GameObject menu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            menu.gameObject.SetActive(!menu.gameObject.activeSelf);
            Time.timeScale = !menu.gameObject.activeSelf ? 1 : 0;
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Resume()
    {
        menu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
