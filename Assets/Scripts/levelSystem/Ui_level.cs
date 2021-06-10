using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui_level : MonoBehaviour
{
    private LevelSystem levelSystem;
    private TextMeshProUGUI level;

    private int mineralValue;
    private int manaValue;
    private int medkitValue;
    private int healthValue;

    public Slider mineralBar;
    public Slider medkitBar;
    public Slider manaBar;
    public Slider healthBar;

    private void Start()
    {
        level = transform.Find("level").GetComponent<TextMeshProUGUI>();
        level.SetText("Level: " + levelSystem.GetLevelNumber().ToString());

        mineralValue = levelSystem.GetMineralLevelValue();
        medkitValue = levelSystem.GetMedkitLevelValue();
        manaValue = levelSystem.GetManaLevelValue();
        healthValue = levelSystem.GetHealthLevelValue();

        mineralBar.value = mineralValue;
        medkitBar.value = medkitValue;
        manaBar.value = manaValue;
        healthBar.value = healthValue;
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
        var currentMineralValue = levelSystem.GetManaLevelValue();
        if (currentMineralValue != mineralValue) ResetMineralValue(currentMineralValue);

        var currentMedkitValue = levelSystem.GetMedkitLevelValue();
        if (currentMedkitValue != medkitValue) ResetMedkitValue(currentMedkitValue);

        var currentManaValue = levelSystem.GetManaLevelValue();
        if (currentManaValue != manaValue) ResetManaValue(currentManaValue);

        var currentHealthValue = levelSystem.GetHealthLevelValue();
        if (currentHealthValue != healthValue) ResetHealthValue(currentHealthValue);
    }

    private void ResetMineralValue(int value)
    {
        mineralBar.value = value;
    }

    private void ResetManaValue(int value)
    {
        manaBar.value = value;
    }

    private void ResetMedkitValue(int value)
    {
        medkitBar.value = value;
    }

    private void ResetHealthValue(int value)
    {
        healthBar.value = value;
    }

}
