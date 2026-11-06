using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public abstract class Weapon : MonoBehaviour
{
    //need to get stats from somewhere

    public Texture2D texture;

    PlayerInput playerInput;
    InputAction fireAction;

    public float ProjectileSpeed = 30.0f;

    [SerializeField]
    private Transform pivot;

    public float rotationSpeed = 1000.0f;

    [SerializeField]
    private Projectile projectile;
    private CrosshairController crosshairController;

    private Vector3 crosshairWorldPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        fireAction = playerInput.Gameplay.Shoot;
        crosshairController = FindAnyObjectByType<CrosshairController>();
    }

    private void OnEnable()
    {
        fireAction.Enable();
        fireAction.performed += Fire;
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }

    public virtual void Fire(InputAction.CallbackContext obj)
    {

        Quaternion rotation = Quaternion.FromToRotation(gameObject.transform.position, crosshairWorldPosition);
        rotation.x = 0;
        rotation.y = 0;

        Projectile proj = Instantiate(projectile, gameObject.transform.position, rotation);
        proj.IsPlayerOwned = true;

        Vector3 direction = crosshairWorldPosition - gameObject.transform.position;
        //Debug.Log("Src:" + gameObject.transform.position);
        //Debug.Log("Dest:" + dest);
        //Debug.Log("Dir:" + direction);
        Vector2 nDireciton = direction.normalized;
        projectile.velocity = nDireciton * ProjectileSpeed; //handle projectile speed later
    }

    private void RotateToCrosshair()
    {
        Vector2 direction = crosshairWorldPosition - pivot.position;

        // Calculate angle toward target
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate the rotation difference
        float currentAngle = transform.eulerAngles.z;

        // Rotate around the pivot
        transform.RotateAround(
            pivot.position,
            Vector3.forward,
            targetAngle - currentAngle
        );
    }

    // Update is called once per frame
    void Update()
    {
        crosshairWorldPosition = crosshairController.getWorldPosOfCrosshair();

        RotateToCrosshair();
    }
}
