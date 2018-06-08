using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    //Dimensiones del escenario

    //TODO: TOCAR ESTA MIERDA
    [SerializeField] private int width;
    [SerializeField] private int depth;
    [SerializeField] private int height;
    //[SerializeField] private int sceneTopY = 15;
    //[SerializeField] private int sceneBotY = -3;
    [SerializeField] private Vector3 offset;

    /// <summary>
    /// Prefab del Fantasma para crealos en el grupo
    /// </summary>
    public GameObject GhostPrefab;

    /// <summary>
    /// Número de peces que crea el Manager
    /// </summary>
    private static int numFish = 20;

    /// <summary>
    /// Array con todos los Fantasmas
    /// </summary>
    public static GameObject[] AllGhosts = new GameObject[numFish];

    /// <summary>
    /// Tiene que estar en el centro de los fantasmas
    /// </summary>
    public static Vector3 GoalPos = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = offset + new Vector3(Random.Range(-width, width), Random.Range(-height, height), Random.Range(-depth, depth));
            AllGhosts[i] = Instantiate(GhostPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,200) < 1)
        {
            GoalPos = offset + new Vector3(Random.Range(-width, width), Random.Range(-height, height), Random.Range(-depth, depth));
        }
    }
}
