using UnityEngine;

public class Flock : MonoBehaviour
{
    /// <summary>
    /// Velocidad a la que se mueve el fantasma
    /// </summary>
    public float Speed;

    /// <summary>
    /// Velocidad a la que rota el fantasma
    /// 4.0f
    /// </summary>
    public float RotationSpeed;

    /// <summary>
    /// Distancia a otros fantasmas que necesita para poder congregarse
    /// 3.0f
    /// </summary>
    public float NeighbourDistance;

    ///// <summary>
    ///// Direccion promedio
    ///// </summary>
    //Vector3 averageHeading;

    ///// <summary>
    ///// Posición promedio
    ///// </summary>
    //Vector3 averagePosition;

    void Start()
    {
        Speed = Random.Range(0.5f, 1.0f);
    }

    void Update()
    {
        //Actualizamos si ha salido de la escena
        bool turning = Vector3.Distance(transform.position, Vector3.zero) >= GlobalFlock.SceneSize;

        //Si está volviendo
        if (turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);

            Speed = Random.Range(0.5f, 1);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }

        //Mueve el pez hacia delante
        transform.Translate(0, 0, Time.deltaTime * Speed);
    }

    /// <summary>
    /// Hace todo el trabajo de congregarse
    /// </summary>
    private void ApplyRules()
    {
        //Cada pez necesita información de todos los demás fantasmas
        GameObject[] allGhosts = GlobalFlock.AllGhosts;

        Vector3 center = Vector3.zero;  //Centro del grupo
        Vector3 avoid = Vector3.zero;   //Vector para evitar colisionar a otros fantasmas

        float groupSpeed = 0.1f;    //Velocidad del grupo

        Vector3 goalPos = GlobalFlock.GoalPos; //Posición a la que se dirigen

        float distance; //Aux

        int groupSize = 0; //Cuantos fantasmas están juntos a este

        foreach (GameObject ghost in allGhosts)
        {
            //Si el fantasma no es él mismo
            if (ghost != this.gameObject)
            {
                //Calculamos la distancia a él.
                distance = Vector3.Distance(ghost.transform.position, transform.position);

                //Si forma parte de su grupo
                if (distance <= NeighbourDistance)
                {
                    //Calculamos el centro
                    center += ghost.transform.position;

                    //Si estamos muy cerca de él, lo tenemos que evitar
                    if (distance < 1.0f)
                        avoid += (transform.position - ghost.transform.position);

                    //Calculamos la velocidad del grupo
                    groupSpeed += ghost.GetComponent<Flock>().Speed;

                    //Aumentamos el número de fantasmas en el grupo
                    groupSize++;
                }
            }

            //Si el fantasma está en un grupo
            if (groupSize > 0)
            {
                //Obtenemos el centro total del grupo y la velocidad total del grupo
                center = center / groupSize + (goalPos - transform.position);
                Speed = 3.0f;

                //Obtenemos la dirección del grupo y giramos al pez gradualmente
                Vector3 direction = (center + avoid) - transform.position;

                //Si la dirección es distinta a la actual, rotamos gradualmente
                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
            }

            Debug.Log(Speed);

        }
    }
}
