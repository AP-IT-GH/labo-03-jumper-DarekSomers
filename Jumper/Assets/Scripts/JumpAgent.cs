using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Linq;
using static UnityEngine.Rendering.DebugUI.Table;

public class JumpAgent : Agent
{
    public Rigidbody rigidB;
    public float jumpForce;
    public float gravityMod = 3;
    public bool isGrounded = true;

    public GameObject obstacle;
    private Vector3 obstacleMoveDirection;
    private float obstacleSpeed;

    private bool badEnd;
    private bool goodEnd;

    public override void OnEpisodeBegin()
    {
        // reset de positie en orientatie als de agent gevallen is
        if (this.transform.localPosition.y < 0 || badEnd)
        {
            badEnd = false;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }

        // Bepaal richting van obstakel
        int obstacleDirection = Random.Range(0, 2);
        // Plaatsen van een obstakel + snelheid
        if (obstacleDirection == 0)
        {
            obstacle.transform.localPosition = new Vector3(-20f, 0.5f, 0);
            obstacle.transform.eulerAngles = new Vector3(0, 0, 0);
            obstacleMoveDirection = new Vector3(Random.Range(10,20) * Time.deltaTime, 0, 0);
        }
        else
        {
            obstacle.transform.localPosition = new Vector3(0, 0.5f, -20f);
            obstacle.transform.eulerAngles = new Vector3(0, -90, 0);
            obstacleMoveDirection = new Vector3(Random.Range(10, 20) * Time.deltaTime, 0, 0);

        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target en Agent posities
        sensor.AddObservation(isGrounded);
        sensor.AddObservation(this.transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        obstacle.transform.Translate(obstacleMoveDirection);
        int jump = actionBuffers.DiscreteActions[0];
        // Acties, size = 1
        if (jump == 1 && isGrounded)
        {
            rigidB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;         
        }

        // Beloningen


        if (badEnd)
        {
            AddReward(-1f);
            EndEpisode();
        }
        else if (goodEnd)
        {
            goodEnd = false;
            AddReward(1.5f);
            EndEpisode();
        }
        else if (actionBuffers.DiscreteActions[0] == 1 && isGrounded)
            AddReward(-.5f);
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
            badEnd = true;
        if (collision.gameObject.layer == 3)
            isGrounded = true;
    }

    public void EndReached()
    {
        goodEnd = true;
    }
}
