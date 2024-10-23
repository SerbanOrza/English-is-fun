using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class NavigateManager : MonoBehaviour
{
    public static NavigateManager Instance;
    // Start is called before the first frame update
    public GameObject readyImage,defeatPanel;
    public GameObject[] panels;
    public TMP_Text yourScore;
    public int currentPanel;
    void Awake()
    {
        Instance=this;
    }
    public void ApasatReady()
    {
        readyImage.SetActive(false);
        TheManager.Instance.SelectRandomVerb();
    }
    public void goHome()
    {
       // yourScore.text=TheManager.Instance.punctajText.text;
        defeatPanel.SetActive(true);
        TheManager.Instance.verifyButton.SetActive(false);
        TheManager.Instance.exitButton.SetActive(false);
        TheManager.Instance.nextButton.SetActive(false);
        TheManager.Instance.correctMesaj.SetActive(false);
        TheManager.Instance.incorrectMesaj.SetActive(false);
        //goToPanel(0);
    }
    public void ApasatStartGame()
    {
        Debug.Log("apasatStartGame");
        readyImage.SetActive(true);
    }
    public void ApasatExitDupaCongrat()
    {
        defeatPanel.SetActive(false);
        TheManager.Instance.ExitGame();
    }
    public void goToPanel(int k)
    {
        panels[currentPanel].SetActive(false);
        panels[k].SetActive(true);
        currentPanel=k;
    }
}
