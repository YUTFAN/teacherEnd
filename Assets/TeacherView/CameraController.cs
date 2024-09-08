using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 10.0f; // 旋转速度
    public float zoomSpeed = 10.0f;
    public float dragSpeed = 2.0f;

    private Vector3 dragOrigin;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        HandleRotation();
        HandleZoom();
        HandleDrag();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // 右键拖动
        {
            yaw += rotationSpeed * Input.GetAxis("Mouse X");
            pitch -= rotationSpeed * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, 0, scroll * zoomSpeed, Space.Self);
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0)) // 左键按下
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(0)) // 左键拖动
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

            transform.Translate(-move, Space.Self);

            dragOrigin = Input.mousePosition;
        }
    }
}
