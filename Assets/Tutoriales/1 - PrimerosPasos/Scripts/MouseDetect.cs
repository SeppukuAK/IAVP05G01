using UnityEngine;
using UnityEngine.AI;

// Detección rudimentaria del ratón
public class MouseDetect : MonoBehaviour {

    public GameObject npc; 

    void OnMouseDown() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            npc.GetComponent<NavMeshAgent>().SetDestination(hit.point);
    }
}
