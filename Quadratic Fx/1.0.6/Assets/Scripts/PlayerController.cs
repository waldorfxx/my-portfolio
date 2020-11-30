using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    // Player Variables
    public float playerJumpForce;
    private float lastCharacterPosition;
    private bool playerFalling;
    private bool onGround;
    private bool isBendDown;
    private bool leftArm;
    private bool rightArm;
    private Rigidbody2D playerBody;
    private Animator playerAnimator;
    private PlayerEmotionController emotionController;

    // Ground Variables
    private LayerMask groundMaterial;
    private Transform groundDetection;
    private float groundDetectionRadius;

    // Game Variables
    private GameController game;
    private CoinsController coinsManager;

    // Sounds
    public AudioSource jumpSound;
    public AudioSource drownSound;
    public AudioSource killedSound;
    public AudioSource runSound;

    //Speed
    public float startSpeed;
    private float playerSpeed;
    private float maxSpeed;
    public float boostSpeed = 10f;
    public float timer = 0.0f;
    public int seconds;
    public static bool checkToBend = false;
    public static bool checkToRaiseL = false;
    public static bool checkToRaiseR = false;
    private Animation anim;
    public static bool smileSwitch = false;
    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }
    private float startTime;

    //
    public Collider coinJumpbound;
    private float rannum;
    private float floatnum;
    private int intnum;
    
    // exponential
    private float lambda;
    private float x; //times of trial
    private float exp;
    private float e = 2.72f;
    private float rounded;

    // quadratic
    public static int time = 0;
    private int time2 = 0;
    private static float quadratic_c;
    private static float quadratic_t;
    //Curve Estimation (Trap) = 0.0000201𝑠^2 – 0.003𝑠+ 0.226 
    private float _at = 0.0000201f;
    private float _bt = 0.003f;
    private float _ct = 0.226f;
    //Curve Estimation (Coin) = 0.000006817 𝑠^2 + 0.001 𝑠+ 0.407 
    private float _ac = 0.000006817f;
    private float _bc = 0.001f;
    private float _cc = 0.407f;
    //Data
    Dictionary<string, int> data = new Dictionary<string, int>() { { "platform5", 0}, { "platform6", 0 }, { "platform7", 0 }, { "platform10", 0 },
                                                                   { "spikes", 0 }, { "Catcher", 0 }, { "evilBlock", 0 } , { "evilSaw", 0 } };

    /** 
     *  Initialization
     */
    void Start()
    {
        emotionController = this.gameObject.AddComponent<PlayerEmotionController>();
        groundMaterial = LayerMask.GetMask("Ground");
        groundDetection = GameObject.Find("GroundCollision").GetComponent<Transform>();

        game = GameObject.Find("GameManager").GetComponent<GameController>();
        coinsManager = GameObject.Find("CoinManager").GetComponent<CoinsController>();
        Time.timeScale = 1;
        maxSpeed = 9f; //7.5f
        //cannot use this switch case
        switch (PlayerPrefs.GetInt("Speed"))
        {
            case 1:
                startSpeed += (maxSpeed - startSpeed) / 2;
                break;
            case 2:
                startSpeed = maxSpeed;
                break;
            default:
                break;
        }

        playerBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        groundDetectionRadius = GetComponent<BoxCollider2D>().size.x / 2;
        lastCharacterPosition = transform.position.x;

 

        onGround = false;
        isBendDown = false;
        leftArm = false;
        rightArm = false;
        playerFalling = false;


        startTime = Time.time;

        runSound.Play();

    }

    /** 
     *  Update is called once per frame
     */
    void Update()
    {
        playerBody.velocity = new Vector2(playerSpeed, playerBody.velocity.y);
        CheckCollision();
        SetAnimatorBool();
        CheckJump();
        IncreaseSpeed();

        time2 = quadratic_trap(time, time2);
        time2 = quadratic_coin(time, time2);

        if(checkToBend == true)
            playerAnimator.SetBool("isLoopBend", checkToBend); 
        else if(checkToRaiseL == true)
            playerAnimator.SetBool("leftArm", checkToRaiseL);
        else if(checkToRaiseR == true)
            playerAnimator.SetBool("rightArm", checkToRaiseR);
        
        

        // if(Input.GetKeyDown(KeyCode.P)) LogFileManager.pressP++;
        // if(Input.GetKeyDown(KeyCode.V))
        
        if(SmileGaugeController.currentGauge>99)
            {
                smileSwitch = true;
                playerSpeed = boostSpeed;                
            }


        if (smileSwitch==true && seconds < 3) {
                
                timer += Time.deltaTime;
                seconds = (int)timer % 60;   

                Vector3 newScale = transform.localScale;

                if(timer <= 0.4)
                {
                newScale *= 1.015f;
                }
                else if(timer >=2.7 && timer <3)
                {
                newScale /= 1.02f;
                }

                transform.localScale = newScale;

                

        } 
            else if (seconds >=3) {
            smileSwitch = false;
            seconds = 0;
            timer = 0.0f;
            playerSpeed = startSpeed;
            transform.localScale = new Vector3 (1f, 1f, 1f);
            LogFileManager.gaugeActivated++;

            }



        if(PlayerEmotionController.smileee==true)
        {
            LogFileManager.smileTime = LogFileManager.smileTime + Time.fixedDeltaTime;
            // LogFileManager.smileLog = LogFileManager.smileTime.ToString();
        }
        else if (PlayerEmotionController.smileee == false)
        {
            if (LogFileManager.smileTime != 0)
            {
                // LogFileManager.smileLog = LogFileManager.smileLog + "Smile " + LogFileManager.i.ToString() + "Time(s), " + LogFileManager.smileTime.ToString() + "\n";
                // print("send " + LogFileManager.smileLog);

                // LogFileManager.smileLog = LogFileManager.smileLog + "Smile " + LogFileManager.i.ToString() + "Time(s), " + LogFileManager.smileTime.ToString() + "\n";
                LogFileManager.totalSmileTime += LogFileManager.smileTime;
                // print("send " + LogFileManager.smileLog);
                LogFileManager.smileTime = 0;
                print("reset smileTime = 0");
                LogFileManager.i++;
            }

        }


    }

    /**
     *  Set the variable in the Animator
     */
    void SetAnimatorBool()
    {
        onGround = Physics2D.OverlapCircle(groundDetection.position, groundDetectionRadius, groundMaterial);
        isBendDown = Input.GetKey(KeyCode.S) && onGround;
        leftArm = Input.GetKey(KeyCode.A) && onGround && !Input.GetKey(KeyCode.D) && !isBendDown;
        rightArm = Input.GetKey(KeyCode.D) && onGround && !Input.GetKey(KeyCode.A) && !isBendDown;
        playerFalling = playerBody.velocity.y < -0.1;

        playerAnimator.SetFloat("playerSpeed", playerBody.velocity.x);
        playerAnimator.SetBool("onGround", onGround);
        playerAnimator.SetBool("isBendDown", isBendDown);
        playerAnimator.SetBool("leftArm", leftArm);
        playerAnimator.SetBool("rightArm", rightArm);
        playerAnimator.SetBool("playerFalling", playerFalling);
    }

    /**
     *  Check if the player want to jump 
     */
    void CheckJump()
    {

        // Using spacebar to jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround && !GameObject.Find("PauseMenu"))
            {
                playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                StartCoroutine(SwitchSoundJump());
            }
        }
    }

    /**
     *  Check if the caracter is blocked 
     */
    void CheckCollision()
    {
        if (lastCharacterPosition == transform.position.x && !GameObject.Find("PauseMenu"))
        {
            transform.position = new Vector2(transform.position.x + 0.01f, transform.position.y);
        }
        lastCharacterPosition = transform.position.x;
    }

    /**
     *  Co-routine to switch jump sound and footsteps sound
     */
    IEnumerator SwitchSoundJump()
    {
        runSound.Pause();
        jumpSound.Play();
        yield return new WaitForSeconds(0.75f);
        runSound.Play();
    }

    /** 
     *  Load the adapted action when the player collides with an object
     */
    IEnumerator OnTriggerEnter2D(Collider2D item)
    {
             
        bool immune = smileSwitch == true;
    


        if (item.gameObject.tag == "kill")
        {
            if (immune) {
                print("emotion");
                yield return null;;
            }

            SetData(item.gameObject.name);
            string playerName = PlayerPrefs.GetString("PlayerName");
            //Data are set only during the body mode, for a real player
            if (!playerName.Equals("Default") && PlayerPrefs.GetInt("BodyMode") == 1) SendData(playerName);
            runSound.Stop();
            killedSound.Play();
            game.LoadDeathMenu();
        }

        
        else if (item.gameObject.tag == "crouchbound")
        {
            if (immune) {
                // print("emotion");
                item.gameObject.GetComponent<Collider2D>().isTrigger=false;
                yield return null;
            }
                // rannum = Random.Range(1,10);
                rannum = Random.Range(0.00f, 1.00f);
                Debug.Log("rannum = "+ rannum + " qdt = " + quadratic_t);
                
                if (rannum>quadratic_t)
                {
                    floatnum = Random.Range(1,10);
                    floatnum = (floatnum/10);
                    lambda = 1f;
                    exp = 1 - Mathf.Pow(e, -lambda*x);
                    while(floatnum>rounded){
                        x++;
                        yield return new WaitForSeconds(0.15f);
                        floatnum = Random.Range(1,10);
                        floatnum = (floatnum/10);
                        exp = 1 - Mathf.Pow(e, -lambda*x);
                        rounded = Mathf.Round(exp*10)/10; ////

                        // Debug.Log("[1] Exponential =" + rounded + " Random Prob =" + floatnum);
                    }
                    checkToBend = true;
                }
                else if (rannum<=quadratic_t)
                {
                    floatnum = Random.Range(1,10);
                    floatnum = (floatnum/10);
                    lambda = 0.15f;
                    exp = 1 - Mathf.Pow(e, -lambda*x);
                    while(floatnum>rounded){
                        x++;
                        yield return new WaitForSeconds(0.15f);
                        floatnum = Random.Range(1,10);
                        floatnum = (floatnum/10);
                        exp = 1 - Mathf.Pow(e, -lambda*x);
                        rounded = Mathf.Round(exp*10)/10; ////

                        // Debug.Log("[2] Exponential =" + rounded + " Random Prob =" + floatnum);
                    }

                    checkToBend = false;  
                    intnum = Random.Range(1,3);
                    if(intnum==1)
                    {
                        if (onGround && !GameObject.Find("PauseMenu"))
                        {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                        StartCoroutine(SwitchSoundJump());
                        }
                    }
                    else checkToBend = false; 

                    yield return null;
                //playerAnimator.Play("isBendDown");
                }
        
        }

        else if (item.gameObject.tag == "water")
        {
            if (immune) {
                item.gameObject.GetComponent<Collider2D>().isTrigger=false;
                yield return null;;
            }
            // print("ab");
            runSound.Stop();
            drownSound.Play();
            game.LoadDeathMenu();
        }

        else if (item.gameObject.tag == "leftBound")
        {
            // Debug.Log("leftBound");
            rannum = Random.Range(0.00f, 1.00f);
            Debug.Log("rannum = "+ rannum + " qdt = " + quadratic_c);
                if(rannum>quadratic_c)
                { 
                    onGround = true;
                    checkToBend = false;
                    checkToRaiseL = true;
                    playerAnimator.SetBool("leftArm", checkToRaiseL);
                    // Debug.Log("coinL");
                }   
                else if (rannum<=quadratic_c)
                {
                    checkToRaiseL = false;  
                    intnum = Random.Range(1,3);
                    if(intnum==1)
                    {
                        if (onGround && !GameObject.Find("PauseMenu"))
                        {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                        StartCoroutine(SwitchSoundJump());
                        }
                    }
                    else if(intnum==2) checkToRaiseR = false; 
                    else yield return null;
                    // Debug.Log("Prob[2] Random Number = " + rannum);
   
                }
        }

        else if (item.gameObject.tag == "rightBound")
        {
            // Debug.Log("rightBound");
            rannum = Random.Range(0.00f, 1.00f);
            Debug.Log("rannum = "+ rannum + " qdt = " + quadratic_c);
                if(rannum>quadratic_c)
                { 
                    onGround = true;
                    checkToBend = false;
                    checkToRaiseR = true;
                    // playerAnimator.SetBool("rightArm", checkToRaiseR);
                    // Debug.Log("coinR");
                }   
                else if (rannum<=quadratic_c)
                {
                    checkToRaiseR = false;  
                    intnum = Random.Range(1,4);
                    if(intnum==1)
                    {
                        if (onGround && !GameObject.Find("PauseMenu"))
                        {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                        StartCoroutine(SwitchSoundJump());
                        }
                    }
                    else if(intnum==2) checkToRaiseL = false; 
                    else if(intnum==3) checkToBend = true;
                    else yield return null;
                    // Debug.Log("Prob[2] Random Number = " + rannum);    
                }

        }       

            
        
        else if (item.gameObject.tag == "jumpbound")
        {
            if (immune) {
                item.gameObject.GetComponent<Collider2D>().isTrigger=false;
                yield return null;;
            }

                rannum = Random.Range(0.00f, 1.00f);
                Debug.Log("rannum = "+ rannum + " qdt = " + quadratic_t);

                if (rannum>quadratic_t)
                {
                    floatnum = Random.Range(1,6);
                    floatnum = (floatnum/10);
                    lambda = 1f;
                    exp = 1 - Mathf.Pow(e, -lambda*x);
                    while(floatnum>rounded){
                        x++;
                        yield return new WaitForSeconds(0.05f);
                        floatnum = Random.Range(1,3);
                        floatnum = (floatnum/3);
                        exp = 1 - Mathf.Pow(e, -lambda*x);
                        rounded = Mathf.Round(exp*10)/10; ////

                        // Debug.Log("[1] Exponential =" + rounded + " Random Prob =" + floatnum);
                    }
                    if (onGround && !GameObject.Find("PauseMenu"))
                    {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                        StartCoroutine(SwitchSoundJump());
                    }
                    //playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                    // Debug.Log("Prob[2] Random Number = " + rannum + " Delay =" + floatnum);
                }
                else if (rannum<=quadratic_t)
                {
                    checkToBend = true;  
                    intnum = Random.Range(1,3);
                    if(intnum==1) checkToBend = true; 
                    else yield return null;   
                }
        } 
            
            else if (item.gameObject.tag == "coinJumpbound"){
    
                rannum = Random.Range(0.00f, 1.00f);
                Debug.Log("rannum = "+ rannum + " qdt = " + quadratic_c);
                if(rannum>quadratic_c)
                { 
                    if (onGround && !GameObject.Find("PauseMenu"))
                    {
                    playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                    }

                    if (item.gameObject.tag == "coin")   coinsManager.CollectCoin(item.gameObject);
                    
                    if (item.gameObject.tag == "coinJumpbound") 
                    {
                        yield return new WaitForSeconds(0.2f);
                        item.enabled = false;
                        Debug.Log("[1]");
                    }
                }   
                
                else if (rannum<=quadratic_c)
                {
                    floatnum = Random.Range(3,10);
                    floatnum = (floatnum/10);
                    yield return new WaitForSeconds(floatnum);
                    intnum = Random.Range(1,2);
                    if(intnum==1)
                    {
                        if (onGround && !GameObject.Find("PauseMenu"))
                        {
                        playerBody.velocity = new Vector2(playerBody.velocity.x, playerJumpForce);
                        StartCoroutine(SwitchSoundJump());
                        }
                    }
                    else checkToBend = true; 

                    if (item.gameObject.tag == "coin")   coinsManager.CollectCoin(item.gameObject);
                        if (item.gameObject.tag == "coinJumpbound") 
                        {
                        yield return new WaitForSeconds(0.2f);
                        item.enabled = false;
                        // Debug.Log("[1]");
                        }
                }        
            }
        

        //  checkToBend = false;  
        yield return null;
        
    }


 

    /** 
     *  Load the adapted action when the player is colliding with an object
     */
    private void OnTriggerStay2D(Collider2D item)
    {
        if (item.gameObject.tag == "coin" || (item.gameObject.tag == "coinL" && checkToRaiseL) || (item.gameObject.tag == "coinR" && checkToRaiseR) || (item.gameObject.tag == "coinB" && checkToBend))
        {
            item.gameObject.SetActive(false);
            coinsManager.CollectCoin(item.gameObject);
        }
    }

    
    void OnTriggerExit2D(Collider2D item)
     {
        checkToBend = false;   
        playerAnimator.SetBool("isLoopBend", checkToBend); 
        checkToRaiseL = false;
        playerAnimator.SetBool("leftArm", checkToRaiseL);
        checkToRaiseR = false;
        playerAnimator.SetBool("rightArm", checkToRaiseR);
     }
     
    /** 
     *  Increase the speed of the player
     */
    void IncreaseSpeed()
    {
        if (playerSpeed < maxSpeed)
        {
            playerSpeed = startSpeed + Mathf.Log10(Time.time - startTime + 1) / 10;

        }
    }

    //WARNING! CHANGE startSpeed AND startTime ( ReserSpeed() ) Between INCREASE AND DECREASE

    /** 
     *  Decrease the speed of the player
     */
    void DecreaseSpeed()
    {
        if (playerSpeed > startSpeed)
        {
            playerSpeed = startSpeed - Mathf.Log10(Time.time - startTime + 1) / 10;
        }

        else
        {
            playerSpeed = startSpeed - Mathf.Log10(Time.time - startTime + 1) / 10;
        }
    }

    /** 
     *  Reset the speed of the player
     */
    public void ResetSpeed()
    {
        startTime = Time.time;
    }

    /** 
     *  Set the data file of the player
     */
    public void SetData(string platformName)
    {
        if (data.ContainsKey(platformName))
        {
            data[platformName]++;
        }
    }

    /** 
     *  Remove the platform that the player has not yet crossed
     */
    public void ModifyData()
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("trapPlatform");
        GameObject[] water = GameObject.FindGameObjectsWithTag("water");
        string trapName = "";

        for (int i = 0; i < traps.Length; i++)
        {
            trapName = traps[i].name;
            trapName = trapName.Remove(trapName.Length - 7);
            if (traps[i].transform.position.x > gameObject.transform.position.x + 3 && data.ContainsKey(trapName)) data[trapName] = data[trapName] - 1;
        }

        for (int i = 0; i < water.Length; i++)
        {
            trapName = water[i].name;
            trapName = trapName.Remove(trapName.Length - 7);
            if (water[i].transform.position.x > gameObject.transform.position.x + 3 && data.ContainsKey(trapName)) data[trapName] = data[trapName] - 1;
        }
    }

    /** 
     *  Set the data file of the player
     */

    public int quadratic_trap(int time, int time2)
    {
        
        quadratic_t = (_at*(Mathf.Pow(time, 2))) - (_bt*time) + _ct;
        if(time > time2) 
        {
            Debug.Log("QDT Trap = " + quadratic_t);
            time2 +=15;
        }
        else if(time == time2) 
        {
            Debug.Log("QDT Trap = " + quadratic_t);
            time2 +=15; 
        }
        return time2;
    }

        public int quadratic_coin(int time, int time2)
    {
        
        quadratic_c = (_ac*(Mathf.Pow(time, 2))) - (_bc*time) + _cc;
        if(time > time2) 
        {
            Debug.Log("QDT Coin = " + quadratic_c);
            time2 +=15;
        }
        else if(time == time2) 
        {
            Debug.Log("QDT Coin = " + quadratic_c);
            time2 +=15; 
        }
        return time2;
    }
    public void SendData(string playerName)
    {
        ModifyData();
        PlayerData player = new PlayerData(playerName, data["platform5"], data["platform6"], data["platform7"], data["platform10"], data["spikes"], data["Catcher"], data["evilBlock"], data["evilSaw"], PlayerPrefs.GetInt("RandomMode"));
        DataFileManager.AddNewPlayer(player);
    }

}
