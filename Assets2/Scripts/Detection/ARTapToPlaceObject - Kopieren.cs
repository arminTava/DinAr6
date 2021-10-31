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

public class ARTapToPlaceObject2 : MonoBehaviour
{
    public GameObject placementIndicator;
    public API api;

    private ARRaycastManager arOrigin;
    private Camera arCamera;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private int currentArtIndex;
    private GameObject CurrentObject;

    public GameObject ButtonPrefab;
    public GameObject SpecialButtonPrefab;
    public GameObject ButtonPrefabDownload;
    public GameObject ButtonPrefabDownloadError;
    public Transform ButtonParent;
    public Transform ButtonParentAll;
    private AssetBundle bundle;
    private List<AssetItem> itemsDisc;
    private Color orginalColor;
    private TMP_Text CurrentDownloadButtonText;
    private Button CurrentuiDownloadButton;
    public TMP_Text ChoosedKunstwerk;
    //public TMP_Text TestTXT2;
    private bool isVertical;
    private ButtonEvents buttonEvents;

    public GameObject aRPlaneManagerObject;
    private GameObject Currenttest;
    private long LastViewInSec { get; set; }
    private long LastVisibleInSec { get; set; }
    private string assetNameSpecial { get; set; }
    public GameObject SpecialBackupObject; 
    public Button RefreshItemsBtn;
    //public Dropdown dropdown; 

    //void Start()
    //{
    //    assetNameSpecial = "A&ASpecial";
    //    arOrigin = FindObjectOfType<ARRaycastManager>();
    //    arCamera = FindObjectOfType<Camera>();
    //    buttonEvents = FindObjectOfType<ButtonEvents>();
    //    InstantiateItemsInYourArtList();
    //    if (Application.internetReachability == NetworkReachability.NotReachable) buttonEvents.showToast("No Internet connection. Not all features are available", 3);
    //    HandleArtSpecial();
    //    InstantiateItemsInAllArtList();
    //    ARPlaneManager aRPlaneManager = aRPlaneManagerObject.GetComponent<ARPlaneManager>();
    //    aRPlaneManager.planesChanged += OnPlaneChanged;
    //    LastViewInSec = 0;
    //    PrepareToRefreshItems(RefreshItemsBtn);
    //}

    //void Update()
    //{
    //    UpdatePlacementPose();
    //    UpdatePlacementIndicator();
    //    TestTXT.text = arCamera.transform.forward.ToString();
    //    if (buttonEvents.isPressedMinus)
    //    {
    //        if (CurrentObject != null) CurrentObject.transform.Rotate(Vector3.down * 20 * Time.deltaTime);
    //    }
    //    if (buttonEvents.isPressedPlus)
    //    {
    //        if (CurrentObject != null) CurrentObject.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    //    }
    //    if (buttonEvents.isPressedTransMinus)
    //    {
    //        if (CurrentObject != null) CurrentObject.transform.Translate(Vector3.down * 3 * Time.deltaTime);
    //    }
    //    if (buttonEvents.isPressedTransPlus)
    //    {
    //        if (CurrentObject != null) CurrentObject.transform.Translate(Vector3.up * 3 * Time.deltaTime);
    //    }
    //    if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        if (IsPointerOverUIObject() == false) PlaceObject();
    //        TestTXT.text = CurrentObject.transform.rotation.ToString();
    //    }
    //    if ((DateTimeOffset.Now.ToUnixTimeSeconds()- LastViewInSec) > 4 && !buttonEvents.isPanelVisible)
    //    {
    //        if ((DateTimeOffset.Now.ToUnixTimeSeconds() - LastVisibleInSec)>5)
    //        {
    //            buttonEvents.showScanToast(3); //showToast("Bewege deine Kamera und scanne deinen Raum", 3);
    //            LastVisibleInSec = DateTimeOffset.Now.ToUnixTimeSeconds() + 3;
    //        }
    //    }
    //    TestTXT.text = arCamera.transform.eulerAngles.ToString();

    //}
    //private void PlaceObject()
    //{
    //    if (CurrentObject != null) Destroy(CurrentObject);
    //    if (bundle != null)
    //    {
    //        string rootAssetPath = bundle.GetAllAssetNames()[0];
    //        var gameObsInst = bundle.LoadAsset(rootAssetPath) as GameObject;
    //        placementPose.rotation = gameObsInst.transform.rotation.normalized;
    //        InstantiateModel(gameObsInst);
    //    }
    //    else if (buttonEvents.ASpecialNotSaved)
    //    {
    //        buttonEvents.ASpecialNotSaved = false;
    //        InstantiateModel(SpecialBackupObject);
    //    }
    //    else
    //    {
    //        buttonEvents.showToast("Please select a piece of art", 3);
    //    }
    //}
    //private void InstantiateModel(GameObject gameObsInst)
    //{
    //    var cameraForward = arCamera.transform.forward;
    //    var rotation = Quaternion.Euler(gameObsInst.transform.eulerAngles.x, arCamera.transform.eulerAngles.y + gameObsInst.transform.eulerAngles.y, gameObsInst.transform.eulerAngles.z);//Quaternion.LookRotation(new Vector3(0,cameraForward.y,0).normalized);
    //    CurrentObject = Instantiate(gameObsInst, placementPose.position, rotation);
    //}

    //private void UpdatePlacementIndicator()
    //{
    //    if (placementPoseIsValid)
    //    {
    //        placementIndicator.SetActive(true);
    //        if (isVertical)
    //        {
    //            var zDiff = 90 + arCamera.transform.eulerAngles.z;
    //            var cameraForward = arCamera.transform.forward;
    //            var cameraBearing = new Vector3(cameraForward.x, cameraForward.y,0 ).normalized;
    //            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
    //            var rotInd = Quaternion.Euler(arCamera.transform.eulerAngles.x - 90, arCamera.transform.eulerAngles.y, 0);
    //            placementPose.rotation = rotInd;
    //        }
    //        placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
    //        LastViewInSec = DateTimeOffset.Now.ToUnixTimeSeconds();
    //        buttonEvents.isAborted = true;
    //    }
    //    else
    //    {
    //        placementIndicator.SetActive(false);
    //    }
    //}

    //private void UpdatePlacementPose()
    //{
    //    var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
    //    var hits = new List<ARRaycastHit>();
    //    arOrigin.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);
    //    placementPoseIsValid = hits.Count > 0;
    //    if (placementPoseIsValid)
    //    {
    //        placementPoseIsValid = true;
    //        placementPose = hits[0].pose;
    //        var cameraForward = arCamera.transform.forward;
    //        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
    //        var cameraBearing2 = new Vector3(0, cameraForward.y, cameraForward.z).normalized;//new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
    //        placementPose.rotation = Quaternion.LookRotation(cameraBearing); //* Quaternion.LookRotation(cameraBearing);
    //        TestTXT2.text = placementPose.rotation.ToString();
    //    }
    //}
    //private void HandleArtSpecial()
    //{
    //    Transform buttonChooseTransform = SpecialButtonPrefab.transform.GetChild(1);
    //    Button uiButtonChoose = buttonChooseTransform.GetComponent<Button>();
    //    uiButtonChoose.onClick.AddListener(delegate { onItemButtonClicked(assetNameSpecial); });
    //    if (Application.internetReachability != NetworkReachability.NotReachable) api.GetBundleObject(assetNameSpecial, OnSpecialLoaded);
    //    else HandleSpecialError();
    //}
    //private void HandleSpecialError()
    //{
    //    var filename = assetNameSpecial + ".unity3d";
    //    string assetPath = Path.Combine(FileService.AssetPath, filename);
    //    if (!File.Exists(assetPath)) buttonEvents.ASpecialNotSaved = true;
    //}

    //private void OnSpecialLoaded(AssetItem arg0, bool isEror)
    //{
    //    if (isEror) HandleSpecialError();
    //}

    //private void OnPlaneChanged(ARPlanesChangedEventArgs obj)
    //{
    //    List<ARPlane> addedPlanes = obj.added;
    //    var Count = addedPlanes.Count;
    //    if (Count > 0)
    //    {
    //        ARPlane currentPlane = addedPlanes[addedPlanes.Count - 1];
    //        TestTXT.text = currentPlane.alignment.ToString();
    //        if (currentPlane.alignment == PlaneAlignment.Vertical) isVertical = true;
    //        else isVertical = false;
    //    }
    //}
    //public static bool IsPointerOverUIObject()
    //{
    //    bool isOverTaggedElement = false;
    //    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    //    eventDataCurrentPosition.position = Input.GetTouch(0).position;
    //    List<RaycastResult> results = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    //    if (results.Count > 0)
    //    {
    //        for (int i = 0; i < results.Count; ++i)
    //        {
    //            if (results[i].gameObject.CompareTag("ROTBTN")) isOverTaggedElement = true;
    //        }
    //    }        
    //    return isOverTaggedElement;
    //}


    //load and instanitate Items from disc
    //private void InstantiateItemsInYourArtList()
    //{
    //    if (!Directory.Exists(FileService.AssetPath))
    //    {
    //        Directory.CreateDirectory(FileService.AssetPath);
    //    }
    //    itemsDisc = ReadAssetItemsFromListDisc();
    //    foreach (AssetItem assetItem in itemsDisc)
    //    {
    //        InstatiateItemInYourList(assetItem);
    //    }
    //}
    //private void InstatiateItemInYourList(AssetItem assetItem, bool isDownloaded = false)
    //{
    //    GameObject ArtItemObject = Instantiate(ButtonPrefab, ButtonParent);
    //    ButtonArtBehaviour buttonBehavior = ArtItemObject.GetComponent<ButtonArtBehaviour>();
    //    buttonBehavior.Init(assetItem.name);
    //    Transform buttonChooseTransform = ArtItemObject.transform.GetChild(1);
    //    Transform buttonDeleteTransform = ArtItemObject.transform.GetChild(0);
    //    Button uiButtonChoose = buttonChooseTransform.GetComponent<Button>();
    //    Button uiButtonDelete = buttonDeleteTransform.GetComponent<Button>();
    //    uiButtonChoose.onClick.AddListener(delegate { onItemButtonClicked(assetItem.name); });
    //    uiButtonDelete.onClick.AddListener(delegate { onItemButtonDelete(assetItem.name, ArtItemObject); });
    //    if (isDownloaded) itemsDisc.Add(new AssetItem() { name = assetItem.name });
    //}

    //private void onItemButtonDelete(string name, GameObject ArtItemObject)
    //{
    //    var nameUnity = name + ".unity3d";
    //    string assetPath = Path.Combine(FileService.AssetPath, nameUnity);
    //    itemsDisc.RemoveAt(itemsDisc.FindIndex((x) => x.name == name));
    //    File.Delete(assetPath);
    //    Destroy(ArtItemObject);
    //    MakeModelDownloadable(name);
    //}

    //private void onItemButtonClicked(string name)
    //{        
    //    var buttonEvents = FindObjectOfType<ButtonEvents>();
    //    buttonEvents.ItemClicked(false);
    //    ChoosedKunstwerk.text = name; 
    //    name = name + ".unity3d";
    //    string assetPath = Path.Combine(FileService.AssetPath, name);
    //    StartCoroutine(LoadAssetBundle(assetPath));
    //    bundle = AssetBundle.LoadFromFile(assetPath);
    //}

    //IEnumerator LoadAssetBundle(string assetpath)
    //{
    //    if (bundle != null) bundle.Unload(false);
    //    if (File.Exists(assetpath))
    //    {
    //        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(assetpath);
    //        yield return bundleLoadRequest;
    //        bundle = bundleLoadRequest.assetBundle;
    //    }
    //}

    //private List<AssetItem> ReadAssetItemsFromListDisc()
    //{
    //    var files = Directory.GetFiles(FileService.AssetPath, "*.unity3d", SearchOption.TopDirectoryOnly);
    //    List<AssetItem> items = new List<AssetItem>();
    //    foreach (string file in files)
    //    {
    //        string[] stringSeparator = new string[] { "/" };
    //        string[] fileList1 = file.Split(stringSeparator, StringSplitOptions.None);
    //        string[] stringSeparator2 = new string[] { "." };
    //        var Count = fileList1.Length;
    //        var fileRealList = fileList1[Count - 1].Split(stringSeparator2, StringSplitOptions.None);
    //        string RealName = fileRealList[0];
    //        if (RealName != assetNameSpecial) items.Add(new AssetItem() { name = RealName });
    //    }    
    //    return items;
    //}

    // download Assets and add to List from disc
    //public void InstantiateItemsInAllArtList()
    //{
    //    DestroyAllButtons();
    //    api.GetItemList(OnDownloadItemLoaded);
    //}
    //void OnDownloadItemLoaded(List<AssetItem> itemList, bool isError = false)
    //{
    //    if (isError)
    //    {
    //        GameObject buttonObject = Instantiate(ButtonPrefabDownloadError, ButtonParentAll);
    //        return;
    //    }
    //    foreach (AssetItem assetItem in itemList)
    //    {
    //        if (assetItem.name != assetNameSpecial)
    //        {
    //            var Count = itemsDisc.Where((x) => x.name == assetItem.name).ToList().Count();
    //            GameObject ArtItemObjectDownload = Instantiate(ButtonPrefabDownload, ButtonParentAll);
    //            ButtonArtBehaviour buttonBehavior = ArtItemObjectDownload.GetComponent<ButtonArtBehaviour>();
    //            buttonBehavior.InitDownload(assetItem.name);
    //            set button action
    //            Transform buttonDownloadTransform = ArtItemObjectDownload.transform.GetChild(0);
    //            Button uiDownloadButton = buttonDownloadTransform.GetComponent<Button>();
    //            Transform textDownloadTransform = ArtItemObjectDownload.transform.GetChild(1);
    //            TMP_Text txt = textDownloadTransform.GetComponent<TMP_Text>();
    //            System.Threading.Thread.Sleep(50);
    //            if (Count != 0)
    //            {
    //                uiDownloadButton.interactable = false;
    //                orginalColor = txt.color;
    //                txt.color = Color.gray;
    //            }
    //            uiDownloadButton.onClick.AddListener(() => { OnDownloadButtonClicked(uiDownloadButton, txt, assetItem); });
    //        }         
    //    }
    //}
    //private void OnDownloadButtonClicked(Button uiDownloadButton, TMP_Text txt, AssetItem assetItem)
    //{
    //    uiDownloadButton.interactable = false;
    //    txt.color = Color.gray;
    //    var buttonEvents = FindObjectOfType<ButtonEvents>();
    //    buttonEvents.DownloadInfo.SetActive(false);
    //    buttonEvents.showToast("Your artwork is downloading...", 3);
    //    buttonEvents.ShowDownloadPanel(true);
    //    CurrentuiDownloadButton = uiDownloadButton;
    //    CurrentDownloadButtonText = txt;
    //    RefreshItemsBtn.interactable = false;
    //    api.GetBundleObject(assetItem.name, OnContentLoaded);
    //}

    //public void OnContentLoaded(AssetItem assetItem, bool isError = false)
    //{
    //    RefreshItemsBtn.interactable = true;
    //    if (isError)
    //    {
    //        CurrentuiDownloadButton.interactable = true;
    //        CurrentDownloadButtonText.color = Color.white;
    //        buttonEvents.showToast("oops, something went wrong :) Check your internet connection", 3);
    //        return;
    //    }
    //    CurrentuiDownloadButton.interactable = false;
    //    CurrentDownloadButtonText.color = Color.gray;
    //    Debug.Log("assetItem: " + assetItem.name);
    //    InstatiateItemInYourList(assetItem, true);
    //    buttonEvents.ItemClicked(true);
    //    buttonEvents.showToast("Your artwork has been added to your pieces", 3);
    //}

    //private void PrepareToRefreshItems(Button button)
    //{
    //    button.onClick.AddListener(() => { InstantiateItemsInAllArtList(); });
    //}

    //void DestroyAllButtons()
    //{
    //    foreach (Transform child in ButtonParentAll)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //}

    //private void MakeModelDownloadable(string name)
    //{
    //    for (int i = 0; i < ButtonParentAll.transform.childCount; ++i)
    //    {
    //        Transform currentItem = ButtonParentAll.transform.GetChild(i);           
    //        Transform textDownloadTransform = currentItem.transform.GetChild(1);
    //        TMP_Text txt = textDownloadTransform.GetComponent<TMP_Text>();
    //        if (txt.text == name)
    //        {
    //            txt.color = Color.white;
    //            Transform buttonDownloadTransform = currentItem.transform.GetChild(0);
    //            Button uiDownloadButton = buttonDownloadTransform.GetComponent<Button>();
    //            uiDownloadButton.interactable = true;
    //        }                        
    //    }            
    //}   



    //private void SaveList(List<AssetItem> items)
    //{
    //    string tempPath = FileService.FileListPath;
    //    FileStream fs = new FileStream(tempPath, FileMode.Create);
    //    BinaryFormatter bf = new BinaryFormatter();
    //    bf.Serialize(fs, items);
    //    fs.Close();
    //}
}
