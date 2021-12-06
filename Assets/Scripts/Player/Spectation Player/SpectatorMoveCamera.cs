using UnityEngine;

public class SpectatorMoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition;
    [SerializeField] Transform cameraRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = cameraPosition.position;
        transform.rotation = cameraRotation.rotation;
    }
}
