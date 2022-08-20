using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;


public class readFile : MonoBehaviour
{
    public const string path = @"D:\Home\Nhi\Documents\REU_Summer2022\REU_Summer2022_Project\Assets\StreamingAssets";
    public string fullPath = Path.Combine(path);
    string dataFile;
    string temp;
    List<string> listCombine = new List<string>();
    public GameObject refMain;
    public GameObject refTXT;
    public GameObject refCSV;
    public GameObject refJSON;
    private Text bounce;
    public bool bounceBool = true;
    private Text details;
    public string score;
    private Text highscore;
    private TimeSpan t;
    //public string highscore;

    public class catClass
    {
        public string TotalBounces {get; set;} // totalBounce string;
        public string Catch1 {get; set;}//catLabelList string;
        public string ElapsedTime1 {get; set;} //catDataList string;
        public string Catch2 {get; set;} //catLabelList string;
        public string ElapsedTime2 {get; set;} //catDataList string;
        public string Catch3 {get; set;} //catLabelList string;
        public string ElapsedTime3 {get; set;} //catDataList string;
        public string Catch4 {get; set;} //catLabelList string;
        public string ElapsedTime4 {get; set;} //catDataList string;
        public string Catch5 {get; set;} //catLabelList string;
        public string ElapsedTime5 {get; set;} //catDataList string;
    }
    
    //Challenge 9, View MinimumWindowSize.cs script!!!!
    int minWidth = 950;
    int minHeight = 600;
    
    // Start is called before the first frame update
    void Start()
    {       
        //Challenge 9
        MinimumWindowSize.Set(minWidth, minHeight);

        refMain = GameObject.Find("CanvasTXT").gameObject;
        if(refMain.activeSelf == false){refMain.SetActive(true);}
        refTXT = GameObject.Find("CanvasTXT").gameObject;
        refTXT.SetActive(false);
        refCSV = GameObject.Find("CanvasCSV").gameObject;
        refCSV.SetActive(false);
        refJSON = GameObject.Find("CanvasJSON").gameObject;
        refJSON.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // -------------WINDOW RESIZING------------------ //Challenge 9
        // ---------------------------------------------- // View MinimumWindowSize.cs script!!!!
        MinimumWindowSize.Set(minWidth, minHeight);
    }

    public void readTXT()
    {
        refMain.SetActive(false);
        refTXT.SetActive(true);
        dataFile = fullPath +  @"\dataFileTXT.txt";
        // --------------------------------------------------------------------------------------
        // REFERENCING CODE FROM:??????????????????????????????????????????????????????????????
        // https://zetcode.com/csharp/readtext/
        // --------------------------------------------------------------------------------------
        var enumLines = File.ReadLines(dataFile, Encoding.UTF8);
        foreach (var line in enumLines)
        {
            if(line.Equals('\n'))
            {break;}
            if(bounceBool == true)
            {
                bounce = refTXT.transform.GetChild(0).GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>();
                bounce.text = line;
                bounceBool = false;
            }
            else{temp += line + "\n";}
        }
        
        details = refTXT.transform.GetChild(0).GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>();
        details.text = temp;

        calculateHighScore(refTXT, getScore(temp, score));
    }

    public void readCSV()
    {
        refMain.SetActive(false);
        refCSV.SetActive(true);
        dataFile = fullPath + @"\dataFileCSV.csv";
    
        List<string> listA = new List<string>();
        List<string> listB = new List<string>();

        // ---------------------------------------------------------------------------------------------
        // REFERENCING CODE FROM:??????????????????????????????????????????????????????????????
        // https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
        //----------------------------------------------------------------------------------------------
        using(var sr = new StreamReader(dataFile, false))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                //print(line);
                if(line.Equals('\n'))
                {break;}
                string[] values = line.Split(',');
                // values[0] = values[0].TrimEnd();
                listA.Add(values[0]);
                listB.Add(values[1]);
                listCombine.Add(listA.Last() + listB.Last());
                if(bounceBool == true)
                {
                    bounce = refCSV.transform.GetChild(0).GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>();
                    bounce.text = listCombine.Last();
                    bounceBool = false;
                }
                else{temp += listCombine.Last() + "\n";}
            }
        }
        // Debug.Log("Printing ListA:");
        // for(int i = 0; i < listA.Count; i++){Debug.Log(listA[i]);}
        // Debug.Log("Printing ListB:");
        // for(int i = 0; i < listB.Count; i++){Debug.Log(listB[i]);}
        listA.Clear();
        listB.Clear();
        details = refCSV.transform.GetChild(0).GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>();
        details.text = temp;

        calculateHighScore(refCSV, getScore(temp, score));

        listCombine.Clear();
    }

    public void readJSON()
    {
        refMain.SetActive(false);
        refJSON.SetActive(true);
        // GameObject highScoreObject = GameObject.Find("ScoreText");
        // highScoreObject.SetActive(false);
        dataFile = fullPath +  @"\dataFileJSON.json";

        string jsonString = File.ReadAllText(dataFile, Encoding.UTF8);
        // Debug.Log(jsonString);
        catClass ccRead = JsonConvert.DeserializeObject<catClass>(jsonString);
        PropertyInfo[] properties = typeof(catClass).GetProperties();
        foreach(PropertyInfo property in properties)
        {
            // Debug.Log(property.GetValue(ccRead, null));
            if(bounceBool == true)
            {
                bounce = refJSON.transform.GetChild(0).GetChild(1).gameObject.GetComponent<UnityEngine.UI.Text>();
                bounce.text = property.Name + ": " + (property.GetValue(ccRead, null)).ToString();
                bounceBool = false;
            }
            else{temp += property.Name + ": " + (property.GetValue(ccRead, null)).ToString() + "\n";}
        }

        details = refJSON.transform.GetChild(0).GetChild(2).gameObject.GetComponent<UnityEngine.UI.Text>();
        details.text = temp;

        calculateHighScore(refJSON, getScore(temp, score));
    }

    public TimeSpan getScore(string temp_, string score_)
    {
        int i = temp_.Length - 1;
        do
        {
            score_ += temp_[i];
            //Debug.Log(score_);
            i--;

        } while (i > (temp_.Length - 7));
        // --------------------------------------------------------------------------------------
        // REFERENCING CODE FROM:??????????????????????????????????????????????????????????????
        // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string/15111719#15111719
        // --------------------------------------------------------------------------------------
        char[] array = score_.ToCharArray();
        Array.Reverse(array);
        score_ = new string(array);
        score_ = score_.TrimEnd();
        // Debug.Log(score);
        //Debug.Log(score.Length);
        // for(int j = 0; j < score.Length; j++){Debug.Log(score[j]);}
        int temp1 = Int32.Parse((score_[1]).ToString());
        int temp2 = Int32.Parse((score_[3].ToString() + score_[4].ToString()).ToString());
        t = new TimeSpan(0, temp1, temp2);
        //Debug.Log(t);

        return t;
    }

    public void calculateHighScore(GameObject ref_, TimeSpan t_)
    {
        //Debug.Log("call calc");
        if(!PlayerPrefs.HasKey("prevTime")){PlayerPrefs.SetString("prevTime", "00:01:11");}

        string pt = PlayerPrefs.GetString("prevTime");
        int temp1 = Int32.Parse((pt[4]).ToString());
        int temp2 = Int32.Parse((pt[6].ToString() + pt[7].ToString()).ToString());
        TimeSpan prevTimeSpan = new TimeSpan(0, temp1, temp2);
        //Debug.Log("Prev Time: " + prevTimeSpan);

        highscore = ref_.transform.GetChild(0).GetChild(3).gameObject.GetComponent<UnityEngine.UI.Text>();

        if(t_ < prevTimeSpan)
        {
            highscore.text = "Fastest Time: " + "\n" + "*" + t_.ToString();
            //Debug.Log("Changing");
            PlayerPrefs.SetString("prevTime", t_.ToString());
            PlayerPrefs.Save();
        }
        else
        {
            highscore.text = "Fastest Time: " + "\n" + prevTimeSpan.ToString();
            //Debug.Log("Not Changing");
        }
    }

    public void returnFileRead()
    {
        if(listCombine.Count != 0){listCombine.Clear();}
        SceneManager.LoadScene("fileReader");
        //PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit() //Challenge 9, View MinimumWindowSize.cs script!!!!
    {
        MinimumWindowSize.Reset();
    }
}