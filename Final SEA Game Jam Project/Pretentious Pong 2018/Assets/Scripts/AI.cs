using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject positionMover;
    public float speed = 30.0f;

    private Ball ball;
    private GameManager gameManager;
    private Rigidbody2D rb2d;
    private Vector2 move;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ball = gameManager.ball.GetComponent<Ball>();
    }

    private void Update()
    {
        //UpdateRecover();
    }

    void FixedUpdate()
    {
        // Follow ball once it is launched and game is not stopped.
        if (ball.isLaunch && !gameManager.stopGame)
        {
            float d = gameManager.ball.transform.position.y - transform.position.y + ball.returnRange;
            if (d > 0)
            {
                move.y = speed * Mathf.Min(d, 1.0f);
            }
            if (d < 0)
            {
                move.y = -(speed * Mathf.Min(-d, 1.0f));
            }
            rb2d.velocity = move;
        }
    }
    public void StopAI()
    {
        speed = 0.0f;
        rb2d.velocity = Vector2.zero;
    }
    
    void UpdateRecover()
    {
        if (transform.position.x > positionMover.transform.position.x)
        {
            rb2d.velocity = new Vector2(-1.0f, rb2d.velocity.y);
        }
        if (transform.position.x < positionMover.transform.position.x)
        {
            rb2d.velocity = new Vector2(1.0f, rb2d.velocity.y);
        }
    }
}
