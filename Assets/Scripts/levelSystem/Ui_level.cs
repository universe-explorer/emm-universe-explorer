using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///   Represents Level window that gets notified each time when level is updated
/// </summary>
public class Ui_level : MonoBehaviour
{
    private LevelSystem levelSystem;
    private TextMeshProUGUI level;
    private readonly float tweenTime = 3f;

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

    /// <summary> 
    ///   Sets the associated Level System which provides Events utilities and data accessibilities
    /// </summary>
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    /// <summary>
    ///   Initialise
    /// </summary>
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
        ItemRankEntry entry = levelSystem.GetCurrentItemLevelRank();

        mineralBar.value = mineralValue;
        mineralBar.maxValue = entry.MineralRequired;
        mineralMaxValue.SetText(entry.MineralRequired.ToString());

        medkitBar.value = medkitValue;
        medkitBar.maxValue = entry.MedkitRequired;
        medkitMaxValue.SetText(entry.MedkitRequired.ToString());

        manaBar.value = manaValue;
        manaBar.maxValue = entry.ManaRequired;
        manaMaxValue.SetText(entry.ManaRequired.ToString());

        healthBar.value = healthValue;
        healthBar.maxValue = entry.HealthRequired;
        healthMaxValue.SetText(entry.HealthRequired.ToString());
    }

    /// <summary>
    ///   Refreshes Level Window on Item list changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        RefreshExperience();
    }

    /// <summary>
    ///   Refreshes Level Window on Level changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        UpdateLevelWindow();
    }

    /// <summary> 
    ///   Updates UI elements when items list is changed
    /// </summary>
    private void RefreshExperience()
    {
        var currentMineralValue = levelSystem.GetMineralLevelValue();
        if (currentMineralValue != mineralValue) UpdateMineralValue(currentMineralValue);

        var currentMedkitValue = levelSystem.GetMedkitLevelValue();
        if (currentMedkitValue != medkitValue) UpdateMedkitValue(currentMedkitValue);

        var currentManaValue = levelSystem.GetManaLevelValue();
        if (currentManaValue != manaValue) UpdateManaValue(currentManaValue);

        var currentHealthValue = levelSystem.GetHealthLevelValue();
        if (currentHealthValue != healthValue) UpdateHealthValue(currentHealthValue);
    }

    /// <summary> 
    ///   Updates UI element's value when level is changed
    /// </summary>
    private void UpdateLevelWindow()
    {
        level.SetText("Level: " + levelSystem.GetLevelNumber().ToString());
        LeanTween.scale(level.gameObject, Vector3.one * 2, tweenTime).setEasePunch();

        ItemRankEntry entry = levelSystem.GetCurrentItemLevelRank();
        ResetMineralBar(entry.MineralRequired);
        ResetManaBar(entry.ManaRequired);
        ResetMedkitBar(entry.MedkitRequired);
        ResetHealthBar(entry.HealthRequired);
    }

    /// <summary>
    ///   Resets Mineral Bar
    /// </summary>
    /// <param name="maxValue">Max Value</param>
    private void ResetMineralBar(int maxValue)
    {
        mineralBar.maxValue = maxValue;
        mineralBar.value = mineralValue > maxValue ? maxValue : mineralValue;
        mineralMaxValue.SetText(maxValue.ToString());
    }

    /// <summary>
    ///   Resets Mana Bar
    /// </summary>
    /// <param name="maxValue">maxValue</param>
    private void ResetManaBar(int maxValue)
    {
        manaBar.maxValue = maxValue;
        manaBar.value = manaValue > maxValue ? maxValue : manaValue;
        manaMaxValue.SetText(maxValue.ToString());
    }

    /// <summary>
    ///   Resets Medkit Bar
    /// </summary>
    /// <param name="maxValue">maxValue</param>
    private void ResetMedkitBar(int maxValue)
    {
        medkitBar.maxValue = maxValue;
        medkitBar.value = medkitValue > maxValue ? maxValue : medkitValue;
        medkitMaxValue.SetText(maxValue.ToString());
    }

    /// <summary>
    ///   Resets Health Bar
    /// </summary>
    /// <param name="maxValue">maxValue</param>
    private void ResetHealthBar(int maxValue)
    {
        healthBar.maxValue = maxValue;
        healthBar.value = healthValue > maxValue ? maxValue : healthValue;
        healthMaxValue.SetText(maxValue.ToString());
    }

    /// <summary>
    ///   Updates Mineral Bar's value and cache it
    /// </summary>
    /// <param name="value">maxValue</param>
    private void UpdateMineralValue(int value)
    {
        mineralBar.value = value;
        mineralValue = value;
    }

    /// <summary>
    ///   Updates Mana Bar's value and cache it
    /// </summary>
    /// <param name="value">Value</param>
    private void UpdateManaValue(int value)
    {
        manaBar.value = value;
        manaValue = value;
    }

    /// <summary>
    ///   Updates Medkit Bar's value and cache it
    /// </summary>
    /// <param name="value">Value</param>
    private void UpdateMedkitValue(int value)
    {
        medkitBar.value = value;
        medkitValue = value;
    }

    /// <summary>
    ///   Updates Health Bar's value and cache it
    /// </summary>
    /// <param name="value">Value</param>
    private void UpdateHealthValue(int value)
    {
        healthBar.value = value;
        healthValue = value;
    }

}
