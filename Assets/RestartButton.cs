using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button button;


    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    private void  Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
