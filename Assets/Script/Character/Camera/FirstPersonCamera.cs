using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Reference")]
    public Camera playerCamera;
    public PlayerControler PlayerControler;

    [Header("Settings")]
    public bool isInvertedY = true;
    public float sensitivity = 9f;
    private float yaw = .0f;
    private float pitch = .0f;
    public Vector2 pitchLimits = new Vector2(-40.0f, 80.0f);
    public void Update()
    {
        yaw += PlayerControler.lookInputValue.x * sensitivity;
        pitch += PlayerControler.lookInputValue.y * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

        PlayerControler.character.transform.rotation = Quaternion.Euler(0, yaw, 0.0f);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch * (isInvertedY ? 1 : -1), 0, 0.0f);
    }
}
