using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DeerWander : MonoBehaviour
{
    public float wanderRadius = 10f;         // Bán kính di chuyển loanh quanh
    public float wanderInterval = 5f;        // Thời gian chờ giữa các lần di chuyển
    public float stopDistance = 1f;          // Khoảng cách được coi là "đã đến nơi"

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderInterval;
        MoveToRandomPoint();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance < stopDistance && timer >= wanderInterval)
        {
            MoveToRandomPoint();
            timer = 0;
        }

        // Cập nhật animation dựa vào tốc độ
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
