using UnityEngine;

public class SpectatorRotation : MonoBehaviour
{
    public RotateAxes axes = RotateAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVer = 9.0f;

    public float minVer = -45.0f;
    public float maxVer = 45.0f;
    private float rotationX = 0.0f;

    private void Start()
    {
        //Freeze phisics influence on player rotation
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
        {
            body.freezeRotation = true;
        }
    }
    private void Update()
    {
        //Rotation horizontal
        if (axes == RotateAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        }
        //Rotation vertical
        else if (axes == RotateAxes.MouseY)
        {
            rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            rotationX = Mathf.Clamp(rotationX, minVer, maxVer);

            float rotationY = transform.localEulerAngles.y;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
        //Rotation both
        else
        {
            rotationX -= Input.GetAxis("Mouse Y") * sensitivityVer;
            rotationX = Mathf.Clamp(rotationX, minVer, maxVer);
            float delta = Input.GetAxis("Mouse X") * sensitivityHor;

            float rotationY = transform.localEulerAngles.y + delta;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
}

[System.Serializable]
public enum RotateAxes
{
    MouseXAndY = 0,
    MouseX = 1,
    MouseY = 2
}