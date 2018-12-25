using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// container of data
public class Dialogue : MonoBehaviour {
    public int m_id;
    public string m_text;
    public int impressionPoint = 1; // -1 for negative impression, 1 for positive impression, 0 for neutral impression.
    public float heightRatio; // the vertical ratio of the two dialogues below.
    public Material replyEmoji = null;
    public string replyFromCrush;
    public Dialogue[] m_branchingDialogues; // point to the next two dialogues that is going to show up, subsequent to this dialogue.
}
