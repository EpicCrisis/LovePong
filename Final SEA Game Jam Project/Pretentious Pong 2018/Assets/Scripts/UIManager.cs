using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Text loseText;
    public Text winText;

    void Start()
    {
        loseText.enabled = false;
        winText.enabled = false;
    }

    void Update()
    {

    }
}
