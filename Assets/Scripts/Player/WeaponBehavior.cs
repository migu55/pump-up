using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBehavior : MonoBehaviour
{
    //need to get stats from somewhere

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
        fireAction.performed += FireWeapon;
    }

    private void OnDisable()
    {
        fireAction.Disable();
    }

    private void FireWeapon(InputAction.CallbackContext obj)
    {
        Vector3 dest = crosshairController.getWorldPosOfCrosshair();

        //Weapon.Fire(src, dest);
        

        Vector3 src = gameObject.transform.position;
        Quaternion rotation = Quaternion.FromToRotation(src, dest);
        rotation.x = 0;
        rotation.y = 0;

        Instantiate(projectile, src, rotation);

        Vector3 direction = (dest - src);
        Debug.Log("Src:" + src);
        Debug.Log("Dest:" + dest);
        Debug.Log("Dir:" + direction);
        Vector2 nDireciton = direction.normalized;
        projectile.velocity = nDireciton * 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
