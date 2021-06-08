using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
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

    public void ViewSettings()
    {
        Debug.Log("View settings");
    }

    public void Exit()
    {
        Debug.Log("Exit");
    }
}
