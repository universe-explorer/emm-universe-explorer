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

    public TextMeshProUGUI mineralMaxValue;
    public TextMeshProUGUI medkitMaxValue;
    public TextMeshProUGUI manaMaxValue;
    public TextMeshProUGUI healthMaxValue;

    private void Start()
    {
        level = transform.Find("level").GetComponent<TextMeshProUGUI>();
        level.SetText("Level: " + levelSystem.GetLevelNumber().ToString());

        mineralValue = levelSystem.GetMineralLevelValue();
        medkitValue = levelSystem.GetMedkitLevelValue();
        manaValue = levelSystem.GetManaLevelValue();
        healthValue = levelSystem.GetHealthLevelValue();

        SetInitialValue();
    }

    /// <summary> 
    ///   Sets initial UI element's value
    /// </summary>
    private void SetInitialValue()
    {
        RankEntry entry = levelSystem.GetCurrentLevelRank();

        mineralBar.value = mineralValue;
        mineralBar.maxValue = entry.mineralRequired;
        mineralMaxValue.SetText(entry.mineralRequired.ToString());

        medkitBar.value = medkitValue;
        medkitBar.maxValue = entry.medkitRequired;
        medkitMaxValue.SetText(entry.medkitRequired.ToString());

        manaBar.value = manaValue;
        manaBar.maxValue = entry.manaRequired;
        manaMaxValue.SetText(entry.manaRequired.ToString());

        healthBar.value = healthValue;
        healthBar.maxValue = entry.healthRequired;
        healthMaxValue.SetText(entry.healthRequired.ToString());
    }

    /// <summary> 
    ///   Sets the associated Level System which provides Events utilities and data accessibilities
    /// </summary>
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
        UpdateLevelWindow();
    }

    /// <summary> 
    ///   Updates UI elements when items list is changed
    /// </summary>
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

    /// <summary> 
    ///   Updates UI element's value when level is changed
    /// </summary>
    private void UpdateLevelWindow()
    {
        level.SetText(levelSystem.GetLevelNumber().ToString());

        RankEntry entry = levelSystem.GetCurrentLevelRank();

        mineralBar.maxValue = entry.mineralRequired;
        mineralMaxValue.SetText(entry.mineralRequired.ToString());

        manaBar.maxValue = entry.manaRequired;
        manaMaxValue.SetText(entry.manaRequired.ToString());

        medkitBar.maxValue = entry.medkitRequired;
        medkitMaxValue.SetText(entry.medkitRequired.ToString());

        healthBar.maxValue = entry.healthRequired;
        healthMaxValue.SetText(entry.healthRequired.ToString());
    }

    private void ResetMineralValue(int value)
    {
        mineralBar.value = value;
        mineralValue = value;
    }

    private void ResetManaValue(int value)
    {
        manaBar.value = value;
        manaValue = value;
    }

    private void ResetMedkitValue(int value)
    {
        medkitBar.value = value;
        medkitValue = value;
    }

    private void ResetHealthValue(int value)
    {
        healthBar.value = value;
        healthValue = value;
    }

}
