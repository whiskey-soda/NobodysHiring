using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActivityMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Animator animator;
    [Space]
    [SerializeField] Button prevPageButton;
    [SerializeField] Button nextPageButton;
    [SerializeField] List<GameObject> pages = new List<GameObject>();
    uint currentPage = 0;

    private void Start()
    {
        UpdateNavButtonInteractability();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // activate statbars on hover
        Statbars.Instance.energy.ShowPreview();
        Statbars.Instance.motivation.ShowPreview();

        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Opening");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // deactivate statbars on unhover
        Statbars.Instance.energy.HidePreview();
        Statbars.Instance.motivation.HidePreview();

        if (animator == null) { return; } // do nothing if animator is null
        animator.Play("Closing");
    }

    /// <summary>
    /// disable previous page button on first page, and disables next page button on last page.
    /// ensures buttons are enabled except in those two circumstances
    /// </summary>
    void UpdateNavButtonInteractability()
    {
        if (currentPage == 0) { prevPageButton.interactable = false; }
        else { prevPageButton.interactable = true;}

        if (currentPage == pages.Count - 1) { nextPageButton.interactable = false; }
        else { nextPageButton.interactable = true;}
    }

    /// <summary>
    /// sets the current page object as active, and sets all others as inactive.
    /// also sets nav button interactability after changing page.
    /// </summary>
    /// <param name="pageNum"></param>
    void ShowPage(uint pageNum)
    {
        foreach (GameObject page in pages)
        {
            // only set current page active. set all others inactive
            if (pages.IndexOf(page) == pageNum) { page.SetActive(true); }
            else { page.SetActive(false); }
        }

        UpdateNavButtonInteractability();
    }

    /// <summary>
    /// sets the current page to a specified page, and shows that page.
    /// </summary>
    /// <param name="pageNum"></param>
    void SetPage(uint pageNum)
    {
        // cap pagenum to # of pages
        if (pageNum > pages.Count - 1) { pageNum = (uint)(pages.Count - 1); }

        currentPage = pageNum;
        ShowPage(currentPage);
    }

    public void PreviousPage()
    {
        // do nothing if on first page
        if (currentPage == 0) { return; }

        SetPage(currentPage - 1);
    }

    public void NextPage()
    {
        // do nothing if on last page
        if (currentPage >= pages.Count - 1) { return; }

        SetPage(currentPage + 1);
    }
}
