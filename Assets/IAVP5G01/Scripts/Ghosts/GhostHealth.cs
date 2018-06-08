using UnityEngine;
using Opsive.ThirdPersonController;

/// <summary>
/// Hereda de GhostHealth para poder dar un feedback sonoro y de animación cuando el fantasma muere
/// </summary>
public class GhostHealth : Health
{
    private const string DEATH = "Anim_Death";
    private Animation anim;

    public AudioSource DieAudioSource;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    /// <summary>
    /// The object is no longer alive. Kill it. Call the corresponding server or client method.
    /// </summary>
    /// <param name="position">The position of the damage.</param>
    /// <param name="force">The amount of force applied to the object while taking the damage.</param>
    /// <param name="attacker">The GameObject that killed the character.</param>
    protected override void Die(Vector3 position, Vector3 force, GameObject attacker)
    {
        //Feedback
        DieAudioSource.Play();
        anim.CrossFade(DEATH);

        GetComponent<Flock>().StopMovement();

        base.Die(position, force, attacker);
    }
   
}
