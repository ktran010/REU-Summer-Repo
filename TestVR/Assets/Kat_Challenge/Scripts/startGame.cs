using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    // Update is called once per frame
    public void startChallenge()
    {
        SceneManager.LoadScene("Challenge");
    }
}
