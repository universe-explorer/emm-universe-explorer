using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu manager
/// </summary>
public class MainMenuManager : BaseMenu
{
    [SerializeField] private string gameSceneName;
    
    
    /// <summary>
    /// Load game scene
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
}
