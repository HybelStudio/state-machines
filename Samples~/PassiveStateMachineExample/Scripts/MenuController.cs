using System.Collections.Generic;
using Hybel.StateMachines;
using UnityEngine;

// Here we derive from the PassiveStateObject class which maintains a state machine and provides wrapper functions for interactive with it.
public class MenuController : PassiveStateObject<MenuState, MenuTrigger>
{
    public MenuController(Dictionary<MenuTrigger, GameObject> menuObjectDictionary)
    {
        // First, create the states which will be used in the state machine.
        var mainMenu = new MainMenuState(this, menuObjectDictionary[MenuTrigger.MainMenu]);
        var optionsMenu = new OptionsMenuState(this, menuObjectDictionary[MenuTrigger.Options]);

        // Then configure the state machine.
        Configure()
            // Whenever we are in the state 'mainMenu'...
            .At(mainMenu)
                // Allow the state machine to transition to 'optionsMenu' if the
                // MenuTrigger.Options trigger is passed in the Fire function on the state machine.
                .Permit(optionsMenu, MenuTrigger.Options)
            // Whenever we are in the state 'optionsMenu' (which we can reach from the main menu dure to line above)...
            .At(optionsMenu)
                // Allow the state machine transition to 'mainMenu' if the
                // MenuTrigger.MainMenu trigger is passed in the Fire function on the state machine.
                .Permit(mainMenu, MenuTrigger.MainMenu);

        // Set the initial state explicitly. This must be done, otherwise the state machine can never have a state.
        SetInitialState(mainMenu);
    }

    // In the case that this object gets garbage collected, make sure to exit the current state first.
    ~MenuController() => CurrentState.ExitState();

    // The OnEnterState method is called whenever the state machine enters a state. This is called when setting the initial state.
    protected override void OnEnterState(MenuState menuState) => menuState.EnterState();

    // The OnExitState method is called whenever the state machine exits a state, usually due to entering a different state.
    protected override void OnExitState(MenuState menuState) => menuState.ExitState();
}
