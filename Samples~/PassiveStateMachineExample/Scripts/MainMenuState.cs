using UnityEngine;

public class MainMenuState : MenuState
{
    private GameObject _mainMenu;

    public MainMenuState(MenuController menuController, GameObject mainMenu) : base(menuController) => _mainMenu = mainMenu;

    public override void EnterState()
    {
        Debug.Log("Entered Main Menu");
        _mainMenu.SetActive(true);
    }

    public override void ExitState()
    {
        Debug.Log("Exited Main Menu");
        _mainMenu.SetActive(false);
    }
}
