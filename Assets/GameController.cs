using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    enum State
    {
        SHOW_SPLASHSCREEN,
        ASK_ENTER,
        SHOW_MENU
    }

    private State CurrentState;

    [Header("SplashScreen")]
    public UnityEngine.UI.Image SplashScreen;
    public float SS_FadeInTime = 1;
    public float SS_FadeOutTime = 1;
    public float SS_StayTime = 2;

    private float SplashScreenTimer = 0;

    [Header("Ask Enter")]
    public UnityEngine.UI.Text AskEnter;
    public float AE_FadeInTime = 1;
    public float AE_FadeOutTime = 1;

    private float AskEnterTimer = 0;
    private bool AE_StartFadeOut = false;

    [Header("Main Menu")]
    // public UnityEngine.UI.Button StartButton;
    // public UnityEngine.UI.Button ExitButton;
    public GameObject ButtonParent;

	// Use this for initialization
	void Start () {
        SwitchState(State.SHOW_SPLASHSCREEN);
	}
	
	// Update is called once per frame
	void Update () {
        switch(CurrentState)
        {
            case State.SHOW_SPLASHSCREEN:
                UpdateSplashScreen(Time.deltaTime);
                break;
            case State.ASK_ENTER:
                UpdateAskEnter(Time.deltaTime);
                break;
            case State.SHOW_MENU:
                UpdateShowMenuMode();
                break;

            default:
                break;
        }
	}

    void SwitchState(State newState)
    {
        DisableEverything();
        switch (newState)
        {
            case State.SHOW_SPLASHSCREEN:
                InitSplashScreenMode();
                CurrentState = newState;
                break;
            case State.ASK_ENTER:
                InitAskEnterMode();
                CurrentState = newState;
                break;
            case State.SHOW_MENU:
                InitShowMenuMode();
                CurrentState = newState;
                break;

            default:
                break;
        }
    }

    void DisableEverything()
    {
        SplashScreen.gameObject.SetActive(false);
        AskEnter.gameObject.SetActive(false);
        ButtonParent.SetActive(false);
    }

    void InitSplashScreenMode()
    {
        if(SplashScreen != null)
        {
            SplashScreen.gameObject.SetActive(true);
            SplashScreen.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            SplashScreenTimer = 0;
        }
    }

    void InitAskEnterMode()
    {
        if(AskEnter != null)
        {
            AskEnter.gameObject.SetActive(true);
            AskEnter.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            AskEnterTimer = 0;
            AE_StartFadeOut = false;
        }
    }

    void InitShowMenuMode()
    {
        if(ButtonParent != null)
        {
            ButtonParent.SetActive(false);
        }
    }

    void UpdateSplashScreen(float dt)
    {
        if (SplashScreen == null)
            return;

        if(Input.GetButton("Submit"))
        {
            InitSplashScreenMode();
            SwitchState(State.ASK_ENTER);
        }

        SplashScreenTimer += dt;
        if(SplashScreenTimer > SS_FadeInTime+SS_FadeOutTime+SS_StayTime)
        {
            InitSplashScreenMode();
            SwitchState(State.ASK_ENTER);
        }
        else if(SplashScreenTimer < SS_FadeInTime)
        {
            float alpha = Mathf.SmoothStep(0, 1, SplashScreenTimer / SS_FadeInTime);
            SplashScreen.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
        else if(SplashScreenTimer >= SS_FadeInTime && SplashScreenTimer < SS_StayTime + SS_FadeInTime)
        {
            SplashScreen.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else if(SplashScreenTimer >= SS_StayTime + SS_FadeInTime && SplashScreenTimer < SS_StayTime + SS_FadeInTime + SS_FadeOutTime)
        {
            float alpha = Mathf.SmoothStep(1, 0, (SplashScreenTimer - SS_StayTime - SS_FadeInTime) / SS_FadeOutTime);
            SplashScreen.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }

    void UpdateAskEnter(float dt)
    {
        if (!AE_StartFadeOut && Input.GetButton("Submit"))
        {
            AE_StartFadeOut = true;
            AskEnterTimer = 0;
        }

        if(AE_StartFadeOut)
        {
            AskEnterTimer += dt;
            if(AskEnterTimer >= AE_FadeOutTime)
            {
                InitAskEnterMode();
                SwitchState(State.SHOW_MENU);
                return;
            }
            
            float alpha = Mathf.SmoothStep(1, 0, AskEnterTimer / AE_FadeOutTime);
            AskEnter.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
        else if(AskEnterTimer < AE_FadeInTime)
        {
            AskEnterTimer += dt; 
            
            float alpha = Mathf.SmoothStep(0, 1, AskEnterTimer / AE_FadeInTime);
            AskEnter.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }

    void UpdateShowMenuMode()
    {
        if (!ButtonParent.activeSelf)
        {
            Debug.Log("Button.activeSelf==false");
            ButtonParent.SetActive(true);
        }
    }

    public void OnButtonClicked(string command)
    {
        Debug.Log("Click command is: " + command);
        if(command == "Start")
        {
            Application.LoadLevel("Main");
        }

        if(command == "Exit")
        {
            Application.Quit();
        }
    }
}
