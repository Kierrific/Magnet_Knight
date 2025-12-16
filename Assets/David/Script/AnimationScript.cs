using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (v < 0)
        {
            animator.SetBool("IsWalkingFront", true);
        }
    }

}