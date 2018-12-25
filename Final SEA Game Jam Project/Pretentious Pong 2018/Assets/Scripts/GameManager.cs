using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
//using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;
    public bool stopGame;

    public GameObject AI;
    public GameObject ball;
    public GameObject player;

    private AI AIComp;
    private Ball ballComp;
    private Player playerComp;
    private AudioManager audioManager;

    public int gamePhase;

    public int balllevelnumber = 0;
    public int levelnumber = 0;
    public GameObject[] level;
    private Rigidbody2D rb2d;
    public float ballspeed = 30.0f;
    public float ballbackspeed = 30.0f;
    public GameObject[] balldialogue;
    public Text levelint;
    public Text ballint;
    [SerializeField] Slider impressionBar;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject winPanel;
    public GameObject fzonePanel;
    public GameObject diePanel; // disconnected
    [SerializeField] TextBox m_topTextBox;
    [SerializeField] TextBox m_btmTextBox;
    [SerializeField] SceneFader sceneFader;
    [SerializeField] GameObject hand;
    WaitForSeconds transitionDelay = new WaitForSeconds(1.0f);

    private void Awake()
    {
        HideAllDialogue();
        AIComp = AI.GetComponent<AI>();
        ballComp = ball.GetComponent<Ball>();
        playerComp = player.GetComponent<Player>();
        rb2d = ballComp.GetComponent<Rigidbody2D>();
        audioManager = FindObjectOfType<AudioManager>();
        sceneFader.gameObject.SetActive(true);
    }

    public void ReallyStartGame()
    {
        stopGame = false;
        LaunchBall();
    }

    public void StartGame()
    {
        audioManager.Play("iMessageSend");
        audioManager.Stop("HeartBeat");

        audioManager.Play("FollowHer");
        UpdateBallDialogue();
        levelnumber += 1;
        balllevelnumber += 1;
        hand.GetComponent<Animator>().Play("HandWavesOut");
        impressionBar.gameObject.SetActive(true);
        impressionBar.GetComponent<Animator>().Play("BarFadesIn");
        CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
        HideAllLevels();
        Invoke("ReallyStartGame", 1f);
    }

    public void RestartGame()
    {
        StartCoroutine(sceneFader.FadeToScene(1));
    }

    public void LaunchBall()
    {
        ballComp.isLaunch = true;
        Vector2 launchDirection = new Vector2(1.0f, 0f);
        rb2d.velocity = launchDirection * ballspeed;
    }

    public void HideAllLevels()
    {
        foreach (GameObject levelshowing in level)
        {
            levelshowing.SetActive(false);
        }
    }

    public void HideAllDialogue()
    {
        balldialogue[0].SetActive(false);
    }
    public void UpdateBallDialogue()
    {
        //balldialogue[balllevelnumber].SetActive(true);
        if(balllevelnumber == 0) balldialogue[0].SetActive(true);
        else balldialogue[1].SetActive(true);
    }
    public void UpdateLevelShowing()
    {
        //level[levelnumber].SetActive(true);
    }
    private void Update()
    {
        if (levelnumber > 0) levelint.text = "Level " + levelnumber;
        else levelint.text = "";
        ballint.text = "Ball " + balllevelnumber;
    }

    public void UpdateImpressionBar(int receivedPoint)
    {
        impressionBar.value = impressionBar.value + receivedPoint;

        if (impressionBar.value <= 0)
        {
            // Lose condition 1 - Being blocked
            StartCoroutine(TransitionToGOPanel(losePanel));
        }
        else if (impressionBar.value >= impressionBar.maxValue)
        {
            // Win condition - Be together
            StartCoroutine(TransitionToGOPanel(winPanel));
        }

        /*
        if (balllevelnumber > 20)
        {
            // They had 20 conversation but the crush isn't impressed.
            StartCoroutine(TransitionToGOPanel(NormPanel));
        }
        */
    }

    void HideHUD()
    {
        impressionBar.gameObject.SetActive(false);
        m_topTextBox.gameObject.SetActive(false);
        m_btmTextBox.gameObject.SetActive(false);
    }

    // Fades out and in to one of the game over panel. Called in ball too, when the ball hit the left wall.
    public IEnumerator TransitionToGOPanel(GameObject panel)
    {
        HideHUD();
        ballComp.StopBall();
        audioManager.Stop("FollowHer");
        StartCoroutine(sceneFader.FadeOutAndIn());
        yield return transitionDelay;
        if (panel == diePanel)
            audioManager.Play("Disconnect");
        if (panel == losePanel)
            audioManager.Play("SoundOff");
        if (panel == winPanel)
            audioManager.Play("Connect");
        panel.SetActive(true);
    }
}
