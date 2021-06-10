using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: Create abstract class to avoid redundancy
public class InfoCircleScript : MonoBehaviour
{

    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject circleText;
    private Image imgCircle;
    private TextMeshPro txtCircle;

    private float maxValue = 100;
    private float value = 100;
    private float delayedValue = 100;
    private float progress = 0;
    [SerializeField] private float speed = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        imgCircle = circle.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Reduce(10);
        }

        if (value < delayedValue)
        {
            progress += Time.deltaTime;
            imgCircle.fillAmount = Mathf.Lerp(imgCircle.fillAmount, value / maxValue, progress * speed);
        }
    }

    public void Reduce(float value)
    {
        this.value -= value;
        progress = 0;
    }

    public void Increase(float value)
    {
        // TODO
    }
}
