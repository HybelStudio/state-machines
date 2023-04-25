public abstract class MenuState
{
    protected MenuController _menuController;

    public MenuState(MenuController menuController)
    {
        _menuController = menuController;
    }

    public abstract void EnterState();
    public abstract void ExitState();
}
