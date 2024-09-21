using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public abstract class UI_BaseMenu : MonoBehaviour
{
    public int layerOrder;

    public bool startDisabled;

    public bool disableOnExit;

    [HideInInspector] public List<UI_TweenAnimator> childAnimators = new();

    private static List<UI_BaseMenu> activeMenus = new();

    protected abstract void OnMenuInitialEnabled();

    protected abstract void OnMenuEnabled();

    protected abstract void OnMenuDisabled();

    protected abstract void OnLoadingExit();

    private void InitialEnableMenu()
    {
        activeMenus.Add(this);
        gameObject.SetActive(true);
        OnMenuInitialEnabled();
    }

    private void AssignAnimators()
    {
        childAnimators.AddRange(GetComponentsInChildren<UI_TweenAnimator>());
        foreach (UI_BaseMenu childMenu in GetComponentsInChildren<UI_BaseMenu>())
        {
            if (childMenu == this) continue;
            foreach (UI_TweenAnimator unownedChild in GetComponentsInChildren<UI_TweenAnimator>())
                // Ah yes, parental neglect
                childAnimators.Remove(unownedChild);
        }
    }

    private void InitialiseMenu()
    {
        gameObject.SetActive(true);
        AssignAnimators();

        if (startDisabled) gameObject.SetActive(false);
        else InitialEnableMenu();

        //SYST_SceneNavigation.OnFinishedLoading -= InitialiseMenu;
    }

    public void EnableMenu() 
    {
        activeMenus.Add(this);
        gameObject.SetActive(true);
        OnMenuEnabled();
    }

    public void DisableMenu() 
    {
        activeMenus.Remove(this);
        OnMenuDisabled();
        if (disableOnExit) gameObject.SetActive(false);
    }

    public UI_BaseMenu()
    {
        //SYST_SceneNavigation.OnFinishedLoading += InitialiseMenu;
        //SYST_SceneNavigation.OnLoadingScreenExit += OnLoadingExit;
    }
}
