using UnityEngine;
using Opsive.ThirdPersonController;

/// <summary>
/// Tomando como referencia HealthPickup, utilizamos este componente para cuando entre en contacto
/// el fantasma con un personaje, este reciba daño.
/// </summary>
public class DamageByContact : MonoBehaviour
{
    [Tooltip("The amount of damage to deal")]
    [SerializeField]
    protected float damageAmount;

    [Tooltip("The sound to play when the object is picked up")]
    [SerializeField]
    protected AudioClip damageSound;

    //Referencias a componentes
    private AudioSource audioSource;

    /// <summary>
    /// Coge referencias a componentes e inicializa valores por defecto
    /// </summary>
    private void Awake()
    {
        if (damageSound != null)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    /// <summary>
    /// Hace daño a cualquier objeto que entre en el Trigger y tenga un HealthComponent
    /// </summary>
    public virtual void OnTriggerEnter(Collider other)
    {
        Health health;

        if ((health = Utility.GetComponentForType<Health>(other.gameObject)) != null)
        {
            // Play a pickup sound.
            if (damageSound != null)
            {
                audioSource.clip = damageSound;
                audioSource.Play();
            }

            //Hacemos daño al personaje
            health.Damage(damageAmount, transform.position, Vector3.zero);
        }
    }
}