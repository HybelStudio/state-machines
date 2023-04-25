using UnityEngine;

public class OptionsMenuState : MenuState
{
    private GameObject _optionsMenu;

    public OptionsMenuState(MenuController menuController, GameObject optionsMenu) : base(menuController) => _optionsMenu = optionsMenu;

    public override void EnterState()
    {
        Debug.Log("Entered Options");
        _optionsMenu.SetActive(true);
    }

    public override void ExitState()
    {
        Debug.Log("Exited Options");
        _optionsMenu.SetActive(false);
    }
}
