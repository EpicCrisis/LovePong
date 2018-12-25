using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 300f;

    public GameObject positionMover;
    private GameManager gameManager;
    private Rigidbody2D rb2d;
    public float mouseSensitivityY = 1;
    float playerPosition;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //UpdateRecover();
    }

    void FixedUpdate()
    {
        if (!gameManager.stopGame)
        {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, targetPos.y, transform.position.z), speed * Time.deltaTime);
        }
    }

    void UpdateRecover()
    {
        if(transform.position.x > positionMover.transform.position.x)
        {
            rb2d.velocity = new Vector2(-0.05f, rb2d.velocity.y);
        }
        if (transform.position.x < positionMover.transform.position.x)
        {
            rb2d.velocity = new Vector2(0.05f, rb2d.velocity.y);
        }
    }
}
