using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Rename menu classes
public class PauseMenuManager : BaseMenu
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private KeyCode pauseMenuKey = KeyCode.Escape;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseMenuKey) && RegisterInput)
            OpenMenu(!pauseMenu.activeSelf);
        
    }

    public void OpenMenu(bool active)
    {
        pauseMenu.SetActive(active);
        Time.timeScale = active ? 0 : 1;
    }
    
    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    /*
     // TODO: Not needed anymore
    public void OpenSettings(bool active)
    {
        // TODO
        Debug.Log("Settings");
    }
    
    public void Exit()
    {
        // TODO
        Debug.Log("Exit");
    }
    */
    
}
