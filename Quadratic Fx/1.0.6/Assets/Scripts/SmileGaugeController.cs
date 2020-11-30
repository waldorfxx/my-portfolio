using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Affdex;

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
    private int _t1=15;
    private int _t2=30;

    private int _t3=40;
    private int _t4=55;
    
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
        // if((TimeCount.seconds>=15&&TimeCount.seconds<=35)||(TimeCount.seconds>=45&&TimeCount.seconds<=55))
        // {
        //     // Instantiate(GameObject.Find("SmileGauge"));
        //     // Instantiate(destroyText)
            
        //     this.gameObject.GetComponent<Image>().enabled = true;
        //     smileObj.GetComponent<Image>().enabled = true;
        //     destroyText.GetComponent<Text>().enabled = true;
        //     // GameObject.Find("Canvas").transform.Find("DeathMenu").GetComponent<Image>().enabled = true;

        //     if(destroyGauge.activeInHierarchy&&PlayerEmotionController.smileee==true)
        //     {
        //         increaseGauge();
        //     }
            
        //     if(!this.gameObject.GetComponent<Image>().enabled)
        //     { 
        //         resetSmlieValue(destroyGauge, destroyText);
        //     }

        // } 
        //else if(TimeCount.seconds>35&&TimeCount.seconds<45) resetSmlieValue(destroyGauge, destroyText);
        
        bool shoulduienabled=(TimeCount.seconds>_t1&&TimeCount.seconds<_t2) ||
                             (TimeCount.seconds>_t3&&TimeCount.seconds<_t4);
        if(shoulduienabled != _isuienabled)
        {
            if(shoulduienabled)
                _alreadyactivated=false;

            setUI(shoulduienabled);
        }

        // if(_isuienabled && PlayerEmotionController.smileee)
        //     increaseGauge();
        if(_isuienabled)
        {
            if(!_alreadyactivated)
            {
                if(PlayerEmotionController.smileee)
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
            //resetSmlieValue(destroyGauge, destroyText);
            resetGaugeValues();
            setUI(false);
        }
        
        updateSmileObj();
        // print("increasegauge"+ currentGauge);
    }

    void increaseGauge()
    {
        currentGauge += 0.5f; //5sec to full
        float calcSmile = currentGauge / maxGauge;
        setSmile(calcSmile);
    }

    void setSmile(float mySmile)
    {
        Bar.fillAmount = mySmile;
    }

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
