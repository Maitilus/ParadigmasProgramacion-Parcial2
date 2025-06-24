using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Links")]
    public Camera playerCamera;

    [Header("Stats")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    //Private Variables
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    [SerializeField] private float InteractionDistance;
    public float Damage;

    #endregion


    void Start()
    {
        //Get Character Controller
        characterController = GetComponent<CharacterController>();

        //Hide And Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //Check For Jump and Apply Force
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        //Add Gravity if Midair
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        //Check For Crouch and Shrink and Reduce Speed
        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        //Delta Time
        characterController.Move(moveDirection * Time.deltaTime);

        //Camera Rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DealDamage();
        }
    }

    private void InteractWithObject()
    {
        Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin,ray.direction * InteractionDistance);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionDistance))
        {
            hit.collider.GetComponent<IInteraction>()?.Interact();
        }
    }

    private void DealDamage()
    {
        Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin,ray.direction * InteractionDistance);

        if (Physics.Raycast(ray, out RaycastHit hit, InteractionDistance))
        {
            hit.collider.GetComponent<ITakeDamage>()?.ReduceHealth(Damage);
        }
    }
}
