using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public PlayerControler playerController;
    public SpringArm springArm;

    [Header("Settings")]
    public bool isInvertedY = true;
    public float sensitivity = 0.2f;
    public Vector2 pitchLimits = new Vector2(-40f, 80.0f);


    private float yaw = 0.0f;
    private float pitch = 0.0f;


    public void Update()
    {
        // Yaw
        yaw += playerController.lookInputValue.x * sensitivity;

        // Pitch
        pitch += playerController.lookInputValue.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        playerController.character.transform.localRotation = Quaternion.Euler(0, yaw, 0);
        springArm.transform.localRotation = Quaternion.Euler(pitch * (isInvertedY ? 1 : -1), 0, 0);

    }
}
