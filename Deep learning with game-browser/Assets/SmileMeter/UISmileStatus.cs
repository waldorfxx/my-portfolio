using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISmileStatus : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = this.GetComponent<Text>();
    }

    void Update()
    {
        print(PlatformsGenerationEmotionController.facefound);
        _text.text = PlayerEmotionController.faceCount > 0 ?
            "Found the face" : "Lost the face";
    }
}