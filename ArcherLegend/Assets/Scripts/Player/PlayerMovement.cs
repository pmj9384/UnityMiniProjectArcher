using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Camera mainCamera;
    public Vector3 cameraFixedPosition = new Vector3(0, 10f, -10f);
    public Vector3 cameraRotationEuler = new Vector3(45f, 0f, 0f);
    public VirtualJoyStick joystick;

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
        Vector2 joystickInput = joystick.Input; // 조이스틱 입력 받기

        if (joystickInput.sqrMagnitude > 0.01f) // 입력이 일정 값 이상일 때만 처리
        {
            // 이동 처리
            Vector3 moveInput = new Vector3(joystickInput.x, 0f, joystickInput.y).normalized;
            Vector3 moveDirection = moveInput * speed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDirection);

            // 회전 처리
            RotateTowardsDirection(moveInput);

            animator.SetBool("Walk", true); // 걷기 애니메이션
        }
        else
        {
            animator.SetBool("Walk", false); // 멈춤 애니메이션
        }

        // 카메라 고정
        LockCamera();
    }

    private void RotateTowardsDirection(Vector3 moveInput)
    {
        // 목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(moveInput);
        // 부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void LockCamera()
    {
        mainCamera.transform.position = cameraFixedPosition;
        mainCamera.transform.rotation = Quaternion.Euler(cameraRotationEuler);
    }
}
