using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMode : MonoBehaviour
{
    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void AgentOnlyButton()
    {
        DataSystem.Instance().Mode = Mode.AgentOnly;
        NextScene();
    }

    public void CollectorTeamButton()
    {
        DataSystem.Instance().Mode = Mode.Collector;
        NextScene();
    }
    public void DisruptorTeamButton()
    {
        DataSystem.Instance().Mode = Mode.Disruptor;
        NextScene();
    }

    public void NextScene()
    {
        if (DataSystem.Instance().State == State.Battle)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (DataSystem.Instance().State == State.CollectItem)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}