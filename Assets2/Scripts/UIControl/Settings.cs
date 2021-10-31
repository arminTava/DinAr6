using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Settings : MonoBehaviour
{
    public Button InstaBtn;
    public Button WebsiteBtn;
    public Button DatenschutzBtn;
    public Button ImpressumBtn;
    public Button GuideBtn;
    public Button ShortInfoBtn;
    public Toggle TogglePointCloud;
    public Toggle ToggleFeatureDetection;
    public GameObject AROrginObject;
    public GameObject ShortInfoPanel;
    public GameObject Options;
    public Button HomeBtn;
    public GameObject StartPanel;
    public SwitchMode SwitchMode;
    public DetectionShare detectionShare;
    public GameObject Guide;


    public bool isAddFeatureDetection { get; set; }
    private bool isShortInfo { get; set; }
    private ButtonEvents buttonEvents;

    // Start is called before the first frame update
    void Start()
    {
        InstaBtn.onClick.AddListener(delegate { OnNavigate(Navigation.Insta); });
        WebsiteBtn.onClick.AddListener(delegate { OnNavigate(Navigation.WebsiteHome); });
        DatenschutzBtn.onClick.AddListener(delegate { OnNavigate(Navigation.Datenschutz); });
        ImpressumBtn.onClick.AddListener(delegate { OnNavigate(Navigation.Impressum); });
        GuideBtn.onClick.AddListener(onGuideClicked);
        HomeBtn.onClick.AddListener(onHomeClicked);
        ShortInfoBtn.onClick.AddListener(delegate { OnShortInfo(); });
        TogglePointCloud.onValueChanged.AddListener(OnToggleTogglePointCloudValueChanged);
        ToggleFeatureDetection.onValueChanged.AddListener(OnToggleFeatureDetection);
        buttonEvents = FindObjectOfType<ButtonEvents>();
    }

    private void onGuideClicked()
    {
        transform.gameObject.SetActive(false);
        Guide.SetActive(true);
    }

    public void onHomeClicked()
    {
        StartPanel.SetActive(true);
        SwitchMode.MarkerInteraction.SetActive(false);
        SwitchMode.SurfaceInteraction.SetActive(false);
        transform.gameObject.SetActive(false);
        detectionShare.SetURLName(new AssetItem() { name = "None", urlpath = "https://artvisity.artakten.de/" });
    }

    private void OnOKSettingsClicked()
    {
        transform.gameObject.SetActive(false);
        buttonEvents.isPanelVisible = false;
        ShortInfoPanel.SetActive(false);
    }

    private void OnShortInfo()
    {
        isShortInfo = !isShortInfo;
        ShortInfoPanel.SetActive(isShortInfo);
    }

    private void OnNavigate(Navigation navigateTo)
    {
        if (navigateTo == Navigation.WebsiteHome) Application.OpenURL("https://artvisity.artakten.de/");
        else if (navigateTo == Navigation.Insta) Application.OpenURL("https://www.instagram.com/digitalartandculture/");
        else if(navigateTo == Navigation.Datenschutz) Application.OpenURL("https://artvisity.artakten.de/datenschutzerklaerung/");
        else if (navigateTo == Navigation.Impressum) Application.OpenURL("https://artvisity.artakten.de/impressum/");
        else if (navigateTo == Navigation.Guide) Application.OpenURL("https://artvisity.artakten.de/guide");
    }

    private void OnToggleFeatureDetection(bool isFeatureDetection)
    {
        if (isFeatureDetection) isAddFeatureDetection = true;
        else isAddFeatureDetection = false;
    }

    public void OnToggleTogglePointCloudValueChanged(bool isPointCloudVisible)
    {
        if (isPointCloudVisible)
        {
            var pointCloudManager = AROrginObject.GetComponent<ARPointCloudManager>();
            pointCloudManager.enabled = true;
            foreach (ARPointCloud pointCLoud in pointCloudManager.trackables)
            {
                pointCLoud.gameObject.SetActive(true);
            }
        }
        else
        {
            var pointCloudManager = AROrginObject.GetComponent<ARPointCloudManager>();
            pointCloudManager.enabled = false;
            foreach (ARPointCloud pointCLoud in pointCloudManager.trackables)
            {
                pointCLoud.gameObject.SetActive(false);
            }
        }
    }

    enum Navigation
    {
        Insta, 
        WebsiteHome, 
        Datenschutz, 
        Impressum, 
        Guide
    }
}
