using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectionShare : MonoBehaviour
{
    public Button ChoosedKunstwerkButton;
    private TMP_Text ChoosedKunstwerk { get; set; }
    private TMP_Text ChoosedKunstwerkURL { get; set; }
    public GameObject SwitchingInfo;
    public bool isSurfaceFirstLoaded { get; set; }
    public bool isMarkerFirstLoaded { get; set; }
    public bool isGPSFirstLoaded { get; set; }

    void Start()
    {
        
    }

    public void SetURLName(AssetItem assetItem)
    {
        ChoosedKunstwerk = ChoosedKunstwerkButton.transform.GetChild(0).GetComponent<TMP_Text>();
        ChoosedKunstwerkURL = ChoosedKunstwerkButton.transform.GetChild(1).GetComponent<TMP_Text>();
        ChoosedKunstwerk.text = assetItem.name;
        ChoosedKunstwerkURL.text = assetItem.urlpath;
    }

    public void OnNavigateArtToWebsite()
    {
        if (ChoosedKunstwerk != null)
        {
            if (ChoosedKunstwerkURL.text != "null") Application.OpenURL(ChoosedKunstwerkURL.text);
            else Application.OpenURL("https://www.artakten.de/");
        }
        else Application.OpenURL("https://www.artakten.de/");
    }

    public Texture2D CreateProfileImg(string str)
    {
        try
        {
            byte[] imageBytes = Convert.FromBase64String(str);
            var tex = new Texture2D(2, 2);

            //Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            return tex;
        }
        catch
        {
            return null;
        }
        //tex.Resize((int)profileImg.texture.width, (int)profileImg.texture.height);
    }

    public void DestroyAllButtons(Transform Parent, List<AssetItem> SpawnedPrefabs = null, int Startindex = 1)
    {
        int childCount = Parent.childCount;
        for (int i = Startindex; i < childCount; i++)
        {
            var child = Parent.GetChild(i);
            if (SpawnedPrefabs != null)
            {
                var currentName = child.GetChild(1).GetComponent<TMP_Text>().text;
                var indexExist = SpawnedPrefabs.FindIndex(x => x.name == currentName);
                if (indexExist < 0) Destroy(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }

        }
        //foreach (Transform child in ButtonParentAll)
        //{
        //    Destroy(child.gameObject);
        //}
    }

    public string[] SplitStringSaved(string file, string sep)
    {
        string[] stringSeparator = new string[] { "/" }; //{ "\\ /" }; // HIER ÄNDERN FÜR APP AUF HANDY
        string[] fileList1 = file.Split(stringSeparator, StringSplitOptions.None);
        string[] stringSeparator2 = new string[] { sep };
        string[] stringSeparator3 = new string[] { FileService.SplitSeperator };
        var Count = fileList1.Length;
        var fileRealList = fileList1[Count - 1].Split(stringSeparator2, StringSplitOptions.None);
        return fileRealList;
    }
}
