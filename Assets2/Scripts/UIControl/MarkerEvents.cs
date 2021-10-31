using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarkerEvents : MonoBehaviour
{
    public Button MarkerButton;
    public GameObject MarkerPanel;
    public Settings settings;
    public GameObject placementIndicator;
    public GameObject MarkerDetectionAnim;
    public TMP_Text toast;
    public bool isAnimAbort { get; set; }
    // Start is called before the first frame update
    void OnEnable()
    {
        MarkerButton.gameObject.SetActive(true);
        settings.OnToggleTogglePointCloudValueChanged(false);
        var toggles = settings.Options.transform.GetComponentsInChildren<Toggle>();
        foreach (var t in toggles) { t.interactable = false; }
        placementIndicator.SetActive(false);
        toast.text = "";
    }
    void OnDisable()
    {
        if (MarkerButton != null) MarkerButton.gameObject.SetActive(false);

    }

    public void setMarkerPanel(bool isActice )
    {
        MarkerPanel.SetActive(isActice);
    }

    public void PositionMarkerButton()
    {
        Vector3 pos = MarkerButton.gameObject.transform.position;
        pos.y = Screen.height * 0.8f;
        MarkerButton.transform.position = pos;
        MarkerButton.gameObject.SetActive(true);
    }

    public void CallDetectionAnim(int duration)
    {
        StartCoroutine(StartMarkerAnim(duration));
    }

    IEnumerator StartMarkerAnim(int duration)
    {
        MarkerDetectionAnim.SetActive(true);
        float counter = 0;
        while (counter < duration)
        {
            if (isAnimAbort) break;
            counter += Time.deltaTime;
            yield return null;
        }
        isAnimAbort = false;
        MarkerDetectionAnim.SetActive(false);
    }

}
