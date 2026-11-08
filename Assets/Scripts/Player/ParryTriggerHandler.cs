using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParryTriggerHandler : MonoBehaviour
{

    private PlayerInput playerActions;
    private InputAction parry;
    private Collider2D parryCollider;

    public float parryTimer = 1.0f;
    public float parryCooldown = 1.0f;  
    public float parryDuration = 0.25f;
    public bool canParry = true;

    public bool somethingParried = false;

    private void Awake()
    {
        parryCollider = GetComponent<Collider2D>();
        playerActions = new PlayerInput();
        parry = playerActions.Gameplay.Parry;
        parry.performed += Parry;
    }


    private void OnEnable()
    {
        parry.Enable();
    }

    private void OnDisable()
    {
        parry.Disable();
    }


    private IEnumerator ParryCoroutine()
    {
        parryCollider.enabled = true;
        somethingParried = false;
        Debug.Log("Parrying");
        yield return new WaitForSeconds(parryDuration);
        parryCollider.enabled = false;
        Debug.Log("Stopping Parry");

        if (somethingParried)
        {
            canParry = true;
        } else
        {
            canParry = false;
            parryTimer = 0.0f;
        }

        somethingParried = false;
    }

    private void Parry(InputAction.CallbackContext obj)
    {
        if (canParry)
        {
            StartCoroutine(ParryCoroutine());
        }
       
    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Projectile" && !collision.gameObject.GetComponent<Projectile>().IsPlayerOwned)
        {

            //DO YOUR FUNNY TIME STOP HERE

            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            projectile.IsPlayerOwned = true;
            projectile.velocity = -projectile.velocity * 1.5f;
            projectile.Damage = projectile.Damage * 1.5f;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.linearVelocity = -rb.linearVelocity * 1.5f;

            //allows multi parrying
            somethingParried = true;

            GameManager.Instance.Energy += 3.0f;
        }

        if (collision.gameObject.TryGetComponent<MeleeTriggerHandler>(out MeleeTriggerHandler mt))
        {

            //DO YOUR FUNNY TIME STOP HERE

            Enemy enemy = mt.GetComponentInParent<Enemy>();
            mt.gameObject.SetActive(false);
            enemy.CanMove = false;
            enemy.Health -= mt.Damage * 1.5f;

            somethingParried = true;

            GameManager.Instance.Energy += 3.0f;

        }
    }

    private void Update()
    {
        if (!canParry)
        {
            parryTimer = Math.Clamp(parryTimer + Time.deltaTime, 0.0f, parryCooldown);

            if (parryTimer >= parryCooldown)
            {
                Debug.Log("Out");
                canParry = true;
            }
        }
        
    }
}
