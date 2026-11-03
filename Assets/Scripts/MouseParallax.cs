using UnityEngine;

public class MouseParallax : MonoBehaviour
{
    [SerializeField] private float maxLeanDistance = 1f; // Max distance camera can move toward mouse
    [SerializeField] private float smoothSpeed = 2f;     // How fast the camera responds

    private Transform playerTransform;
    private CrosshairController crosshairController;

    private void Start()
    {
        playerTransform = transform.parent;
        crosshairController = FindAnyObjectByType<CrosshairController>();
    }

    private void Update()
    {
        if (playerTransform == null) return;

        // 1. Get mouse position in world space
        Vector3 mouseWorldPos = crosshairController.getWorldPosOfCrosshair();
        mouseWorldPos.z = 0f; // Keep it 2D

        // 2. Calculate direction and distance from player to mouse
        Vector3 targetDir = mouseWorldPos - playerTransform.position;

        // 3. Clamp the distance so the camera doesn't lean infinitely far away
        Vector3 clampedOffset = Vector3.ClampMagnitude(targetDir, maxLeanDistance);

        // 4. Smoothly move this anchor toward the target offset relative to the player
        Vector3 targetPosition = playerTransform.position + clampedOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
