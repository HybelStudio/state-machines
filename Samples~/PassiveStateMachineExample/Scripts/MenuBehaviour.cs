using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] private List<Entry> triggersAndPanels;

    private MenuController menuController;

    private void Awake()
    {
        Dictionary<MenuTrigger, GameObject> menuObjectDictionary = new();

        foreach (var (trigger, panel) in triggersAndPanels)
            menuObjectDictionary[trigger] = panel;

        // Setup the menu controller
        menuController = new MenuController(menuObjectDictionary);
    }

    // This is called from the Back button in the Options panel.
    public void EnterMainMenu() => menuController.Fire(MenuTrigger.MainMenu);

    // This is called from the Options button in the Main Menu panel.
    public void EnterOptions() => menuController.Fire(MenuTrigger.Options);

    [Serializable]
    private class Entry
    {
        public MenuTrigger Trigger;
        public GameObject Panel;

        public void Deconstruct(out MenuTrigger trigger, out GameObject panel)
        {
            trigger = Trigger;
            panel = Panel;
        }
    }
}
