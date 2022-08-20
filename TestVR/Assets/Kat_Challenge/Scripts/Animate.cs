using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    // Code snippt taken from:
    // https://gamedev.stackexchange.com/questions/35030/can-i-use-an-animated-gif-as-a-texture
    // Attach this script to some object (i.e plane, cube, etc)
    // Need to add each GIF frame to the "frames" array in the inspector
    //--------------------------------------------------------------------------------------

    [SerializeField] private Texture2D[] frames;
    [SerializeField] private  float fps = 10.0f;
    //public Transform[] catArrayCopy;

    private Material mat;

    void Start ()
    {
        mat = GetComponent<Renderer>().material;
        // catArrayCopy = new Transform[5];

        // for(int i = 0; i < catArrayCopy.Length; i++)
        // {
        //     catArrayCopy[i] = GameObject.Find("BouncyBoy").GetComponent<challengeScript>().catArray[i];
        // }
    }

    void Update ()
    {
        int index = (int)(Time.time * fps);
        index = index % frames.Length;
        mat.mainTexture = frames[index]; // usar en planeObjects
        //GetComponent<RawImage> ().texture = frames [index];
    }

    public void OnCollisionEnter(Collision c)
    {
        // print("OWIE!");
        if(c.gameObject.tag == "FENCE") // if kitty collides with fence, flip their direction
        {
            // print("OW FENCE");
            this.transform.localEulerAngles = new Vector3(-180.0f, this.transform.localEulerAngles.y, -180.0f);
            // for(int i = 0; i < catArrayCopy.Length; i++)
            // {
            //     catArrayCopy[i].transform.Translate(Vector3.forward*-1 * Time.deltaTime * 10.0f);
            // }
        }
    }
}
