using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Button startButton;
    public Button startRandomButton;
    public Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        startRandomButton.onClick.AddListener(StartRandomGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    private void StartRandomGame()
    {
        SceneManager.LoadScene(1);

    }
    private void ExitGame()
    {
        Application.Quit();
    }
}
