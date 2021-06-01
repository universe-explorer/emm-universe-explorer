using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform spaceShip;
    public float offsetX = 10.0f;
    public float offsetZ = 2.5f;
    public float rotationOffsetX = 2.5f;

    void Update()
    {
        var cam = transform;

        cam.position = spaceShip.transform.position - spaceShip.transform.forward * offsetX;
        cam.position += new Vector3(0, offsetZ, 0);

        cam.rotation = Quaternion.LookRotation(spaceShip.transform.position - cam.position, spaceShip.transform.up);
        cam.Rotate(new Vector3(rotationOffsetX, 0f, 0f), Space.Self);
    }
}