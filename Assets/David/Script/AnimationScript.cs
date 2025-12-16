using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    Animator animator;
    StatsScript StatsScript;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //get the Input from Horizontal axis
        float h = Input.GetAxis("Horizontal");
        //get the Input from Vertical axis
        float v = Input.GetAxis("Vertical");

        //update the position
        transform.position = transform.position + new Vector3(h * StatsScript.MoveSpeed * Time.deltaTime, v * StatsScript.MoveSpeed * Time.deltaTime, 0);

        //output to log the position change
        Debug.Log(transform.position);

        if (v < 0)
        {
            animator.SetBool("IsWalkingFront", true);
            animator.SetBool("IsWalkingBack", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalkingLeft", false);
            animator.SetBool("IsWalkingRight", false);
        }

        if (v > 0)
        {
            animator.SetBool("IsWalkingBack", true);
            animator.SetBool("IsWalkingFront", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalkingLeft", false);
            animator.SetBool("IsWalkingRight", false);
        }

        if (h < 0)
        {
            animator.SetBool("IsWalkingLeft", true);
            animator.SetBool("IsWalkingRight", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalkingFront", false);
            animator.SetBool("IsWalkingBack", false);
        }

        if (h > 0)
        {
            animator.SetBool("IsWalkingRight", true);
            animator.SetBool("IsWalkingLeft", false);
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsWalkingFront", false);
            animator.SetBool("IsWalkingBack", false);
        }
    }

}