using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MinimapBehaviour : MonoBehaviour
{

    public Transform spaceShip;
    //[SerializeField] private Shader emissionShader;
    [SerializeField] private float emissionValue = 0.005f;
    [SerializeField] private float iconSize = 20; // Not relative to planet size
    [SerializeField] private float iconScale = 1; // 1 equals original size
    private float cameraHeight = 100;
    private float referenceHeight;
    private string planetTag = "Planet";
    public Sprite playerMapMarker;
    private float playerIconScale = 7;

    void Start()
    {
        referenceHeight = spaceShip.position.y;
        AddMinimapIcons();
        createSpaceShipIcon();
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
            MeshRenderer planetMeshRenderer = planet.GetComponentInChildren<MeshRenderer>();
            Debug.Log("Material name: " + planetMeshRenderer.material.name);
            
            /*
            // temporary test texture
            Texture2D iconTexture = new Texture2D(100, 100);
            Color[] colors = new Color[10000];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.green;
            }
            iconTexture.SetPixels(0, 0, 100, 100, colors);
            iconTexture.Apply();
            */


            string minimapIconObjName = "minimapIconObj_" + planet.name;
            //GameObject minimapIconObj = new GameObject(minimapIconObjName);
            GameObject minimapIconObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            minimapIconObj.name = minimapIconObjName;
            MeshRenderer minimapIconMeshRenderer = minimapIconObj.GetComponent<MeshRenderer>();
            minimapIconMeshRenderer.material.CopyPropertiesFromMaterial(planetMeshRenderer.material);
            //minimapIconMeshRenderer.material.shader = emissionShader;
            minimapIconMeshRenderer.material.EnableKeyword("_EMISSION");
            minimapIconMeshRenderer.material.SetColor("_EmissionColor", Color.white*Mathf.LinearToGammaSpace(emissionValue));
            //minimapIconMeshRenderer.material.DisableKeyword("_ReceiveShadows");
            minimapIconMeshRenderer.material.EnableKeyword("_RECEIVE_SHADOWS_OFF");
            
            minimapIconObj.layer = 6;
            // TODO: Add shader to change brightness

            /*
            SpriteRenderer spriteRenderer = minimapIconObj.AddComponent<SpriteRenderer>();
            
            Sprite iconSprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
            //iconSprite = GenerateIconSprite(meshRenderer.material);
            spriteRenderer.sprite = iconSprite;
            */

            float radius = planet.GetComponent<CelestialBody>().Radius;
            minimapIconObj.transform.localScale = new Vector3(1, 0, 1) * (iconScale * radius);
            
            minimapIconObj.transform.SetParent(planet.transform);
            
            minimapIconObj.transform.localPosition = Vector3.zero;
            
        }
        
    }

    private void createSpaceShipIcon()
    {
        GameObject go = new GameObject("playerMapMarker");
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = playerMapMarker;

        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.position += Vector3.forward; 

        go.transform.localScale *= playerIconScale;

        go.layer = 6;
    }

    /*
    private Sprite GenerateIconSprite(Material material)
    {
        Texture2D iconTexture = (Texture2D) material.mainTexture;
        iconTexture.Resize(100, 100);
        
        return Sprite.Create(iconTexture, new Rect(0, 0, 100, 100),
            new Vector2(0.5f, 0.5f));
    }
    */
    
     
}
