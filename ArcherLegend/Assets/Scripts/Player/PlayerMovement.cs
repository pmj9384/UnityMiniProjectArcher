using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;          
    public Camera mainCamera;                 
    public Vector3 cameraFixedPosition = new Vector3(0, 10f, -10f); 
    public Vector3 cameraRotationEuler = new Vector3(45f, 0f, 0f); 
    public VirtualJoyStick joystick;    
    // public VirtualJoyStick rightJoystick; 

    private Rigidbody rb;
    private Animator animator; 

    private float rotationSpeed = 10f;  

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();  

        Cursor.lockState = CursorLockMode.None;  
        Cursor.visible = true;  
                
        mainCamera.transform.position = cameraFixedPosition;
        mainCamera.transform.rotation = Quaternion.Euler(cameraRotationEuler);
    }

    private void FixedUpdate()
    {
      
        float horizontal = 0f;
        float vertical = 0f;

#if UNITY_ANDROID || UNITY_IOS
        Vector2 joystickInput = joystick.Input; 
        horizontal = joystickInput.x;
        vertical = joystickInput.y;
#else 
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
#endif

        Vector3 moveInput = (transform.right * horizontal + transform.forward * vertical).normalized;

        if (moveInput.sqrMagnitude > 0.01f)  
        {
            Vector3 moveDirection = moveInput * speed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDirection);
            animator.SetBool("Walk", true);  // 걷기 애니메이션
        }
        else
        {
            animator.SetBool("Walk", false); // 멈춤 애니메이션
        }

        // RotateWithRightJoystick();  

        // 카메라 고정
        LockCamera();          
    }

//     private void RotateWithRightJoystick()
//     {
// #if UNITY_ANDROID || UNITY_IOS
  
//         Vector2 rightJoystickInput = rightJoystick.Input;
//         float horizontal = rightJoystickInput.x;
//         float vertical = rightJoystickInput.y;

//         if (horizontal != 0f || vertical != 0f)
//         {
            
//             Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

//             Quaternion targetRotation = Quaternion.LookRotation(direction);
//             transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
//         }
// #endif
//     }

    private void LockCamera()
    {
        mainCamera.transform.position = cameraFixedPosition;
        mainCamera.transform.rotation = Quaternion.Euler(cameraRotationEuler);
    }
}
