using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


public class LogFileManager
{

    public static int countGame = 1;
    public static int totalScore = 0;
    public static int gaugeActivated = 0;
    public static int i = 1;
    public static float smileTime = 0;
    public static float totalSmileTime = 0;

    public static string smileLog="";



    public static void LogFile()
    {
        string fileName = "RunnerResult" + System.DateTime.Now.ToString();
        if (MainMenuController.playerName != "") fileName = MainMenuController.playerName;
        FileInfo fi = new FileInfo("../Playerdatalogs/");
        if (!fi.Directory.Exists)
        {
            System.IO.Directory.CreateDirectory("../Playerdatalogs/");
        }

        using (StreamWriter sw = new StreamWriter("../Playerdatalogs/" + fileName + ".csv", true))
        {
            sw.WriteLine("Game Count," + countGame);
            sw.WriteLine("Score," + totalScore);
            sw.WriteLine("Activated (times)," + gaugeActivated);
            // sw.WriteLine("Test count smiletime," + i);

            // for(int j=0; j<i; j++)
            // {
            // sw.WriteLine("Smile " + j + " Time(s), " + smileLog);
            // }
            // sw.Write(smileLog);
            sw.WriteLine("Total Smile Time, "+ totalSmileTime);

            sw.WriteLine("----------------------------------");

            // sw.WriteLine(s"Sum of Smile Period (secs)," + smileTime)
            sw.Close();
        }

        

    }

    public static void ResetValue()
    {
        gaugeActivated = 0;
        totalScore = 0;
        totalSmileTime = 0;
        smileTime = 0;
        i = 1;
        smileLog="";
        Debug.Log("Reset");
    }


}
