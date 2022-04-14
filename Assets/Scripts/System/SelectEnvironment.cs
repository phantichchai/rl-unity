using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SelectEnvironment : MonoBehaviour
{
    public void SelectState(int stateNumber)
    {
        if (stateNumber == 0)
        {
            DataSystem.Instance().State = State.Battle;
        }
        else if (stateNumber == 1)
        {
            DataSystem.Instance().State = State.CollectItem;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
