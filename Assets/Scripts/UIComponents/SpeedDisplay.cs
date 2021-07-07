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

        arrow.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(arrow.rotation.eulerAngles.z, rotationAmount, 0.05f));

        if(arrow.rotation.eulerAngles.z > 180)
        {
            arrow.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    /// <summary> 
    ///     Sets new maximum speed this component compares the current speed against
    /// </summary>
    /// <param name="newMaxSpeed"> New maximum speed value</param>
    public void SetNewMaxSpeed(float newMaxSpeed)
    {
        _maxSpeed = newMaxSpeed;
    }

    /// <summary> 
    ///     Sets new current speed value this component compares the maximum speed against
    /// </summary>
    /// <param name="newCurrentSpeed"> New current speed value</param>
    public void SetNewCurrentSpeed(float newCurrentSpeed)
    {
        _currentSpeed = newCurrentSpeed;
    }
}
