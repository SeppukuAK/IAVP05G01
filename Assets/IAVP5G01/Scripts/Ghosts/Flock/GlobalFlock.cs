using UnityEngine;
using System.Collections;
public class GlobalFlock : MonoBehaviour
{
    //Dimensiones del escenario
    [SerializeField] private int width;
    [SerializeField] private int depth;
    [SerializeField] private int height;
    [SerializeField] private int offsetY;
    [SerializeField] private float seconds;
    /// <summary>
    /// Prefab del Fantasma para crearlos en el grupo
    /// </summary>
    public GameObject GhostPrefab;

    /// <summary>
    /// Número de fantasmas que crea el Manager
    /// </summary>
    private static int numGhost = 20;

    /// <summary>
    /// Array con todos los Fantasmas
    /// </summary>
    public static GameObject[] AllGhosts = new GameObject[numGhost];

    /// <summary>
    /// Punto de destino de los fantasmas. Tiene que estar en el centro de los fantasmas
    /// </summary>
    public static Vector3 GoalPos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numGhost; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-width, width), Random.Range(-height, height) + offsetY, Random.Range(-depth, depth));
            AllGhosts[i] = Instantiate(GhostPrefab, pos, Quaternion.identity);
        }
        StartCoroutine(ModifyGoal());

    }

    /// <summary>
    /// Corrutina que modifica el punto de destino del fantasma cada x segundos
    /// </summary>
    /// <returns></returns>
    private IEnumerator ModifyGoal()
    {
        while (true)
        {
            GoalPos = new Vector3(Random.Range(-width, width), Random.Range(-height, height) + offsetY, Random.Range(-depth, depth));
            yield return new WaitForSeconds(seconds);
        }
        
    }
}
