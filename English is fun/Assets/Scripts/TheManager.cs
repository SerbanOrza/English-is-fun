using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;

public class TheManager : MonoBehaviour
{
    public static TheManager Instance;
    public List<ButtonMagic> answers=new List<ButtonMagic>();
    [Serializable]
    public class Verb
    {
        public string name;
        public int nrPrep;
        public List<string> prepositions=new List<string>();
    }
    public List<Verb> verbe=new List<Verb>();
    public float maxPunctaj,punctaj,timpDeRaspuns=5;
    public TMP_Text maxPunctajText,punctajText,verbul;
    public Gradient gradient;
    void Awake()
    {
        Instance=this;
    }
    void Start()
    {
        //load maxpunctaj...
        LoadData();
        maxPunctajText.text="Best score: "+maxPunctaj.ToString();
    }
    int previosVerbId=-1,idButonBun=0;
    public bool seScurgeTimpul=false,aRaspuns=false;
    public Image left,right;
    public Color red,green;
    public GameObject exitButton,verifyButton,nextButton,correctMesaj,incorrectMesaj;
    public int sol,solPlayer;
    public IEnumerator TimpulTrece(float durata)
    {
        seScurgeTimpul=true;
        float i=1;
        left.fillAmount=1;
        right.fillAmount=1;
        for(float t=durata;t>=0;t=t-Time.deltaTime)
        {
            i=i-Time.deltaTime/durata;
            left.fillAmount=i;
            left.color=gradient.Evaluate(1-i);
            right.fillAmount=i;
            right.color=gradient.Evaluate(1-i);
            yield return null;
            if(aRaspuns==true)
            {
                addPunctDacaE();
                StartCoroutine(afisRaspunsuri());
                //timp cand poate apasa next
                //yield return new WaitForSeconds(0.7f);
                nextButton.SetActive(true);
                seScurgeTimpul=false;
                yield break;
            }
        }
        left.fillAmount=0;
        right.fillAmount=0;
        addPunctDacaE();
        StartCoroutine(afisRaspunsuri());
        //timp cand poate apasa next
        //yield return new WaitForSeconds(0.7f);
        nextButton.SetActive(true);
        seScurgeTimpul=false;
        yield return null;
    }
    public void addPunctDacaE()
    {
        if(sol!=solPlayer)//a gresit
        {
            punctaj--;
            incorrectMesaj.SetActive(true);
        }
        else//a raspuns corect
        {
            punctaj++;
            correctMesaj.SetActive(true);
        }
        punctajText.text=punctaj.ToString();
        NavigateManager.Instance.yourScore.text=punctaj.ToString();
    }
    public IEnumerator afisRaspunsuri()
    {
        verifyButton.SetActive(false);
        exitButton.SetActive(true);
        //nextButton.SetActive(true);
        foreach(ButtonMagic b in answers)
        {
            if((sol&(1<<b.id))!=0)
                b.SchimbaCuloarea(green);
            else
                b.SchimbaCuloarea(red);
        }
        yield return null;
    }
    public void EndIntrebare()//cand apesi butonul ca vrei sa opresti timerul
    {
        aRaspuns=true;
    }
    public void ExitGame()//reset punctaj
    {
        //verifica cu best score etc... apoi
        
        verifyButton.SetActive(false);
        exitButton.SetActive(false);
        nextButton.SetActive(false);
        if(punctaj>maxPunctaj)
        {
            maxPunctaj=punctaj;
            maxPunctajText.text="Best score: "+maxPunctaj.ToString();
            //save maxPunctaj
            SaveData();
        }
        punctaj=0;
        punctajText.text=punctaj.ToString();
        NavigateManager.Instance.yourScore.text=punctaj.ToString();
        //reset barile de stres
        
        left.fillAmount=1;
        left.color=gradient.Evaluate(0);
        right.fillAmount=1;
        right.color=gradient.Evaluate(0);
    }
    public void SelectRandomVerb()
    {
        if(seScurgeTimpul==true)
            return;
        //timpDeRaspuns-=0.1f;
        correctMesaj.SetActive(false);
        incorrectMesaj.SetActive(false);
        verifyButton.SetActive(true);
        exitButton.SetActive(false);
        nextButton.SetActive(false);
        aRaspuns=false;
        foreach(ButtonMagic b in answers)
            b.RestartButton();
        int k=UnityEngine.Random.Range(0,verbe.Count);
        while(k==previosVerbId)
        {
            k=UnityEngine.Random.Range(0,answers.Count);
        }
        previosVerbId=k;
        Debug.Log(k);
        verbul.text=verbe[k].name;
        allprepInCreare.Clear();
        foreach(string s in allprep)
        {
            if(isInLista(s,k)==false)
                allprepInCreare.Add(s);
        }
        foreach(ButtonMagic b in answers)
        {
            b.continut.text=getRandomPrep();
        }
        //pune raspunsurile in butoane
        int i,poz,nr=verbe[k].prepositions.Count;
        idButonBun=UnityEngine.Random.Range(0,4);
        sol=0;
        solPlayer=0;
        //reset listapozitii
        listaPoz.Clear();
        for(i=0;i<answers.Count;i++)
            listaPoz.Add(i);
        for(i=0;i<nr;i++)
        {
            poz=getPozRandom();//0->nrButoane;
            sol=(sol+(1<<poz));
            answers[poz].continut.text=verbe[k].prepositions[i];
        }
        StartCoroutine(TimpulTrece(timpDeRaspuns));

    }
    bool isInLista(string s,int k)
    {
        foreach(string prep in verbe[k].prepositions)
        if(s==prep)
            return true;
        return false;
    }
    public List<int> listaPoz=new List<int>();
    public int getPozRandom()
    {
        int k=UnityEngine.Random.Range(0,listaPoz.Count),val;
        val=listaPoz[k];
        listaPoz.RemoveAt(k);
        return val;
    }
    public List<string> allprep=new List<string>();
    public List<string> allprepInCreare=new List<string>();
    public string getRandomPrep()
    {
        int k=UnityEngine.Random.Range(0,allprepInCreare.Count);
        string s=allprepInCreare[k];
        allprepInCreare.RemoveAt(k);
        return s;
    }

    public void VerificaPrep(int id)
    {
        if(seScurgeTimpul==false)//a raspuns cand nu se rula jocul
            return;
        if(aRaspuns==true)//a raspuns deja
           return;
        //aRaspuns=true;
    }
    public TMP_InputField inputverb;
    public TMP_Text verbtyped,prep;
    public Verb verbDePus;
    public void AddPrep(string s)
    {
        verbDePus.prepositions.Add(s);
        prep.text=prep.text+" "+s;
    }
    public void clear()
    {
        prep.text="";
        inputverb.text="";
        verbtyped.text=inputverb.text;
        verbDePus=null;
    }
    public void AddVerb()
    {
        verbDePus.nrPrep=verbDePus.prepositions.Count;
        verbDePus.name=inputverb.text;
        Verb vb=new Verb();
        vb=verbDePus;
        verbe.Add(verbDePus);
        clear();
    }

    // Update is called once per frame
    void Update()
    {
        verbtyped.text=inputverb.text;
    }
    public void ResetBestScore()
    {
        maxPunctaj=0;
        maxPunctajText.text="Best score: "+maxPunctaj.ToString();
        SaveData();
    }
    public void ExitAplicatie()
    {
        Application.Quit();
    }
    // Save data to a file
    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.dat");
        bf.Serialize(file, maxPunctaj);
        file.Close();
    }

    // Load data from a file
    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/savedData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.dat", FileMode.Open);
            maxPunctaj = (float)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            maxPunctaj = 0;
        }
    }
}
