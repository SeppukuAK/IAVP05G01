using UnityEngine;
using System.Collections;

public class Flock : MonoBehaviour
{
    /// <summary>
    /// Velocidad a la que se mueve el fantasma
    /// </summary>
    [SerializeField] private float speedIndividual;
    [SerializeField] private float speedGroup;

    /// <summary>
    /// Velocidad a la que rota el fantasma
    /// 4.0f
    /// </summary>
    [SerializeField] private float rotationSpeed;

    /// <summary>
    /// Distancia a otros fantasmas que necesita para poder congregarse
    /// 3.0f
    /// </summary>
    [SerializeField] private float neighbourDistance;

    /// <summary>
    /// Tiempo que tarda el agente en saber si los fantasmas están agrupados o no. Cuanto menor sea, mejor
    /// </summary>
    [SerializeField] private float dumbTime;

    /// <summary>
    /// Velocidad real del fantasma. Si está en grupo tiene el valor de speedGroup y si está solo tiene el de speedIndividual
    /// </summary>
    private float speed;

    /// <summary>
    /// tamaño del grupo de fantasmas
    /// </summary>
    private int groupSize;

    void Start()
    {
        speed = speedIndividual;
        groupSize = 0;
        StartApplyRules();
    }

    public void StartApplyRules()
    {
        StartCoroutine(ApplyRules());
    }

    public void StopMovement()
    {
        StopAllCoroutines();

        speed = 0;
    }


    void Update()
    {
        //Mueve el fantasma hacia delante
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    private IEnumerator ApplyRules()
    {
        while (true)
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
                    if (distance <= neighbourDistance)
                    {
                        //Calculamos el centro
                        center += ghost.transform.position;

                        //Si estamos muy cerca de él, lo tenemos que evitar
                        if (distance < 2.0f)
                            avoid += (transform.position - ghost.transform.position);

                        //Aumentamos el número de fantasmas en el grupo
                        groupSize++;
                    }
                }
            }

            Vector3 direction;

            if (groupSize == 0)
            {
                speed = speedIndividual;

                //Obtenemos la dirección del grupo y giramos al fantasma gradualmente
                direction = GlobalFlock.GoalPos - transform.position;

               
            }

            //Si el fantasma está en un grupo
            else
            {
                //Obtenemos el centro total del grupo y la velocidad total del grupo
                center = center / groupSize + (goalPos - transform.position);
                speed = speedGroup;

                //Obtenemos la dirección del grupo y giramos alfantasma gradualmente
                 direction = (center + avoid) - transform.position;

            }

            //Si la dirección es distinta a la actual, rotamos gradualmente
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

            yield return new WaitForSeconds(dumbTime);

        }
    }
}

