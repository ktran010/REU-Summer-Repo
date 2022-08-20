using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class challengeScript : MonoBehaviour
{
    //Challenge 1
    bool timeFreezeBool; // bool for freezing bouncing ball
    float timeFreeze; // freeze bouncing ball for 3 seconds
    int counterBounce; // counts number of times the ball bounces

    //Challenge 2
    private Camera cameraRef; // reference to a camera
    private GameObject Cam1; // will reference to "MainCamera" tag
    private GameObject Cam2; // will reference to second camera created in script
    public Vector3 cameraOffset; // starting position of Camera.main at runtime
    private Vector3 Cam1Position; // position of the Camera.main to determine Camera 2's position
    private Vector3 Cam2Position; // position of Camera 2
    public float velocity; // speed of camera translation
    float timeSwitch; // switch between Camera.main and second Camera every 4 seconds

    //Challenge 3
    public float ballVelocity;

    //Challenge 5
    private Text t; // text to display number of kitties caught
    private Text highest; // text to display points
    private Text timer; // text to display elapsed time
    float timerFloat; // holds value of elapsed time
    float min; // holds value of elapsed time, for minutes
    float sec; // holds value of elapsed time, for seconds
    int highestScore;  // text that saves score
    int pointTracker; // add/minus counter depending on number of kitties caught

    //Challenge 6
    private GameObject gameOverObject;

    //Setting random position and rotation of kitties
    bool setRandomPositionBool;
    bool setRandomEulerAnglesBool;
    public Vector3 minPosition;
    public Vector3 maxPosition;

    public Vector3 minEulerAngles;
    public Vector3 maxEulerAngles;

    public Transform[] catArray; // remember to add Cat1-Cat5 gameObjects to this array in the script!


    // Start is called before the first frame update
    void Start()
    {
        //Challenge 1
        counterBounce = 0;
        timeFreezeBool = false;
        timeFreeze = 0.0f;

        //Challenge 2
        Cam1 = GameObject.FindWithTag("MainCamera");
        Cam2 = new GameObject("Cam2");
        Cam2.AddComponent<Camera>();
        Cam2.GetComponent<Camera>().depth = 1;
        Cam2.GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
        Cam2.gameObject.SetActive(false);

        timeSwitch = 0.0f;
        cameraRef = Cam1.gameObject.GetComponent<Camera>(); // setting camera reference to be Camera.main
        cameraOffset = Cam1.gameObject.GetComponent<Camera>().transform.position - GameObject.Find("BouncyBoy").transform.position;

        //Challenge 5
        pointTracker = 0;
        highestScore = 0;

        //Challenge 6
        gameOverObject = GameObject.Find("GameOver").gameObject;
        gameOverObject.SetActive(false);

        //Positioning kitties
        setRandomPositionBool = true;
        setRandomEulerAnglesBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        //---------------BALL FREEZING----------------- //Challenge 1
        //---------------------------------------------
        if(timeFreezeBool == true) // if 10th bounce is counted, timeFreezeBool will be true
        {
            timeFreeze += Time.deltaTime;
            // print("Freeze time: " + timeFreeze);
    
            if(timeFreeze >= 3.00f) //if 3 seconds has passed
            {
                print("UNFREEZE!");
                unFreeze(); // call upon unFreeze() function
            }
        }
        else
        {
             //---------------SPHERE MOVEMENT---------------- //Challenge 3
            //----------------------------------------------
            sphereMovement();
        }


        //---------------CAMERAS ORBITING-------------- //Challenge 2
        //---------------------------------------------
        orbit();
        Cam1.gameObject.transform.position = GameObject.Find("BouncyBoy").transform.position + cameraOffset;
        Cam1.gameObject.transform.LookAt(GameObject.Find("BouncyBoy").transform.position); //camera looks at the BouncyBoy gameObject

        Cam2.gameObject.transform.position = GameObject.Find("BouncyBoy").transform.position +
        new Vector3(-cameraOffset.x, cameraOffset.y, -cameraOffset.z);
        Cam2.gameObject.transform.LookAt(GameObject.Find("BouncyBoy").transform.position); //camera looks at the BouncyBoy gameObject

        //---------------CAMERA SWITCHING--------------- //Challenge 2
        //----------------------------------------------
        // print(cameraRef);
        timeSwitch += Time.deltaTime;
        // print("Switch time: " + timeSwitch);

        if(timeSwitch >= 4.00f) //if 3 seconds has passed
        {
            print("SWITCH CAMERAS!");
            switchCameras(); // call upon switchCameras() function
        }

        //---------------PROP MOVEMENT------------------ //Challenge 4
        //---------------------------------------------- // need to Set Active "Type3-MoveOnOwn" group of objects in Hierarchy!!!!!!!!!
        // propMovement();


        //---------------POINTS SYSTEM------------------ //Challenge 5
        //----------------------------------------------
        timerFloat = Time.timeSinceLevelLoad;
        timer = GameObject.Find("Text1.5").GetComponent<UnityEngine.UI.Text>();
        // min = ((int)timerFloat / 60).ToString();
        // sec = ((int)timerFloat % 60).ToString("F2");
        // timer.text = min + ":" + sec;
        min = Mathf.FloorToInt(timerFloat / 60);
        sec = Mathf.FloorToInt(timerFloat % 60);
        timer.text = string.Format("{0:00}:{1:00}", min, sec);

        t = GameObject.Find("Text1").GetComponent<UnityEngine.UI.Text>();
        t.text = "Kittens: " + pointTracker.ToString() + "/5";


        //---------------GAME OVER---------------------- //Challenge 6
        //----------------------------------------------
        if(pointTracker == 5) // game over condition, if score drops below zero
        {
            gameOver();
        }
        if(Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("Challenge"); //reload Challenge scene upon hitting P key
        }


        //--------POSITIONING, MOVING KITTIES----------- // Randomly setting position + rotation of kitties
        //----------------------------------------------
        if(setRandomPositionBool)
        {
          SetRandomPosition();
          setRandomPositionBool = false;
        }
        if(setRandomEulerAnglesBool)
        {
          SetRandomEulerAngles();
          setRandomEulerAnglesBool = false;
        }

        runner();
    }

    public void OnCollisionEnter(Collision c) //Challenge 1 and Challenge 5
    {
        //---------------BOUNCING BALL----------------- //Challenge 1
        //----------------------------------------------
        // print("COLLIDE");
        // this.gameObject.GetComponent<Rigidbody>().AddForce(0,500f,0);
        GameObject.Find("BouncyBoy").GetComponent<Renderer>().material.color = Random.ColorHSV();
        counterBounce++; // increment counterBounce for number of bounces
        print("Bounce Counter: " + counterBounce);

        if(counterBounce == 10) // if the ball bounces a 10th time, freeze it (i.e, turn on "Is Kinematic" property of Rigidbody component)
        {
            GameObject.Find("BouncyBoy").GetComponent<Rigidbody>().isKinematic = true; // turn on "Is Kinematic" property
            timeFreezeBool = true;
        }

        //---------------POINTS SYSTEM------------------ //Challenge 5
        //----------------------------------------------
        if(c.gameObject.tag == "POINTS")
        {
            print("hit object, points++");
            pointTracker++;
            if(pointTracker > highestScore)
            {
                highestScore = pointTracker;
            }

            c.gameObject.SetActive(false);
        }

        if(c.gameObject.tag == "MoveProp1")
        {
            print("hit MP1, points--");
            pointTracker--;
            if(pointTracker > highestScore)
            {
                highestScore = pointTracker;
            }
        }

        if(c.gameObject.tag == "MoveProp2")
        {
            print("hit MP2, points--");
            pointTracker--;
            if(pointTracker > highestScore)
            {
                highestScore = pointTracker;
            }
        }
    }

    public void unFreeze() //Challenge 1
    {
        GameObject.Find("BouncyBoy").GetComponent<Rigidbody>().isKinematic = false; // turn off "Is Kinematic" property
        GameObject.Find("BouncyBoy").GetComponent<Rigidbody>().AddForce(0,400f,0); // add a force to ball after unfreezing, won't bounce otherwise

        timeFreeze = 0.0f; // reset timeFreeze, timeFreezeBool, counterBounce
        timeFreezeBool = false;
        counterBounce = 0;
    }

    public void orbit() // Challenge 2
    {
        cameraOffset = Quaternion.AngleAxis(Time.deltaTime * 20.0f * velocity, Vector3.up) * cameraOffset;
    }

    public void switchCameras() //Challenge 2
    {
        //if(cameraRef == Cam1.gameObject.GetComponent<Camera>())
        if(Cam1.gameObject.activeSelf == true)
        {
            // print("Main cam active");
            Cam2.gameObject.SetActive(true);
            cameraRef = Cam2.gameObject.GetComponent<Camera>();
            // cameraRef.transform.LookAt(Cam2.gameObject.transform);
            Cam1.gameObject.SetActive(false);
        }
        //else if(cameraRef == Cam2.gameObject.GetComponent<Camera>())
        else if(Cam2.gameObject.activeSelf == true)
        {
            // print("Cam2 active");
            Cam1.gameObject.SetActive(true);
            cameraRef = Cam1.gameObject.GetComponent<Camera>();
            // cameraRef.transform.LookAt(Cam1.gameObject.GetComponent<Camera>().transform);
            Cam2.gameObject.SetActive(false);
        }

        timeSwitch = 0.0f; // reset timeSwitch
    }

    public void sphereMovement() //Challenge 3
    {
        if(Input.GetKey(KeyCode.W))
        {
            GameObject.Find("BouncyBoy").transform.Translate(cameraRef.transform.forward * ballVelocity * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S))
        {
            GameObject.Find("BouncyBoy").transform.Translate(cameraRef.transform.forward * -ballVelocity * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            GameObject.Find("BouncyBoy").transform.Translate(cameraRef.transform.right * ballVelocity * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A))
        {
            GameObject.Find("BouncyBoy").transform.Translate(cameraRef.transform.right * -ballVelocity * Time.deltaTime);
        }
    }

    // public void propMovement() //Challenge 4, need to Set Active "Type3-MoveOnOwn" group of objects in Hierarchy!!!!!!!!!
    // {
    //     GameObject.FindWithTag("MoveProp1").transform.Rotate(Vector3.forward * Time.deltaTime * 180.0f);
    //     GameObject.FindWithTag("MoveProp2").transform.Translate(Vector3.right * Time.deltaTime * 1.0f);
    // }

    public void gameOver() //Challenge 6
    {
        GameObject.Find("BouncyBoy").gameObject.SetActive(false); // disable controlling ball

        gameOverObject.SetActive(true); // make game over screen appear
        GameObject.Find("PointsView").gameObject.SetActive(false);
        timer = GameObject.Find("Text4").GetComponent<UnityEngine.UI.Text>();
        timer.text = string.Format("{0:00}:{1:00}", min, sec);
        // highest = GameObject.Find("Text4").GetComponent<UnityEngine.UI.Text>();
        // highest.text = highestScore.ToString(); // displaying highest score
        // // GameObject.Find("BouncyBoy").GetComponent<challengeScript>().t = GameObject.Find("Text2").GetComponent<UnityEngine.UI.Text>();
    }

    public void MainMenuReturn() //Challenge 6
    {
        SceneManager.LoadScene("StartGame"); // Clicking on "Main Menu" at GAME OVER screen takes you to StartGame scene
        // UnityEditor.EditorApplication.isPlaying = false; // quitting program, AKA exiting Play Mode
    }
    public void quitProgram() //Challenge 6
    {
        //UnityEditor.EditorApplication.isPlaying = false; // quitting program, AKA exiting Play Mode
        Application.Quit();
    }

    void SetRandomPosition() // Setting random position of kitties
    {
      for(int i = 0; i < catArray.Length; i++)
      {
        catArray[i].transform.localPosition = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            1.5f,
            Random.Range(minPosition.z, maxPosition.z));
      }
    }

    void SetRandomEulerAngles() // Setting random rotation of kitties
    {
      for(int i = 0; i < catArray.Length; i++)
      {
        catArray[i].transform.localEulerAngles = new Vector3(
            0.0f,
            Random.Range(minEulerAngles.y, maxEulerAngles.y),
            0.0f);
      }
    }

    void runner() // Setting kitties to run!!!!!
    {
        for(int i = 0; i < catArray.Length; i++)
        {
            //catArray[i].transform.Rotate(Vector3.up * Time.deltaTime * 180.0f);
            catArray[i].transform.Translate(Vector3.forward * Time.deltaTime * 8.0f);
        }
    }
}