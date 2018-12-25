using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject menu;
    public GameObject credits;
    private void Start()
    {
        Screen.SetResolution(1600, 720, false);
    }
    public void Level1()
    {
        SceneManager.LoadScene("main");
    }
    public void Tocredits()
    {
        menu.SetActive(false);
    }
    public void ToMenu()
    {
        menu.SetActive(true);
    }
}
