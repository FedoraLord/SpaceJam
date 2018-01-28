using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject pulse;
    public GameObject stunBolt;

    private int remainingObjectives;

    private void Start()
    {
        remainingObjectives = GameObject.FindGameObjectsWithTag("Energy").Count();
    }

    public void ObjectiveReached()
    {
        remainingObjectives--;
        if (remainingObjectives == 0)
        {
            OpenDaWae();
        }
    }

    private void OpenDaWae()
    {
        //remove block
        //var objectives = GameObject.FindGameObjectsWithTag("Goal");
        //foreach (var item in objectives)
        //{
        //    item.GetComponent<Objective>().IsActive = true;
        //}
        Win();
    }

    public void Lose()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
}
