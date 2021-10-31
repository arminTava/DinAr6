using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchMode : MonoBehaviour
{
    public Toggle ToggleMarker;
    public Toggle ToggleSurface;
    public Toggle ToggleGPS;
    public GameObject SurfaceInteraction;
    public GameObject MarkerInteraction;
    public GameObject GPSInteraction;
    public GameObject TranslateButtons;
    public GameObject RotateButtons;
    public DetectionShare DetectionShare;
    public GameObject SwitchingInfo;


    private ButtonEvents buttonEvents;

    // Start is called before the first frame update
    void Start()
    {
        buttonEvents = FindObjectOfType<ButtonEvents>();
        ToggleSurface.onValueChanged.AddListener(OnToggleSurfaceValueChanged);
        ToggleMarker.onValueChanged.AddListener(OnToggleMarkerDetection);
        ToggleGPS.onValueChanged.AddListener(OnToggleGPSDetection);
    }

    private void OnToggleMarkerDetection(bool isActive)
    {
        if (isActive)
        {
            var surfaceInt = SurfaceInteraction.GetComponent<ARTapToPlaceObject>();
            if(surfaceInt.artModels != null)
            {
                if (surfaceInt.artModels.bundle != null) surfaceInt.artModels.bundle.Unload(false);
                if (surfaceInt.CurrentObject != null) Destroy(surfaceInt.CurrentObject);
            }
            GPSInteraction.GetComponent<GPSDetection>().UnloadAndDestroy();
            MarkerInteraction.gameObject.SetActive(true);
            SurfaceInteraction.gameObject.SetActive(false);
            GPSInteraction.gameObject.SetActive(false);
            //buttonEvents.SurfaceEvents.AllArtButton.gameObject.SetActive(false);
            //buttonEvents.SurfaceEvents.YourArtButton.gameObject.SetActive(false);
            buttonEvents.CallMarkerEvent();
            //buttonEvents.MarkerEvents.PositionMarkerButton();
            if (buttonEvents.isMenuPanelVisible) buttonEvents.onMenuClicked();
            //TranslateButtons.SetActive(false);
            var posRot = RotateButtons.transform.position;
            posRot.x = Screen.width * 0.65f;
            //RotateButtons.transform.position = posRot;
            ToggleMarker.interactable = false;
            ToggleSurface.interactable = true;
            ToggleGPS.interactable = true;
            buttonEvents.showToast("", 1);
            buttonEvents.ArrowScanObject.SetActive(false);
            if (!DetectionShare.isMarkerFirstLoaded) DetectionShare.SwitchingInfo.SetActive(true);
            //var markerdetectionScript = MarkerInteraction.GetComponent<MarkerDetection>();
            //if (markerdetectionScript.CurrentObjectMarker != null) Destroy(markerdetectionScript.CurrentObjectMarker);
        }
    }

    private void OnToggleSurfaceValueChanged(bool isActive)
    {
        if (isActive)
        {
            MarkerInteraction.GetComponent<MarkerDetection>().UnloadAndDestroy();
            GPSInteraction.GetComponent<GPSDetection>().UnloadAndDestroy();
            MarkerInteraction.gameObject.SetActive(false);
            SurfaceInteraction.gameObject.SetActive(true);
            GPSInteraction.gameObject.SetActive(false);
            //buttonEvents.SurfaceEvents.AllArtButton.gameObject.SetActive(true);
            //buttonEvents.SurfaceEvents.YourArtButton.gameObject.SetActive(true);   
            //buttonEvents.MarkerEvents.MarkerButton.gameObject.SetActive(false);
            buttonEvents.CallSurfaceEvents();
            if (buttonEvents.isMenuPanelVisible) buttonEvents.onMenuClicked();
            TranslateButtons.SetActive(true);
            var posRot = RotateButtons.transform.position;
            posRot.x = Screen.width * 0.5f;
            //RotateButtons.transform.position = posRot;
            ToggleMarker.interactable = true;
            ToggleSurface.interactable = false;
            ToggleGPS.interactable = true;
            if (!DetectionShare.isSurfaceFirstLoaded) DetectionShare.SwitchingInfo.SetActive(true);
            //var surfacedetectionScript = SurfaceInteraction.GetComponent<ARTapToPlaceObject>();
            //if (surfacedetectionScript.CurrentObject != null) Destroy(surfacedetectionScript.CurrentObject);
        }
    }

    private void OnToggleGPSDetection(bool isActive)
    {
        if (isActive)
        {
            var surfaceInt = SurfaceInteraction.GetComponent<ARTapToPlaceObject>();
            if (surfaceInt.artModels != null)
            {
                if (surfaceInt.artModels.bundle != null) surfaceInt.artModels.bundle.Unload(false);
                if (surfaceInt.CurrentObject != null) Destroy(surfaceInt.CurrentObject);
            }
            MarkerInteraction.GetComponent<MarkerDetection>().UnloadAndDestroy();
            MarkerInteraction.gameObject.SetActive(false);
            SurfaceInteraction.gameObject.SetActive(false);
            GPSInteraction.gameObject.SetActive(true);
            buttonEvents.CallGPSEvent();
            if (buttonEvents.isMenuPanelVisible) buttonEvents.onMenuClicked();
            ToggleGPS.interactable = false;
            ToggleMarker.interactable = true;
            ToggleSurface.interactable = true;
            buttonEvents.showToast("", 1);
            buttonEvents.ArrowScanObject.SetActive(false);
            if (!DetectionShare.isGPSFirstLoaded) DetectionShare.SwitchingInfo.SetActive(true);
            //var markerdetectionScript = MarkerInteraction.GetComponent<MarkerDetection>();
            //if (markerdetectionScript.CurrentObjectMarker != null) Destroy(markerdetectionScript.CurrentObjectMarker);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
