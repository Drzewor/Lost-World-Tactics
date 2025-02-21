using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 3f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    [SerializeField] private float minFollowXposition = -2;
    [SerializeField] private float maxFollowXposition = 40;
    [SerializeField] private float minFollowZposition = -2;
    [SerializeField] private float maxFollowZposition = 42;
    [SerializeField] private CinemachineFollow cinemachineCamera;
    private Vector3 targetFollowOffset;

    private void Start() 
    {
        targetFollowOffset = cinemachineCamera.FollowOffset;
    }
    private void Update() 
    {
        HandleMovement();

        HandleRotation();
        
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
        //transform.position += moveVector * moveSpeed * Time.deltaTime;
        Vector3 newCameraPosition = transform.position + (moveVector * moveSpeed * Time.deltaTime);

        if(newCameraPosition.x < minFollowXposition)
        {
            newCameraPosition.x = minFollowXposition;
        }
        if(newCameraPosition.x > maxFollowXposition)
        {
            newCameraPosition.x = maxFollowXposition;
        }
        if(newCameraPosition.z < minFollowZposition)
        {
            newCameraPosition.z = minFollowZposition;
        }
        if(newCameraPosition.z > maxFollowZposition)
        {
            newCameraPosition.z = maxFollowZposition;
        }

        transform.position = newCameraPosition;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0,0,0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount(); 

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        float zoomSpeed = 5f;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineCamera.FollowOffset = 
            Vector3.Lerp(cinemachineCamera.FollowOffset,targetFollowOffset,zoomSpeed*Time.deltaTime);   
    }
}
