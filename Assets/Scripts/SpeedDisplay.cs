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

    //TODO: Remove after testing
    private void FixedUpdate()
    {
        UpdateArrowRotation();
    }

    // Update is called once per frame
    void UpdateArrowRotation()
    {
        float percentage = _currentSpeed / _maxSpeed;

        float rotationAmount = fullRotation - (fullRotation * percentage);

        arrow.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(arrow.rotation.eulerAngles.z, rotationAmount, 0.05f));
        
    }
}
