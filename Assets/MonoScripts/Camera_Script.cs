
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Camera_Script : MonoBehaviour
{
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public Vector2 sensitivity = new Vector2(1, 1);
    public Vector2 smoothing = new Vector2(1, 1);
    [SerializeField] private GameObject handles;
    [SerializeField] InputField pathInputField;
    [SerializeField] private GameObject pointCloudSlider;
    [SerializeField] GameObject axisSlider;
    private Vector2 mouseAbsolute;
    private Vector2 smoothMouse;
    private Vector2 targetDirection;
    private float zoomSpeed = 1.0f;
    private void Start()
    {
        targetDirection = transform.localRotation.eulerAngles;
    }
    Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.transform.position = new Vector3(0, 1, -10);
        cam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        cam.nearClipPlane = 0.05f;
    }

    private void Update()
    {
        var x = Input.GetAxis("Horizontal")* Time.deltaTime * 3f;
        var z = Input.GetAxis("Vertical")* Time.deltaTime * 3f;
   
        transform.Translate( x * 3 , 0, z * 3 );
        
        transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
        
        if (!Input.GetMouseButton(0) || EventSystem.current.currentSelectedGameObject == handles) return;

        if (EventSystem.current.currentSelectedGameObject == pathInputField.gameObject) return;
        
        if (EventSystem.current.currentSelectedGameObject == pointCloudSlider) return;

        if (EventSystem.current.currentSelectedGameObject == axisSlider.gameObject) return;
        
        // Mouse look
        var targetOrientation = Quaternion.Euler(targetDirection);

        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        mouseAbsolute += smoothMouse;

        if (clampInDegrees.x < 360)
            mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        if (clampInDegrees.y < 360)
            mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        var yRotation = Quaternion.AngleAxis(mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
        transform.localRotation *= yRotation;
        
        
        // WASD
        //var isShiftPressed = true; //Input.GetKey(KeyCode.LeftShift);

        
    }
    
}
