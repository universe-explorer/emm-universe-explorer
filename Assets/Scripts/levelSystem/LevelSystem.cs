using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This level System should be encapsulated into game Object that implements Monobehaviour.
 * 
 * To enable the game object reacting to experience changes, subscribe to the OnExperienceChanged event.
 *      
 * To enable the game object reacting to level changes, subscribe to the OnLevelChanged event.
 *      
 */
public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    
    private int level;
    private int experience;
    private int experienceToNextLevel;

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        experienceToNextLevel = 100;
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        while (experience >= experienceToNextLevel)
        {
            level++;
            experience -= experienceToNextLevel;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

    public int GetLevelNumber()
    {
        return level;
    }
}
