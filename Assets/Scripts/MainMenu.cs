using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame(){
        Debug.Log("Button Clicked1!");
        SceneManager.LoadScene("Playground");
    }

    public void ExitGame(){
        Debug.Log("Button ClickedExit!");
        Application.Quit();
    }
}
