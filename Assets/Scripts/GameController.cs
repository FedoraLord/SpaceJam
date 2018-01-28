using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject pulse;
    public GameObject stunBolt;

    private int remainingObjectives;

    private void Start()
    {
        remainingObjectives = GameObject.FindGameObjectsWithTag("Goal").Where(x => x.GetComponent<Objective>().IsActive).Count();
    }

    public void ObjectiveReached(Objective obj)
    {
        obj.IsActive = false;
        remainingObjectives--;
        if (remainingObjectives == 0)
        {
            OpenDaWae();
        }
    }

    private void OpenDaWae()
    {
        //remove block
        var objectives = GameObject.FindGameObjectsWithTag("Goal");
        foreach (var item in objectives)
        {
            item.GetComponent<Objective>().IsActive = true;
        }
    }

    internal void Lose()
    {
        throw new NotImplementedException();
    }
}
