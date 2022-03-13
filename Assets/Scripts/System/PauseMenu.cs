using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject controlPanel;
    private bool isPause;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPause = false;
    }

    public void MainButton()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPause = false;
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Debug.Log("Exit !!!");
        Application.Quit();
    }

    public void ExitControlMenu()
    {
        pausePanel.SetActive(true);
        controlPanel.SetActive(false);
    }

    public void ControlMenu()
    {
        pausePanel.SetActive(false);
        controlPanel.SetActive(true);
    }
}
