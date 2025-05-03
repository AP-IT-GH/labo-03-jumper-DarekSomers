using UnityEngine;

public class ObstacleChecker : MonoBehaviour
{
    public GameObject agent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
            agent.GetComponent<JumpAgent>().EndReached();
    }
}
