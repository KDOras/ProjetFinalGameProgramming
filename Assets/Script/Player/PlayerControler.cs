using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public Character character;

    [Header("Settings")]
    public bool isFirstPerson = false;
    public bool IsJumping = false;

    [Header("References")]
    public FirstPersonCamera firstPersonCamera;
    public ThirdPersonCamera thirdPersonCamera;
    public Animator animator;
    public Telekinesis telekinesis;

    [Header("Input")]
    public Vector2 moveInputValue;
    public Vector2 lookInputValue;

    public void Start()
    {
        SwitchCamera();
    }

    public void OnMove(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (!IsJumping)
        {
            IsJumping = value.isPressed;
            if (IsJumping)
            {
                animator.SetBool(Animator.StringToHash("IsJumping"), IsJumping);
                StartCoroutine(AnimJumpDelay(0.5f));
                StartCoroutine(ResetJumpAfterDelay(1.5f));
            }
        }
    }

    [System.Obsolete]
    public void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            telekinesis.HandleInteraction();
        }
    }

    public void OnLook(InputValue value)
    {
        lookInputValue = value.Get<Vector2>();
    }

    public void SwitchCamera()
    {
        isFirstPerson = !isFirstPerson;
        ChangeCamera();
    }
    private void ChangeCamera()
    {
        firstPersonCamera.enabled = isFirstPerson;
        thirdPersonCamera.enabled = !isFirstPerson;

        if (isFirstPerson)
        {
            firstPersonCamera.playerCamera.transform.parent = character.transform;
            firstPersonCamera.playerCamera.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
        }
        else
        {
            thirdPersonCamera.springArm.playerCamera.transform.parent = thirdPersonCamera.springArm.transform;
            thirdPersonCamera.springArm.playerCamera.transform.localPosition = new Vector3(0.0f, 1.5f, 2.5f);
        }
    }

    [System.Obsolete]
    private void Update()
    {
        if (telekinesis != null)
        {
            telekinesis.HandleInteraction();
        }
        // move the character based on moveinputValue
        Vector3 x = moveInputValue.x * character.transform.right;
        Vector3 z = moveInputValue.y * character.transform.forward;

        Vector3 velocity = x + z;
        character.Move(velocity * Time.deltaTime);
        float AnimSpeed = velocity.magnitude;
        AnimSpeed = Mathf.Clamp(AnimSpeed, 0.0f, 1.0f);
        animator.SetFloat(Animator.StringToHash("Speed"), AnimSpeed);
    }

    public void OnValidate()
    {
       // SwitchCamera();
    }

    private IEnumerator ResetJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsJumping = false;
        animator.SetBool(Animator.StringToHash("IsJumping"), IsJumping);
    }

    private IEnumerator AnimJumpDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        character.Jump(5);
    }
}
