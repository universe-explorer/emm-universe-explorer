using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{

    [SerializeField]
    private float _currentSpeed = 0f;

    [SerializeField]
    private float _maxSpeed = 150f;

    [SerializeField]
    Transform arrow;

    private const float fullRotation = 90;

    private void Start()
    {
        float percentage = _currentSpeed / _maxSpeed;
        float rotationAmount = fullRotation - (fullRotation * percentage);
        arrow.rotation = Quaternion.Euler(0, 0, rotationAmount);
    }

    private void FixedUpdate()
    {
        UpdateArrowRotation();
    }

    // Update is called once per frame
    private void UpdateArrowRotation()
    {
        float percentage = _currentSpeed / _maxSpeed;
        float rotationAmount = fullRotation - (fullRotation * percentage);

        if(rotationAmount == 0)
        {
            rotationAmount++;
        }

        arrow.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(arrow.rotation.eulerAngles.z, rotationAmount, 0.05f));
        
    }

    public void SetNewMaxSpeed(float newMaxSpeed)
    {
        _maxSpeed = newMaxSpeed;
    }

    public void SetNewCurrentSpeed(float newCurrentSpeed)
    {
        _currentSpeed = newCurrentSpeed;
    }
}
