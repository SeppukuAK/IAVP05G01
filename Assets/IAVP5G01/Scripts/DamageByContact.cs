using UnityEngine;
using Opsive.ThirdPersonController;

/// <summary>
/// Allows an object with the Invetory component to pickup items when that object enters the trigger.
/// </summary>
public class DamageByContact: MonoBehaviour
{
    [Tooltip("The amount of damage to deal")]
    [SerializeField] protected float damageAmount;

    /// <summary>
    /// Add health to any object that enters the trigger and has the Health component.
    /// </summary>
    /// <param name="other">The object which may pick up the health if it has a Health component.</param>
    public virtual void OnTriggerEnter(Collider other)
    {
#if ENABLE_MULTIPLAYER
            // The server should pick up the health and persist it to the clients.
            if (!isServer) {
                return;
            }
#endif
        //// Cannot pickup the item if it is depleted.
        //if (IsDepleted)
        //{
        //    return;
        //}

        Health health;
        if ((health = Utility.GetComponentForType<Health>(other.gameObject)) != null)
        {
            health.Damage(damageAmount, transform.position,Vector3.zero);

            //ObjectPickup();
        }
    }
}