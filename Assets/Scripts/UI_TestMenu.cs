using UnityEngine;

public class UI_TestMenu : UI_BaseMenu
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DisableMenu();
        }
    }

    protected override void OnMenuEnabled()
    {

    }

    protected override void OnMenuDisabled()
    {
        Debug.Log("AAA");
        foreach (UI_TweenAnimator child in childAnimators)
        {
            child.OnMenuDisable();
            Debug.Log("AA");
        }
    }

    protected override void OnLoadingExit() { }

    protected override void OnMenuInitialEnabled()
    {

    }
}
