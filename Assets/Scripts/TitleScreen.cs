using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public GameObject StartButton;

	// Use this for initialization
	void Start () {
        Invoke("ShowButton", 15);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowButton()
    {
        StartButton.SetActive(true);
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
