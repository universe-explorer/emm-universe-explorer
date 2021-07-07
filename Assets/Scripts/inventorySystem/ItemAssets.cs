using UnityEngine;

/// <summary>
///   Helper class to easily assign Item's Sprite Attribute
/// </summary>
public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;

    public Sprite healthPotionSprite;
    public Sprite manaPotionSprite;
    public Sprite mineralSprite;
    public Sprite medkitSprite;
}
