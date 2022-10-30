using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBall : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] float speed = 1;


    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == ball.Position)
        {
            return;
        }

        this.transform.position = Vector3.Lerp(
            this.transform.position,
            ball.Position,
            Time.deltaTime * speed
            );

        if (ball.IsMoving)
            return;

        // kalau sudah sangat dekat langsung teleport
        if (Vector3.Distance(transform.position, ball.Position) < 0.1f)
        {
            transform.position = ball.Position;
        }
    }
}
