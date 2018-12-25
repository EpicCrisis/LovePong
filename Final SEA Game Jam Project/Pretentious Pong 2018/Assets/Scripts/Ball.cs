using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using EZCameraShake;

public class Ball : MonoBehaviour
{
    private AudioManager audioManager;
    public ParticleSystem collideParticle;
    public GameObject BallScale;
    public bool isLaunch = false;
    public float speed = 30.0f;
    public float timeToIncreaseSpeed = 5.0f;

    private float timeCounterToIncreaseSpeed;
    private GameManager gameManager;
    private Rigidbody2D rb2d;
    private UIManager uiManager;
    private AI ai;
    bool justhit = false;
    public bool StickToPlayer = false;
    public float returnRange = 2f;
    public float insideRange = 1f;
    public bool randomizeRange = false;
    public float range = 2f;
    public DialogueManager dialoguemanager;
    private bool hitPlayer = true; // is the ball coming from the player's side
    private TextMesh ballTextMesh;
    private Dialogue chosenDialogue;
    public float Magnitude = 1f;
    public float Roughness = 5f;
    public float FadeOutTime = 2f;
    public ParticleSystem emojis;
    public ParticleSystemRenderer emojiRenderer;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        ballTextMesh = GetComponentInChildren<TextMesh>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ai = gameManager.AI.GetComponent<AI>();
    }

    private void Update()
    {
        if (randomizeRange)
        {
            randomizeRange = false;
            do
            {
                GetRandom();
            }
            while (returnRange > -insideRange && returnRange < insideRange);
        }
        // Manual launch for ball.
        /*if (Input.GetButtonDown("Jump") && !isLaunch)
        {
            isLaunch = true;

            Vector2 launchDirection = new Vector2(1.0f, -0.25f);
            // Launch velocity.
            rb2d.velocity = launchDirection * speed;
        }
        else if (!isLaunch)
        {
            transform.position = gameManager.player.transform.position + new Vector3(2.75f, -0.5f, 0.0f);
        }
        */
        // Increase speed over time.
        if (isLaunch && !gameManager.stopGame)
        {
            if (timeCounterToIncreaseSpeed < timeToIncreaseSpeed)
            {
                timeCounterToIncreaseSpeed += Time.deltaTime;
            }
            else
            {
                timeCounterToIncreaseSpeed = 0.0f;
                speed += 1.0f;
            }
        }
        //BallScale.transform.localScale  =new Vector3 (4+rb2d.velocity.x/10, 4+rb2d.velocity.y / 10,0);
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collideParticle.Play();

        // 'collision' holds the collision information.
        if (collision.gameObject.tag == "Player")
        {
            audioManager.Play("MessengerTyping");

            // Calculate hit factor.
            float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);

            // Calculate direction, normalized.
            Vector2 direction = new Vector2(1.0f, y).normalized;
            gameManager.UpdateBallDialogue();
            ballTextMesh.text = "";

            // Set new velocity with direction * speed.
            rb2d.velocity = direction * gameManager.ballspeed;
            ai.speed = ai.speed + Random.Range(-5, 5);
            randomizeRange = true;
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
            hitPlayer = true;
        
        }

        if (collision.gameObject.tag == "AI")
        {
            audioManager.Play("MessengerSound");

            if (!justhit)
            {
                float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);

                Vector2 direction = new Vector2(-1.0f, y).normalized;
                ballTextMesh.text = chosenDialogue.replyFromCrush;
                emojiRenderer.material = chosenDialogue.replyEmoji;
                if (emojiRenderer.material != null)
                {
                    emojis.Play();
                }
                rb2d.velocity = direction * gameManager.ballbackspeed;
                gameManager.balllevelnumber += 1;
                gameManager.HideAllDialogue();
                gameManager.UpdateImpressionBar(chosenDialogue.impressionPoint);
                if (!(chosenDialogue.m_branchingDialogues.Length > 0)) // check if there's any remaining dialogue.
                    StartCoroutine(gameManager.TransitionToGOPanel(gameManager.fzonePanel)); // friend zone result.
                else
                    dialoguemanager.HideTriggeredTBox();
                Debug.Log(gameManager.balllevelnumber);
                gameManager.HideAllDialogue();
                gameManager.UpdateLevelShowing();
                justhit = true;
                Invoke("JustHitNot", 1.5f);
                CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
            }
        }

        if (collision.gameObject.tag == "WallTop" || collision.gameObject.tag == "WallBottom")
        {
            audioManager.Play("FacebookChat");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Die") // wall left is tagged "Die"
        {
            // Game over
            StartCoroutine(gameManager.TransitionToGOPanel(gameManager.diePanel));
        }

        if (hitPlayer)
        {
            TextBox tbox = other.GetComponent<TextBox>();
            if (tbox) // if the collider we have exit is a text box.
            {
                audioManager.Play("iMessageSend");
                
                chosenDialogue = tbox.m_dialogue;
                dialoguemanager.SetTriggeredDialogue(chosenDialogue);
                hitPlayer = false;
            }
        }
        
    }

    public float GetRandom()
    {
        return returnRange = Random.Range(-range, range);
    }
    void ShowReplies()
    {
        gameManager.levelnumber += 1;
        gameManager.HideAllDialogue();
        gameManager.UpdateBallDialogue();
    }
    void JustHitNot()
    {
        if (chosenDialogue.m_branchingDialogues.Length > 0)
            dialoguemanager.SetupTextBoxes();
        justhit = false;
    }

    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Is the game over?
        //if (!gameManager.stopGame)
        //{
        //    // Collision to cause player lose.
        //    if (collision.gameObject.tag == "WallLeft")
        //    {
        //        uiManager.loseText.enabled = true;
        //        gameManager.stopGame = true;
        //        StopBall();
        //    }
        //    // Collision to cause player win.
        //    if (collision.gameObject.tag == "WallRight")
        //    {
        //        uiManager.winText.enabled = true;
        //        gameManager.stopGame = true;
        //        StopBall();
        //    }
        //}
    }
    */

    float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return ((ballPos.y - racketPos.y) / racketHeight);
    }

    public void StopBall()
    {
        speed = 0.0f;
        timeCounterToIncreaseSpeed = 0.0f;
        rb2d.velocity = Vector2.zero;
        ai.StopAI();
    }
}
