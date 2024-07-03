using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyExit : MonoBehaviour
{
    [SerializeField] GameObject objExit;
    [SerializeField] GameObject objMain;
    [SerializeField] GameObject[] panels;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ButtonExit();
    }


    private void DisableAll()
    {
        foreach (GameObject obj in panels) obj.SetActive(false);
        objMain.SetActive(true);
    }


    public void ButtonExit()
    {
        if (objExit.activeInHierarchy)
        {
            DisableAll();
            objExit.SetActive(false);
            return;
        }

        foreach (GameObject obj in panels)
        {
            if (obj.activeInHierarchy)
            {
                DisableAll();
                return;
            }
        }

        objExit.SetActive(true);
    }

    public void ButtonYes()
    {
        Application.Quit();
    }
}