using System.Collections;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    public Transform holdPosition; // Empty GameObject in front of the player
    public float grabSpeed = 10f;  // Speed at which the object moves to the hold position
    public float throwForce = 10f; // Force applied when throwing
    public float raycastDistance = 3f;

    private Rigidbody grabbedObject;
    private bool isGrabbing = false;
    public bool isThrowing = false;
    public Character character;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        HandleInteraction();
    }

    [System.Obsolete]
    public void HandleInteraction()
    {
        {
            if (Input.GetKeyDown(KeyCode.E))  // If the left mouse button is pressed
            {
                RaycastHit hit;
                Ray ray = character.PlayerCamera.ScreenPointToRay(Input.mousePosition);  // Ray from the camera

                // Visualize the ray
                Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.blue, 1f);  // Draw the ray in red

                if (Physics.Raycast(ray, out hit, raycastDistance))  // Shoot the ray
                {
                    if (hit.collider.CompareTag("Grabbable"))  // If the object is grabbable
                    {
                        TryGrabObject(hit.collider.gameObject);  // Grab the object
                    }
                }
            }

            if (isGrabbing && grabbedObject != null)
            {
                MoveObjectToHoldPosition();
            }

            if (Input.GetKeyUp(KeyCode.E) && isGrabbing)
            {
                animator.SetBool("IsThrowing", true);
                StartCoroutine(AnimThrowDelay(0.8f));
                StartCoroutine(ResetThrowingAfterDelay(1.5f));
            }
        }
    }


    [System.Obsolete]
    public void TryGrabObject(GameObject objectToGrab)
    {
        Rigidbody rb = objectToGrab.GetComponent<Rigidbody>();
        if (rb != null)
        {
            grabbedObject = rb;
            grabbedObject.useGravity = false;  // Disable gravity for the grabbed object
            grabbedObject.drag = 10;           // Increase drag to prevent excessive movement
            grabbedObject.constraints = RigidbodyConstraints.FreezeRotation;  // Freeze rotation

            // Set the object as kinematic to prevent it from interacting with the physics engine while it's grabbed
            grabbedObject.isKinematic = true;

            isGrabbing = true;  // Set the state to "grabbing"
        }
    }

    // Move the object towards the holding position using transform (since it's kinematic)
    public void MoveObjectToHoldPosition()
    {
        // Move the object towards the hold position
        grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, holdPosition.position, grabSpeed * Time.deltaTime);
    }

    // Throw the object and reset the grabbing state
    [System.Obsolete]
    public void ThrowObject()
    {
        // Set the object state to "not grabbing" and reset its properties
        isGrabbing = false;
        grabbedObject.useGravity = true;  // Re-enable gravity
        grabbedObject.drag = 1;           // Reset drag to default
        grabbedObject.constraints = RigidbodyConstraints.None;  // Remove constraints

        // Re-enable physics interactions
        grabbedObject.isKinematic = false;  // Disable kinematic, enabling physics
        // Apply velocity in the direction the player is facing (forward direction)
        Vector3 throwDirection = character.transform.forward;  // Get the forward direction of the player
        grabbedObject.velocity = throwDirection * throwForce;  // Throw the object with the specified force
        grabbedObject = null;  // Clear the grabbed object reference
    }

    private IEnumerator ResetThrowingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isThrowing = false;
        animator.SetBool(Animator.StringToHash("IsThrowing"), isThrowing);
    }

    [System.Obsolete]
    private IEnumerator AnimThrowDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ThrowObject();
    }
}