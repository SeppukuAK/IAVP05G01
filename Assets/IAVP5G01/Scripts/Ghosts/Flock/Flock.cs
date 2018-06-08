using UnityEngine;

public class Flock : MonoBehaviour
{
    /// <summary>
    /// Velocidad a la que se mueve el fantasma
    /// </summary>
    public float SpeedIndividual;
    public float SpeedGroup;

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

    private float speed;
    private int groupSize;

    void Start()
    {
        speed = SpeedIndividual;
        groupSize = 0;
    }

    void Update()
    {
        if (Random.Range(0, 5) < 1)
            ApplyRules();

        if (groupSize == 0)
        {
            speed = SpeedIndividual;

            //Obtenemos la dirección del grupo y giramos al pez gradualmente
            Vector3 direction = GlobalFlock.GoalPos;

            //Si la dirección es distinta a la actual, rotamos gradualmente
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
        }

        //Mueve el pez hacia delante
        transform.Translate(0, 0, Time.deltaTime * speed);
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

        Vector3 goalPos = GlobalFlock.GoalPos; //Posición a la que se dirigen

        float distance; //Aux

        groupSize = 0; //Cuantos fantasmas están juntos a este

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

                    //Aumentamos el número de fantasmas en el grupo
                    groupSize++;
                }
            }
        }

        //Si el fantasma está en un grupo
        if (groupSize > 0)
        {
            //Obtenemos el centro total del grupo y la velocidad total del grupo
            center = center / groupSize + (goalPos - transform.position);
            speed = SpeedGroup;

            //Obtenemos la dirección del grupo y giramos al pez gradualmente
            Vector3 direction = (center + avoid) - transform.position;

            //Si la dirección es distinta a la actual, rotamos gradualmente
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotationSpeed * Time.deltaTime);
        }
    }
}
