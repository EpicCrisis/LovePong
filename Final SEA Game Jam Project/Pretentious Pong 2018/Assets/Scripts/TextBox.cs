using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour {

    [SerializeField] bool m_scaleText; // to determine whether the text label scales (changes in font size) as this object scale.
    [SerializeField] string m_textToDisplay;
    Rect m_textBox;
    [SerializeField] GUIStyle m_textStyle;
    int m_textSize; // default (original) font size defined in GUIStyle.
    Collider2D m_collider;

    [SerializeField] DialogueManager m_dialogueManager;
    [HideInInspector] public Dialogue m_dialogue; // store the dialogue object it is "representing"
    private float Seconds=1;
    WaitForSeconds loseTime = new WaitForSeconds(0.001f);

    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        m_textSize = m_textStyle.fontSize;
    }
    

    public void SetPos(Vector3 _worldPos)
    {
        transform.position = _worldPos;

        Vector3 curScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        m_textBox.x = curScreenPos.x - (m_textBox.width / 2);
        // invert the y coordinates bcoz OnGUI draw from the top left of screen
        m_textBox.y = Screen.height - curScreenPos.y - (m_textBox.height / 2);
    }

    public void SetScale(Vector3 _localScale)
    {
        transform.localScale = _localScale;

        m_textBox.width = m_collider.bounds.size.x * m_dialogueManager.m_pixelPerUnit;
        m_textBox.height = m_collider.bounds.size.y * m_dialogueManager.m_pixelPerUnit;

        Vector3 curScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        m_textBox.x = curScreenPos.x - (m_textBox.width / 2);
        // invert the y coordinates bcoz OnGUI draw from the top left of screen
        m_textBox.y = Screen.height - curScreenPos.y - (m_textBox.height / 2);

        if (m_scaleText)
            m_textStyle.fontSize = Mathf.FloorToInt(m_textSize * (transform.localScale.x + transform.localScale.y) / 2);
        m_textStyle.fontSize = 0;
       m_textSize = m_textStyle.fontSize;
        StartCoroutine("LoseTime");
    }

    public void SetText(string newText)
    {
        m_textToDisplay = newText;
    }

	void OnGUI()
    {
        GUI.Label(m_textBox,
            m_textToDisplay,
            m_textStyle);
    }

    void CreateSolidFont()
    {

    }
    private void Update()
    {
        if(Seconds<=0)
        {
            Debug.Log("Stop courutine");
            Seconds = 1;
            StopCoroutine("LoseTime");
        }
    }

    IEnumerator LoseTime()
    {
        // restore faded text.
        Color c = m_textStyle.normal.textColor;
        c.a = 1.0f;
        m_textStyle.normal.textColor = c;

        while (true)
        {
            yield return null;

            m_textSize = m_textStyle.fontSize+=1;
            Seconds-= 0.03333333333f;
        }
    }

    IEnumerator TextFadesOut()
    {
        float fadesOutDuration = 0.8f, countDown = fadesOutDuration, weight;
        while (countDown > 0.0f)
        {
            weight = countDown * countDown * countDown;
            Color c = m_textStyle.normal.textColor;
            c.a = Mathf.Lerp(0.0f, 1.0f, weight);
            m_textStyle.normal.textColor = c;
            countDown -= Time.deltaTime;
            yield return null;
        }
    }
}