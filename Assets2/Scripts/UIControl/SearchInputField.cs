using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchInputField : MonoBehaviour
{
    public TMP_InputField InputField;
    public Button CleanSearchButton;
    public GameObject ButtonParentAll;
    public Button SearchButton;
    public GameObject SearchPanel;

    void Start()
    {
        CleanSearchButton.onClick.AddListener(OnCleanSearch);
        InputField.onValueChanged.AddListener(delegate { SearchInputChanged(); });
        SearchButton.onClick.AddListener(delegate { OnShowSearch(true); });

    }
    public void OnShowSearch(bool active)
    {
        Debug.Log("SEARCH INPUT");
        if (SearchPanel.activeSelf)
        {
            SearchPanel.SetActive(false);
            OnCleanSearch();
        }
        else SearchPanel.SetActive(true);
    }

    public void OnCleanSearch()
    {
        InputField.text = "";
        SearchInputChanged();
    }

    private void SearchInputChanged()
    {
        StartCoroutine(SearchChildren());
    }
    IEnumerator SearchChildren()
    {
        string currentText = InputField.text.ToLower();
        int children = ButtonParentAll.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            var currentChild = ButtonParentAll.transform.GetChild(i);
            string currentChildText = currentChild.transform.GetChild(1).GetComponent<TMP_Text>().text.ToLower();
            if (!currentChildText.Contains(currentText))
            {
                currentChild.transform.gameObject.SetActive(false);
            }
            else currentChild.transform.gameObject.SetActive(true);
            yield return null;

        }
    }
}
