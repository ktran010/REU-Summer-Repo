// --------------------------------------------------------------------------------------
// REFERENCING CODE FOR writeToFile() FUNCTION:
// https://www.youtube.com/watch?v=iFJeg9AzN2Y&ab_channel=PrefixWiz
// ---------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public class writeFile : MonoBehaviour
{
    public string dataFile;
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

    // ---------------------------------------------------------------------------------=-----
    // REFERENCING CODE FROM:
    // https://stackoverflow.com/questions/70715187/unity-read-and-write-to-txt-file-after-build
    // -----------------------------------------------------------------------------------------
    public const string BaseFolder = "StreamingAssets";
    public static string getBasePath(){
    #if UNITY_EDITOR
        string path = Application.dataPath + $"/{BaseFolder}/" ;
        string path1 = Application.dataPath + $"/{BaseFolder}";
        if (!Directory.Exists(path1)) Directory.CreateDirectory(path1);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    #elif UNITY_ANDROID
        string path = Application.persistentDataPath + $"/{BaseFolder}/";
        string path1 = Application.persistentDataPath + $"/{BaseFolder}";
        if (!Directory.Exists(path1)) Directory.CreateDirectory(path1);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    #elif UNITY_IPHONE
        string path = Application.persistentDataPath + $"/{BaseFolder}/";
        string path1 = Application.persistentDataPath + $"/{BaseFolder}";
        if (!Directory.Exists(path1)) Directory.CreateDirectory(path1);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    #else
        string path = Application.dataPath + $"/{BaseFolder}/";
        string path1 = Application.dataPath + $"/{BaseFolder}";
        if (!Directory.Exists(path1)) Directory.CreateDirectory(path1);
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    #endif
    }

    void Start()
    {}
    void Update()
    {}

    public void writeToFileTXT() //Challenge 10, write data to .txt file upon quitting application
    {
        dataFile = getBasePath() + "dataFileTXT.txt";
        if(!File.Exists(dataFile))
        {
            File.WriteAllText(dataFile, "WRITING DATA TO .TXT FILE \n\n");
        }
        
        File.WriteAllText(dataFile, "Number of Bounces: " + GameObject.Find("BouncyBoy").GetComponent<challengeScript>().bounceDataSave + "\n");

        for(int i = 0; i < GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList.Count; i++)
        {
            File.AppendAllText(dataFile, GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList[i] +
            GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catDataList[i] + "\n");
        }
    }

    public void writeToFileCSV() //Challenge 10, write data to .csv file upon quitting application
    {
        dataFile = getBasePath() + "dataFileCSV.csv";
        if(GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList.Count > 0)
        {
            TextWriter tw = new StreamWriter(dataFile, false);
            tw.WriteLine("Number of Bounces:" + "," + GameObject.Find("BouncyBoy").GetComponent<challengeScript>().bounceDataSave);
            tw.Close();

            tw = new StreamWriter(dataFile, true);
            for(int i = 0; i < GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList.Count; i++)
            {
                tw.WriteLine(GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList[i] + ","
                + GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catDataList[i]);
            }
            tw.Close();
        }
    }

    public void writeToFileJSON() //Challenge 10, write data to .json file upon quitting application
    {
        dataFile = getBasePath() + "dataFileJSON.json";
        catClass cc = new catClass();
        int num = 0;
        bool labelBool = true;

        if(GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList.Count > 0)
        {
            cc.TotalBounces = GameObject.Find("BouncyBoy").GetComponent<challengeScript>().bounceDataSave;

            PropertyInfo[] properties = typeof(catClass).GetProperties();
            foreach(PropertyInfo property in properties)
            {
                if (property.Name!= nameof(catClass.TotalBounces))
                {
                    if(labelBool == true)
                    {
                        property.SetValue(cc, GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catLabelList[num]);
                        labelBool = false;
                    }
                    else
                    {
                        property.SetValue(cc, GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catDataList[num]);
                        labelBool = true;
                        num++;
                        if(num == 5){break;}
                    }
                }
            }
            string jsonString = JsonConvert.SerializeObject(cc);
            File.WriteAllText(dataFile, jsonString);
        }
    }
}