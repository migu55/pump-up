using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    //need to get stats from somewhere

    public Texture2D texture;

    PlayerInput playerInput;
    InputAction fireAction;

    [SerializeField]
    private Projectile projectile;
    private CrosshairController crosshairController;

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
        Vector3 dest = crosshairController.getWorldPosOfCrosshair();

        Quaternion rotation = Quaternion.FromToRotation(gameObject.transform.position, dest);
        rotation.x = 0;
        rotation.y = 0;

        Instantiate(projectile, gameObject.transform.position, rotation);

        Vector3 direction = dest - gameObject.transform.position;
        //Debug.Log("Src:" + gameObject.transform.position);
        //Debug.Log("Dest:" + dest);
        //Debug.Log("Dir:" + direction);
        Vector2 nDireciton = direction.normalized;
        projectile.velocity = nDireciton * 15.0f; //handle projectile speed later
    }

    private void RotateToCrosshair()
    {
        Quaternion rotation = Quaternion.FromToRotation(gameObject.transform.position, crosshairController.getWorldPosOfCrosshair());
        rotation.x = 0;
        rotation.y = 0;
        rotation.z = rotation.z * -1;
        gameObject.transform.rotation = rotation;
        //Debug.Log("R:" + rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // RotateToCrosshair(); fix later
    }
}
