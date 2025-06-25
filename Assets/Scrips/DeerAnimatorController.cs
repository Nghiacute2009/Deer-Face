using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class DeerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float speed = agent.velocity.magnitude;

        // Set tham số "Speed" trong Animator để animator tự quyết định chuyển đổi animation
        animator.SetFloat("Speed", speed, 0.2f, Time.deltaTime); // dùng damping để mượt hơn
        animator.SetFloat("Speed", agent.velocity.magnitude, 0.2f, Time.deltaTime);

    }
}
