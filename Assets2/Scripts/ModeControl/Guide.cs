using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public GameObject Screen1;
    public GameObject Screen2;
    public GameObject Screen3;
    public GameObject Screen4;
    public GameObject Screen5;
    public GameObject RightBtnObj;
    public GameObject LeftBtnObj;
    public GameObject GoBtnObj;

    private int ScreenNumber { get; set; } = 0;
    private GameObject CurrentScreen { get; set; }

    void OnEnable()
    {
        ScreenNumber = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onLeftClick()
    {
        if (ScreenNumber == 4)
        {
            GoBtnObj.SetActive(false);
            RightBtnObj.SetActive(true);
        }
        ScreenNumber = ScreenNumber - 1;
        if (ScreenNumber == 0) LeftBtnObj.SetActive(false);
        if (ScreenNumber < 0) ScreenNumber = 0;
        ScreenTransition(ScreenNumber);
    }
    public void onRightClick()
    {
        ScreenNumber = ScreenNumber + 1;
        if (ScreenNumber == 1) LeftBtnObj.SetActive(true);
        if (ScreenNumber == 4)
        {
            GoBtnObj.SetActive(true);
            RightBtnObj.SetActive(false);
        }
        ScreenTransition(ScreenNumber);
    }

    private void ScreenTransition(int ScreenNumber)
    {
        if (CurrentScreen != null) CurrentScreen.SetActive(false);
        switch (ScreenNumber)
        {
            case 0:
                CurrentScreen = Screen1;
                break;
            case 1:
                CurrentScreen = Screen2;
                break;
            case 2:
                CurrentScreen = Screen3;
                break;
            case 3:
                CurrentScreen = Screen4;
                break;
            case 4:
                CurrentScreen = Screen5;
                break;
        }
        CurrentScreen.SetActive(true);

    }

    public void onBackClicked()
    {
        if (CurrentScreen != null) CurrentScreen.SetActive(false);
        Screen1.SetActive(true);
        GoBtnObj.SetActive(false);
        RightBtnObj.SetActive(true);
        LeftBtnObj.SetActive(false);
        transform.gameObject.SetActive(false);
    }

}
