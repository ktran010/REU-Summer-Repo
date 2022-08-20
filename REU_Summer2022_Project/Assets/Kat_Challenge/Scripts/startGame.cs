using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    //Challenge 9, View MinimumWindowSize.cs script!!!!
    int minWidth = 950;
    int minHeight = 600;

    void Start()
    {
        //Challenge 9
        MinimumWindowSize.Set(minWidth, minHeight);
    }

    void Update()
    {
        // -------------WINDOW RESIZING------------------ //Challenge 9
        // ---------------------------------------------- // View MinimumWindowSize.cs script!!!!
        MinimumWindowSize.Set(minWidth, minHeight);
    }

    public void startChallenge()
    {
        SceneManager.LoadScene("Challenge");
    }
}
