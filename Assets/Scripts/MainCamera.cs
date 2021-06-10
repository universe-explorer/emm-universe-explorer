using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform spaceShip;
    public float offsetX = 10.0f;
    public float offsetZ = 2.5f;
    public float rotationOffsetX = -8f;

    void Update()
    {
        var cam = transform;

        cam.position = spaceShip.transform.position - spaceShip.transform.forward * offsetX;
        cam.position += spaceShip.transform.up * offsetZ;

        cam.rotation = Quaternion.LookRotation(spaceShip.transform.position - cam.position, spaceShip.up);
        cam.Rotate(new Vector3(rotationOffsetX, 0f, 0f), Space.Self);
    }
}