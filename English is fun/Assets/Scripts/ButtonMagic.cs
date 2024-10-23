using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonMagic : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text continut;
    public Color culoareDefault;
    public Image SelectedFundal;
    public bool selected;
    public int id;
    public void SelectThis()
    {
        if(TheManager.Instance.seScurgeTimpul==false)
            return;
        int k=TheManager.Instance.solPlayer;
        if(selected==true)
        {
            k=k-(1<<id);
            selected=false;
            SelectedFundal.color=new Color32(255,255,255,0);
        }
        else
        {
            k=k+(1<<id);
            selected=true;
            SelectedFundal.color=new Color32(255,255,255,100);
        }
        TheManager.Instance.solPlayer=k;
    }
    public void SchimbaCuloarea(Color culoareaNoua)
    {
        gameObject.GetComponent<Image>().color=culoareaNoua;
    }
    public void RestartButton()
    {
        gameObject.GetComponent<Image>().color=culoareDefault;
        selected=false;
        SelectedFundal.color=new Color32(255,255,255,0);
    }
}
