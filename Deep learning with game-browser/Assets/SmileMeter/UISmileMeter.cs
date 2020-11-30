using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISmileMeter : MonoBehaviour
{
    public Color minColor;
    public Color maxColor;

    private const float _MINVAL = 0.0f, _MAXVAL = 100.0f;

    private Image _fill;
    private Color _color;

    void Awake()
    {
        _fill = this.transform.Find("_fill").GetComponent<Image>();
    }

    void Update()
    {
        _fill.color = getCurrentColor(PlatformsGenerationEmotionController.smileAmount);
        _fill.fillAmount = PlatformsGenerationEmotionController.smileAmount / (_MAXVAL - _MINVAL);
    }

    private Color getCurrentColor(float value)
    {
        return Color.Lerp(minColor, maxColor, value / (_MAXVAL - _MINVAL));
    }
}