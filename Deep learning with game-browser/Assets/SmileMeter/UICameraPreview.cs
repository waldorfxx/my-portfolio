using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

// using Affdex;

public class UICameraPreview : MonoBehaviour
{

//     #if UNITY_WEBGL && !UNITY_EDITOR

//     [DllImport("__Internal")] private static extern void Test(); 
//     #else
// // something else to emulate what you want to do

// #endif

//     private static extern void Hello();
    public RawImage rawimage;
     void Start () 
     {
        //  Hello();
         WebCamTexture webcamTexture = new WebCamTexture();
         rawimage.texture = webcamTexture;
         rawimage.material.mainTexture = webcamTexture;
         webcamTexture.Play();
        //  faceapi();
        
     }
}
