using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapBehaviour : MonoBehaviour
{

    public Transform spaceShip;
    private float cameraHeight = 100;
    private float referenceHeight;
    private string planetTag = "Planet";

    void Start()
    {
        referenceHeight = spaceShip.position.y;
        AddMinimapIcons();
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 newPosition = spaceShip.transform.position;
        newPosition.y = referenceHeight + cameraHeight;
        transform.position = newPosition;

        Quaternion newRotation = Quaternion.Euler(90, spaceShip.rotation.eulerAngles.y, spaceShip.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 0.5f);
	}

    public float getCameraHeight()
    {
        return cameraHeight;
    }


    public void setCameraHeight(float newCameraHeight)
    {
        cameraHeight = newCameraHeight;
    }

    private void AddMinimapIcons()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag(planetTag);
        Debug.Log("Amount of planets: " + planets.Length);
        
        foreach (var planet in planets)
        {
            MeshRenderer meshRenderer = planet.GetComponentInChildren<MeshRenderer>();
            Debug.Log("Material name: " + meshRenderer.material.name);
            
            
            // temporary test texture
            Texture2D iconTexture = new Texture2D(100, 100);
            Color[] colors = new Color[10000];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.green;
            }
            iconTexture.SetPixels(0, 0, 100, 100, colors);
            iconTexture.Apply();



            string minimapIconObjName = "minimapIconObj_" + planet.name;
            GameObject minimapIconObj = new GameObject(minimapIconObjName);
            // TODO: Set minimapIconObjName layer to minimap
            
            SpriteRenderer spriteRenderer = minimapIconObj.AddComponent<SpriteRenderer>();
            
            Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = iconSprite;
            
            minimapIconObj.transform.SetParent(planet.transform);
            minimapIconObj.transform.localPosition = Vector3.zero; // TODO: Center icon. Zero vector is not the center of the planet model.
            
        }
        
    }
    
     
}
