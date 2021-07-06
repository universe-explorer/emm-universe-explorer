using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause menu manager
/// </summary>
public class PauseMenuManager : BaseMenu
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode pauseMenuKey = KeyCode.Escape;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseMenuKey) && RegisterInput)
            OpenMenu(!pauseMenu.activeSelf);
        
    }

    /// <summary>
    /// Opens/Closes pause menu and sets time scale accordingly
    /// </summary>
    /// <param name="active">Activate/Deactivate menu</param>
    public void OpenMenu(bool active)
    {
        pauseMenu.SetActive(active);
        Time.timeScale = active ? 0 : 1;
    }
    
    /// <summary>
    /// Sets time scale to 1 and reloads current scene
    /// </summary>
    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    
}
