using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui_level : MonoBehaviour
{
    private LevelSystem levelSystem;
    private TextMeshProUGUI level;

    private int currentMineralValue;
    private int currentManaValue;
    private int currentMedkitValue;
    private int currentHealthValue;

    public Slider mineralBar;
    public Slider medkitBar;
    public Slider manaBar;
    public Slider healthBar;

    private void Start()
    {
        level = transform.Find("level").GetComponent<TextMeshProUGUI>();
        level.SetText("Level: " + levelSystem.GetLevelNumber().ToString());

        currentMineralValue = levelSystem.GetMineralLevelValue();
        currentMedkitValue = levelSystem.GetMedkitLevelValue();
        currentManaValue = levelSystem.GetManaLevelValue();
        currentHealthValue = levelSystem.GetHealthLevelValue();

        mineralBar.value = currentMineralValue;
        medkitBar.value = currentMedkitValue;
        manaBar.value = currentManaValue;
        healthBar.value = currentHealthValue;
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        RefreshExperience();
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        level.SetText(levelSystem.GetLevelNumber().ToString());
    }

    private void RefreshExperience()
    {
        if (levelSystem.GetMineralLevelValue() != currentMineralValue) ResetMineralValue();
        if (levelSystem.GetMedkitLevelValue() != currentMedkitValue) ResetMedkitValue();
        if (levelSystem.GetManaLevelValue() != currentManaValue) ResetManaValue();
        if (levelSystem.GetHealthLevelValue() != currentHealthValue) ResetHealthValue();
    }

    private void ResetMineralValue()
    {
        mineralBar.value = levelSystem.GetMineralLevelValue();
    }

    private void ResetManaValue()
    {
        manaBar.value = levelSystem.GetManaLevelValue();
    }

    private void ResetMedkitValue()
    {
        medkitBar.value = levelSystem.GetMedkitLevelValue();
    }

    private void ResetHealthValue()
    {
        healthBar.value = levelSystem.GetHealthLevelValue();
    }

}
