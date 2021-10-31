using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject SpecialBackupObject;
    public GameObject aRPlaneManagerObject;
    public GameObject placementIndicator;
    public GameObject SettingsObject;

    //Animation 
    //public Button AnimationBtn;
    //public Sprite StartImage;
    //public Sprite PauseImage;

    private ARRaycastManager arOrigin;
    private Camera arCamera;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private int currentArtIndex;
    public GameObject CurrentObject { get; set; }
    private bool isVertical;
    private ButtonEvents buttonEvents;
    private Settings Settings;
    public ArtModels artModels { get; set; }

    private long LastViewInSec { get; set; }
    private long LastVisibleInSec { get; set; }
    private List<int> CheckFrequency { get; set; }

    bool c;

    private void Awake()
    {
        //if (artModels.DetectionShare.isSurfaceFirstLoaded && artModels.DetectionShare.isSurfaceBegining)
        //{
        //    if (Application.internetReachability == NetworkReachability.NotReachable) buttonEvents.showErrorInfoBox(4, LanguageInfo.NoWifiFeatureInfo);
        //    else StartCoroutine(artModels.InstantiateItemsInYourArtListAndLoadFromServer());
        //}
        arOrigin = FindObjectOfType<ARRaycastManager>();
        if (!Directory.Exists(Path.GetDirectoryName(FileService.AssetPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FileService.AssetPath));
        }
        arCamera = FindObjectOfType<Camera>();
        buttonEvents = FindObjectOfType<ButtonEvents>();
        buttonEvents.CallSurfaceEvents();
        Settings = SettingsObject.GetComponent<Settings>();
        artModels = FindObjectOfType<ArtModels>();
        //artModels.InstantiateItemsInYourArtList();
        if (Application.internetReachability == NetworkReachability.NotReachable) buttonEvents.showErrorInfoBox(4, LanguageInfo.NoWifiFeatureInfo);

    }
    private void OnEnable()
    {
        if (!artModels.DetectionShare.isSurfaceFirstLoaded) StartCoroutine(artModels.InstantiateItemsInYourArtListAndLoadFromServer());
        LastViewInSec = 0;
        CheckFrequency = new List<int>();
    }

    void Start()
    {
        ARPlaneManager aRPlaneManager = aRPlaneManagerObject.GetComponent<ARPlaneManager>();
        aRPlaneManager.planesChanged += OnPlaneChanged;
    }
   

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        //MoveObjectWithCross();
        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (IsPointerOverUIObject() == false) PlaceObject();
        }
        if ((DateTimeOffset.Now.ToUnixTimeSeconds()- LastViewInSec) > 4 && !buttonEvents.isPanelVisible)
        {
            if ((DateTimeOffset.Now.ToUnixTimeSeconds() - LastVisibleInSec)>5)
            {
                if(!buttonEvents.isArrowScanNotAllowed)
                {
                    CheckFrequency.Add(1);
                    buttonEvents.showScanToast(4); //showToast("Bewege deine Kamera und scanne deinen Raum", 3);
                    LastVisibleInSec = DateTimeOffset.Now.ToUnixTimeSeconds() + 3;
                }
            }
        }
        if (CheckFrequency.Count != 0 && (CheckFrequency.Count % 3) == 0) buttonEvents.showToast(LanguageInfo.AvoidMonoInfo, 3);
        if (CheckFrequency.Count != 0 && (CheckFrequency.Count % 6) == 0) buttonEvents.showToast(LanguageInfo.AdditionalDetInfo, 3);
        //TestTXT.text = arCamera.transform.eulerAngles.ToString();
        //PlayerPrefs.SetInt("te", 0);
        //if (PlayerPrefs.GetInt("te") != 1)
        //{
        //    PlayerPrefs.SetInt("te", 1);
        //    //buttonEvents.isPanelVisible = true;
        //    PlaceObject(); /////// ---------------------------------------------------------LÖSCHEN

        //}

    }


    private void PlaceObject()
    {
        if (CurrentObject != null) Destroy(CurrentObject);
        if (artModels.bundle != null)
        {
            string rootAssetPath = artModels.bundle.GetAllAssetNames()[0];
            var gameObsInst = artModels.bundle.LoadAsset(rootAssetPath) as GameObject;
            InstantiateModel(gameObsInst);
            artModels.ToolBoxControl.CurrentARObject = CurrentObject;
            //if (artModels.isAnimationAvailable) // -------------------------------------------------------------------------HIER NOCHMAL PRÜFEN
            //{
            //    AnimationBtn.interactable = true;
            //    AnimationBtn.GetComponent<Image>().sprite = StartImage;
            //    //hier auf play verweisen mit einem Pfeil ............................TODO
            //    checkVideo(CurrentObject);
            //}
        }
        else if (buttonEvents.ASpecialNotSaved)
        {
            //buttonEvents.ASpecialNotSaved = false;
            InstantiateModel(SpecialBackupObject);
        }
        else
        {
            buttonEvents.showToast(LanguageInfo.SelectArtInfo, 3);
        }
    }
    private void InstantiateModel(GameObject gameObsInst)
    {
        var cameraForward = arCamera.transform.forward;
        var rotation = Quaternion.Euler(gameObsInst.transform.eulerAngles.x, arCamera.transform.eulerAngles.y + gameObsInst.transform.eulerAngles.y, gameObsInst.transform.eulerAngles.z);//Quaternion.LookRotation(new Vector3(0,cameraForward.y,0).normalized);
        CurrentObject = Instantiate(gameObsInst, placementPose.position, rotation);
    }





    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && buttonEvents.isIndicatorAllowVisible)
        {
            placementIndicator.SetActive(true);
            CheckFrequency.Clear();
            if (isVertical)
            {
                var zDiff = 90 + arCamera.transform.eulerAngles.z;
                //var cameraForward = arCamera.transform.forward;
                //var cameraBearing = new Vector3(cameraForward.x, cameraForward.y,0 ).normalized;
                //placementPose.rotation = Quaternion.LookRotation(cameraBearing);
                var rotInd = Quaternion.Euler(arCamera.transform.eulerAngles.x - 90, arCamera.transform.eulerAngles.y, 0);
                placementPose.rotation = rotInd;
            }
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            LastViewInSec = DateTimeOffset.Now.ToUnixTimeSeconds();
            buttonEvents.isAborted = true;
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hitsPlane = new List<ARRaycastHit>();
        var hits = new List<ARRaycastHit>();
        if (Settings.isAddFeatureDetection)
        {
            arOrigin.Raycast(screenCenter, hits, TrackableType.FeaturePoint); //PlaneWithinPolygon
        }
        arOrigin.Raycast(screenCenter, hitsPlane, TrackableType.PlaneWithinPolygon); //PlaneWithinPolygon
        if (hitsPlane.Count > 0 || hits.Count > 0) placementPoseIsValid = true;
        else placementPoseIsValid = false;
        //placementPoseIsValid = hitsPlane.Count > 0;
        if (placementPoseIsValid)
        {
            //placementPoseIsValid = true;
            if (hitsPlane.Count > 0)
            {
                placementPose = hitsPlane[0].pose;
            }
            else
            {
                if (Settings.isAddFeatureDetection)
                {
                    placementPose = hits[0].pose;
                }
            }
            var cameraForward = arCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            //var cameraBearing2 = new Vector3(0, cameraForward.y, cameraForward.z).normalized;//new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing); //* Quaternion.LookRotation(cameraBearing);
            //TestTXT2.text = placementPose.rotation.ToString();
        }
    }

    private void OnPlaneChanged(ARPlanesChangedEventArgs obj)
    {
        List<ARPlane> addedPlanes = obj.added;
        var Count = addedPlanes.Count;
        if (Count > 0)
        {
            ARPlane currentPlane = addedPlanes[addedPlanes.Count - 1];
            //TestTXT.text = currentPlane.alignment.ToString();
            if (currentPlane.alignment == PlaneAlignment.Vertical) isVertical = true;
            else isVertical = false;
        }
    }
    public static bool IsPointerOverUIObject()
    {
        bool isOverTaggedElement = false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; ++i)
            {
                if (results[i].gameObject.CompareTag("ROTBTN")) isOverTaggedElement = true;
            }
        }        
        return isOverTaggedElement;
    }

}
