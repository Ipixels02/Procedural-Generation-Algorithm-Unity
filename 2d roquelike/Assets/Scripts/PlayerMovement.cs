using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector2 dir;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        TakeInput();
        Move();
    }

    private void Move()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (dir.x !=0 || dir.y != 0)
        {
            SetAnimatorMovement(dir);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    private void TakeInput()
    {
        dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }
    }

    private void SetAnimatorMovement(Vector2 direction)
    {
        animator.SetLayerWeight(1, 1);
        animator.SetFloat("xDir", direction.x);
        animator.SetFloat("yDir", direction.y);
    }
}
