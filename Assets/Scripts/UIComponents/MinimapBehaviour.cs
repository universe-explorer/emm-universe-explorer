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
    [SerializeField] private Shader emissionShader;
    [SerializeField] private float emissionValue = 0.005f;
    [SerializeField] private float iconScale = 1; // 1 equals original size
    [SerializeField] private float outlineSize = 16f;
    [SerializeField] private Color planetOutlineColor = new Color(90f/255, 0, 5f/255);
    [SerializeField] private float cameraHeight = 100;
    private float referenceHeight;
    private string planetTag = "Planet";
    public Sprite playerMapMarker;
    [SerializeField] private float playerIconScale = 28f;
    [SerializeField] private SettingsManager _settingsManager;


    void ReloadSettings()
    {
        Debug.Log("RELOADED!");
        float minimapView = PlayerPrefs.GetFloat("SliderMinimapView", 500f);
        GetComponent<Camera>().orthographicSize = minimapView;
        Debug.Log(cameraHeight);
    }
    void Start()
    {
        ReloadSettings();
        _settingsManager.addSettingsEventListener(ReloadSettings);
        referenceHeight = spaceShip.position.y;
        AddPlanetIcons();
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

    /// <summary> 
    ///     Returns the current Y coordinates of the camera
    /// </summary>
    public float getCameraHeight()
    {
        return cameraHeight;
    }

    /// <summary> 
    ///     Sets camera height to the specified value
    ///   <param name="newCameraHeight"> New Y coordinates of the camera</param>
    /// </summary>
    public void setCameraHeight(float newCameraHeight)
    {
        cameraHeight = newCameraHeight;
    }


    /// <summary> 
    ///     Adds minimap icons to each planet in the scene
    /// </summary>
    private void AddPlanetIcons()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag(planetTag);
        
        foreach (var planet in planets)
        {
            MeshRenderer planetMeshRenderer = planet.GetComponentInChildren<MeshRenderer>();
            
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
            minimapIconObj.transform.localScale = new Vector3(1, 0, 1) * (iconScale * radius); // TODO: Fix scale
            
            minimapIconObj.transform.SetParent(planet.transform);

            minimapIconObj.transform.localPosition = Vector3.zero;

            Transform t = planet.transform;
            Vector3 offset = new Vector3(0, 0, 0);

            for (int i = 0; i < t.childCount; i++)
            {
                if (t.GetChild(i).gameObject.tag == "PlanetSubObject")
                {
                    offset = t.GetChild(i).gameObject.transform.up * (radius * 0.63f);
                    minimapIconObj.transform.position += offset;
                }

            }

            

            addPlanetOutline(planet, offset);
        }
        
    }

    /// <summary> 
    ///     Adds minimap outline to the specified planet
    ///   <param name="planet"> Gameobject to draw the outline around</param>
    ///   <param name="offsetVector"> Offset vector in case the outline should not be drawn in the exact center</param>
    /// </summary>
    private void addPlanetOutline(GameObject planet, Vector3 offsetVector)
    {
        GameObject planetOutline = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        string outlineName = "planetOutline_" + planet.name;
        planetOutline.name = outlineName;
        planetOutline.layer = 6;

        planetOutline.transform.SetParent(planet.transform);
        planetOutline.transform.localPosition =new Vector3(0, -1, 0);
        planetOutline.transform.position += offsetVector;
        
        planetOutline.transform.localScale = new Vector3(1, 0, 1) * (planet.GetComponent<CelestialBody>().Radius + outlineSize); 
        
        MeshRenderer outlineMeshRenderer = planetOutline.GetComponent<MeshRenderer>();
        outlineMeshRenderer.material.shader = emissionShader;
        outlineMeshRenderer.material.SetColor("_Color", planetOutlineColor); // Access Color property of emission shader on material

    }

    /// <summary> 
    ///     Creates the spaceship icon for the minimap
    /// </summary>
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
         
}
