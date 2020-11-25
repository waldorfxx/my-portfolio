using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Smile Gauge


public class SmileGaugeController : MonoBehaviour
{
    public Image Bar;
    public GameObject destroyGauge;
    public GameObject destroyText;
    public RectTransform smileObj;
    public float maxGauge = 100f;
    public static float currentGauge = 0f;

    public bool _isuienabled=false;
    public bool _alreadyactivated=false;
    private int _t1=10; 
    private int _t2=22;
    private int _t3=40;
    private int _t4=52;
    private int _t5=70;
    private int _t6=82;
    private int _t7=100;
    private int _t8=112;
    
    // Start is called before the first frame update
    void Start()
    {
        currentGauge = 0f;
        _alreadyactivated=false;
        setUI(false);
    }

    // Update is called once per frame
    void Update()
{
       
        bool shoulduienabled=(TimeCount.seconds>_t1&&TimeCount.seconds<_t2) ||
                             (TimeCount.seconds>_t3&&TimeCount.seconds<_t4) || (TimeCount.seconds>_t5&&TimeCount.seconds<_t6) || 
                             (TimeCount.seconds>_t7&&TimeCount.seconds<_t8);

        if(shoulduienabled != _isuienabled)
        {
            if(shoulduienabled)
                _alreadyactivated=false;

            setUI(shoulduienabled);
        }

        if(_isuienabled)
        {
            if(!_alreadyactivated)
            {
                if(Input.GetKey(KeyCode.T))
                
                    increaseGauge();

                if(currentGauge>=maxGauge)
                {
                    _alreadyactivated=true;
                    resetGaugeValues();
                    
                    this.gameObject.GetComponent<Image>().enabled = false;
                    smileObj.GetComponent<Image>().enabled = false;
                    destroyText.GetComponent<Text>().enabled = false;
                    Bar.enabled=false;
                }
            }
        }

        if(DeathMenuController.deathMenu==true||!destroyGauge.activeInHierarchy)
        {
            resetGaugeValues();
            setUI(false);
        }
        
        updateSmileObj();
    }

    void increaseGauge()
    {
        currentGauge += 0.25f; //5sec to full
        float calcSmile = currentGauge / maxGauge;
        setSmile(calcSmile);
    }

    void setSmile(float mySmile)
    {
        Bar.fillAmount = mySmile;
    }

    // void getVal()
    // {
    //     if(_isuienabled && !_alreadyactivated)
    //     {
    //         increaseGauge();
    //     }

    //     else
    //     resetGaugeValues();

    // }


    void resetSmlieValue(GameObject SmileGauge, GameObject SmileText)
    {
        this.gameObject.GetComponent<Image>().enabled = false;
        smileObj.GetComponent<Image>().enabled = false;
        destroyText.GetComponent<Text>().enabled = false;
        Bar.enabled=false;
        
        maxGauge = 100f;
        currentGauge = 0f;
        print("reset"+ currentGauge);
    }

    private void setUI(bool isEnabled)
    {
        _isuienabled=isEnabled;
        this.gameObject.GetComponent<Image>().enabled = isEnabled;
        smileObj.GetComponent<Image>().enabled = isEnabled;
        destroyText.GetComponent<Text>().enabled = isEnabled;
        Bar.enabled=isEnabled;

        if(!isEnabled)
            Bar.fillAmount=0;
    }

    private void resetGaugeValues()
    {
        maxGauge=100.0f;
        currentGauge=0.0f;

    }

    void updateSmileObj()
    {
        smileObj.localPosition=new Vector3(
            Mathf.Lerp(this.Bar.rectTransform.rect.xMin,this.Bar.rectTransform.rect.xMax,(currentGauge/maxGauge)),
            smileObj.localPosition.y,smileObj.localPosition.z);
    }

}
