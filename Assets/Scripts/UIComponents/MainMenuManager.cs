using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : BaseMenu
{


    [SerializeField] private string gameSceneName;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }


    

    

    /*
     // TODO: Not needed anymore
     
    public override void OpenSettings(bool active)
    {
        Debug.Log("View settings");
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }
    */
    
}
