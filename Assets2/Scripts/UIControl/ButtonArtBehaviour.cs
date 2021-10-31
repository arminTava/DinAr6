using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonArtBehaviour : MonoBehaviour
{
    public Text buttonText;
    public TMP_Text buttonTextDownload;
    public Button ButtonChoose;

    public void Init(string name)
    {
        buttonText.text = name;
        ButtonChoose.name = name;
    }

    public void InitDownload(string name)
    {
        buttonTextDownload.text = name;
    }
}
