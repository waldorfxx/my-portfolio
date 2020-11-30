using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Affdex;

public class UICameraPreview : MonoBehaviour
{
    public CameraInput camInput;

    private RawImage _img;

    void Awake()
    {
        _img = this.GetComponent<RawImage>();
    }

    void Update()
    {
        if (_img == null || camInput.Texture == null)
            return;
        _img.texture = camInput.Texture;
    }
}
