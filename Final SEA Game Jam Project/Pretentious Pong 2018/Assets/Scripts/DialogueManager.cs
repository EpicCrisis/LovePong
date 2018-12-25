using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    [HideInInspector] public float m_pixelPerUnit;
    [SerializeField] Dialogue m_curDialogue; // triggered dialogue (and is branching out...)
    [SerializeField] TextBox m_topTextBox;
    [SerializeField] TextBox m_btmTextBox;
    [SerializeField] TextBox m_triggeredTBox;

    void Start ()
    {
        // Find out how many pixels are there in one unity world unit.
        m_pixelPerUnit = Screen.height / (Camera.main.orthographicSize * 2);
        m_topTextBox.m_dialogue = m_curDialogue;
        m_btmTextBox.m_dialogue = m_curDialogue;
    }

    // called in OnTriggerEnter of ball
    public void SetTriggeredDialogue(Dialogue dialogue)
    {
        // find out the untriggered text box dialogue and fades it off.
        TextBox untriggeredTBox = dialogue == m_topTextBox.m_dialogue ? m_btmTextBox : m_topTextBox;
        untriggeredTBox.StartCoroutine("TextFadesOut");
        m_triggeredTBox = untriggeredTBox == m_topTextBox ? m_btmTextBox : m_topTextBox;
        m_curDialogue = dialogue;
    }

    // set text boxes position, scaling and text.
    public void SetupTextBoxes()
    {
        float verticalScale = Camera.main.orthographicSize * m_curDialogue.heightRatio; // scale along y-axis of top text box.
        m_topTextBox.SetScale(new Vector3(m_topTextBox.transform.localScale.x, verticalScale, m_topTextBox.transform.localScale.z));
        m_topTextBox.SetPos(new Vector3(m_topTextBox.transform.position.x, Camera.main.orthographicSize - (verticalScale / 2), m_topTextBox.transform.position.z));
        m_topTextBox.SetText(m_curDialogue.m_branchingDialogues[0].m_text);

        verticalScale = (Camera.main.orthographicSize * 2) - verticalScale;
        m_btmTextBox.SetScale(new Vector3(m_topTextBox.transform.localScale.x, verticalScale, m_topTextBox.transform.localScale.z));
        m_btmTextBox.SetPos(new Vector3(m_topTextBox.transform.position.x, -Camera.main.orthographicSize + (verticalScale / 2), m_topTextBox.transform.position.z));
        m_btmTextBox.SetText(m_curDialogue.m_branchingDialogues[1].m_text);

        // Random placement to prevent m_branchingDialogues[0] (which is positive) from getting placed at the upper area every time.
        int zeroOrOne = Random.Range(0, 1);
        m_topTextBox.m_dialogue = m_curDialogue.m_branchingDialogues[zeroOrOne];
        m_btmTextBox.m_dialogue = m_curDialogue.m_branchingDialogues[1 - zeroOrOne];
    }
    
    public void HideTriggeredTBox()
    {
        m_triggeredTBox.StartCoroutine("TextFadesOut");
    }
}
