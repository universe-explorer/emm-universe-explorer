using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject background;
    private Image imgBackground;
    [SerializeField] private GameObject infoText;
    [SerializeField] private GameObject menuButton;
    private bool active = false;

    private float lerpValue = 0f;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float minBackgroundAlpha = 0.7f;
    [SerializeField] private float backgroundAlphaFadeDuration = 5f; // In seconds
    [SerializeField] private float minTimeScale = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        imgBackground = background.GetComponent<Image>();
        imgBackground.canvasRenderer.SetAlpha(0.001f); // CrossFadeAlpha won't work when alpha is 0
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            lerpValue += fadeSpeed * Time.deltaTime;
            Time.timeScale = Mathf.Lerp(1f, minTimeScale, lerpValue); // time scale might not be 1 when death screen gets enabled -> TODO
        }

        // temporary
        if (Input.GetKeyDown(KeyCode.Space))
            enableDeathScreen();
    }

    public void enableDeathScreen()
    {
        active = true;
        background.SetActive(true);
        infoText.SetActive(true);
        menuButton.SetActive(true);

        imgBackground.CrossFadeAlpha(minBackgroundAlpha, backgroundAlphaFadeDuration, true);

        Cursor.lockState = CursorLockMode.None;
    }
}