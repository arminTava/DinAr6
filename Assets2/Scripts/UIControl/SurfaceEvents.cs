using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SurfaceEvents : MonoBehaviour
{
    public Button YourArtButton;
    public Button AllArtButton;
    public GameObject ArtPanel;
    public GameObject AllArtPanel;
    //public GameObject DownloadInfo;
    public Settings settings;
    // Start is called before the first frame update
    void OnEnable()
    {
        YourArtButton.gameObject.SetActive(true);
        AllArtButton.gameObject.SetActive(true);
        settings.OnToggleTogglePointCloudValueChanged(true);
        var toggles = settings.Options.transform.GetComponentsInChildren<Toggle>();
        foreach (var t in toggles) { t.interactable = true; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        if (YourArtButton!= null) YourArtButton.gameObject.SetActive(false);
        if (AllArtButton != null) AllArtButton.gameObject.SetActive(false);
    }

}
