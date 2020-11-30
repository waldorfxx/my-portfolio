using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Affdex;
using System.Linq;


public class PlayerEmotionController : ImageResultsListener
{
    public Dictionary<string, float> expression;
    public static bool smileee=false;
    public static int faceCount = 0;
    // public static bool facefound = false;

    void Start()
    {

    }

    /**
     *  Initialization
     */
    void Awake()
    {
        expression = new Dictionary<string, float>() { { "currentSmile", 0f } };
        // emotions = new Dictionary<string, float>() { { "currentAnger", 0f }, { "currentSurprise", 0f }, { "currentJoy", 0f }, { "currentSadness", 0f } };
    }

    /**
     *  Is called when a face is detected by Affectiva
     */
    public override void onFaceFound(float timestamp, int faceId)
    {
        // if(Debug.isDebugBuild) Debug.Log("Found the face");
        PlatformsGenerationEmotionController.facefound = false;
        // Debug.Log(PlatformsGenerationEmotionController.facefound );
        
    }

    /**
     *  Is called when Affectiva does not detect a face
     */
    public override void onFaceLost(float timestamp, int faceId)
    {
        smileee=false;
        // if(Debug.isDebugBuild) Debug.Log("Lost the face");
        PlatformsGenerationEmotionController.facefound = true;
    }

    /**
     *  Fill the dictionary with the returned values from Affectiva, for each emotion
     */
    public override void onImageResults(Dictionary<int, Face> faces)
    {
        float val = 0;
        if (faces.Count > 0)
        {
            if (faces[0].Expressions.TryGetValue(Expressions.Smile, out val)) expression["currentSmile"] = val;

            if(val > 50) smileee = true;

            
            else smileee=false;

            // faces[0].Expressions.TryGetValue(Expressions.Smile,out smileee);
            
            // detect 4 face emotions
            // if (faces[0].Emotions.TryGetValue(Emotions.Anger, out val)) emotions["currentAnger"] = val;
            // if (faces[0].Emotions.TryGetValue(Emotions.Surprise, out val)) emotions["currentSurprise"] = val;
            // if (faces[0].Emotions.TryGetValue(Emotions.Joy, out val)) emotions["currentJoy"] = val;
            // if (faces[0].Emotions.TryGetValue(Emotions.Sadness, out val)) emotions["currentSadness"] = val;
        }
        PlatformsGenerationEmotionController.smileAmount = val;
        faceCount = faces.Count;
    }

    /**
     *  Return the predominant emotion detected
     */
    public string EmotionDetected(float emotionDetection)
    {
        float max = expression.Values.Max();
        if (max >= emotionDetection) return expression.FirstOrDefault(x => x.Value == expression.Values.Max()).Key;
        return "null";
    }
}