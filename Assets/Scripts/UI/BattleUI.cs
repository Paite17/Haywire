using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleMenuState
{
    MAIN,
    FIGHT,
    FIGHTSUB,
    ITEM,
    ITEMSUB,
    DETAIL
}

// I was never really good at coding UIs, huh
public class BattleUI : MonoBehaviour
{
    // menu pointers
    // main
    private int main_selectIndex;
    private int main_lastSelection;

    // fight
    private int fight_selectIndex;
    private int fight_lastSelection;

    // item
    private int item_selectIndex;
    private int item_lastSelection;

    [SerializeField] private BattleMenuState menuState;

    // unit references for player and current enemy
    // enemy unit should be grabbed seperately since it'd be chosen at random
    [SerializeField] private Unit playerUnit;
    private Unit currentEnemyUnit;

    // these lists will be for changing appearance of selected text
    [SerializeField] private List<Text> mainPromptText;

    [SerializeField] private Text healthText;

    // ui boxes
    [SerializeField] private GameObject battlePrompt;
    [SerializeField] private GameObject fightPrompt;
    [SerializeField] private GameObject fightSubPrompt;
    [SerializeField] private GameObject itemPrompt;
    [SerializeField] private GameObject detailPrompt;

    // setting of ui elements in submenus
    // Attack submenu
    [SerializeField] private List<Text> attackNames;
    [SerializeField] private Text sub_AttackName;
    [SerializeField] private Text sub_attackDescription;
    [SerializeField] private Image sub_AttackCombo;

    // Items submenu (TODO: add more for items)
    [SerializeField] private List<Text> itemNames;

    // Details submenu
    [SerializeField] private Text detail_enemyNameLabel;
    [SerializeField] private Text detail_enemyDescriptionLabel;
    [SerializeField] private Text detail_enemyLevelLabel;
    [SerializeField] private Text detail_enemyAttackLabel;
    [SerializeField] private Text detail_enemyDefenceLabel;
    [SerializeField] private Text detail_enemySpeedLabel;
    [SerializeField] private Text detail_enemyLuckLabel;
    [SerializeField] private Text detail_enemyEXPLabel;

    [SerializeField] private BattleSystem battleSystem;


    // TODO: set strings like the ones in DETAIL and health strings to their respective values!
    // TODO: Grab details of playerattack scriptableobject in UI!

    private void Start()
    {
        // these conflict initially
        main_lastSelection = 1;
        fight_lastSelection = 1;
        item_lastSelection = 1;


        UpdateText();
        
        // call a coroutine because if the data is being called on start with no delay there *could* be conflict
        StartCoroutine(LoadOnDelay());
    }

    private void ProcessInputs()
    {
        // only function if its the player's turn
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            // keyboard inputs for UI on main state
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                FindObjectOfType<AudioManager>().Play("sUp");
                switch (menuState)
                {
                    case BattleMenuState.MAIN:
                        // hopefully grab the previous value of selectIndex before it changes
                        main_lastSelection = main_selectIndex;
                        main_selectIndex++;
                        break;
                    case BattleMenuState.FIGHT:
                        fight_lastSelection = fight_selectIndex;
                        fight_selectIndex++;
                        break;
                    case BattleMenuState.ITEM:
                        item_lastSelection = item_selectIndex;
                        item_selectIndex++;
                        break;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                FindObjectOfType<AudioManager>().Play("sUp");
                switch (menuState)
                {
                    case BattleMenuState.MAIN:
                        main_lastSelection = main_selectIndex;
                        main_selectIndex--;
                        break;
                    case BattleMenuState.FIGHT:
                        fight_lastSelection = fight_selectIndex;
                        fight_selectIndex--;
                        break;
                    case BattleMenuState.ITEM:
                        item_lastSelection = item_selectIndex;
                        item_selectIndex--;
                        break;
                }
            }

            // for menus that have a different layout
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                FindObjectOfType<AudioManager>().Play("sUp");
                switch (menuState)
                {
                    case BattleMenuState.FIGHT:
                        fight_lastSelection = fight_selectIndex;
                        fight_selectIndex -= 2;
                        break;
                    case BattleMenuState.ITEM:
                        item_lastSelection = item_selectIndex;
                        item_selectIndex -= 2;
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                FindObjectOfType<AudioManager>().Play("sUp");
                switch (menuState)
                {
                    case BattleMenuState.FIGHT:
                        fight_lastSelection = fight_selectIndex;
                        fight_selectIndex += 2;
                        break;
                    case BattleMenuState.ITEM:
                        item_lastSelection = item_selectIndex;
                        item_selectIndex += 2;
                        break;
                }
            }

            // confirm selection
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ConfirmSelection();
            }

            // cancell selection
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelSelection();
            }

            // correction: your mum
            if (main_selectIndex > 3)
            {
                main_selectIndex = 0;
            }

            if (main_selectIndex < 0)
            {
                main_selectIndex = 3;
            }

            if (fight_selectIndex > 3)
            {
                fight_selectIndex = 0;
            }

            if (fight_selectIndex < 0)
            {
                fight_selectIndex = 3;
            }

            // this one should be changed in the future to be however big the player's inventory is
            if (fight_selectIndex > 3)
            {
                fight_selectIndex = 0;
            }

            if (item_selectIndex < 0)
            {
                item_selectIndex = 3;
            }
        }

        // set health label
        healthText.text = "Player Health: " + battleSystem.playerUnit.currentHP + "/" + battleSystem.playerUnit.maxHP + "\n \n Enemy Health: " + battleSystem.enemyUnit.currentHP + "/" + battleSystem.enemyUnit.maxHP;

    }


    // scott told me that using Update() and FixedUpdate() together was a bad idea
    // he's prolly right but i'm gonna do it again, nobody can stop me :DDDDD
    // nvm got rid of Update() after getting absolutely blasted yesterday by scott and brett
    // nvm again swapped FixedUpdate() for Update() cus UI felt unresponsive :D
    private void Update()
    {
        // update UI
        UpdateText();

        // process user inputs
        ProcessInputs();
    }

    private void ConfirmSelection()
    {
        // should also only work when player's turn (though this feels like a lazy way of doing it
        if (battleSystem.state == BattleState.PLAYERTURN)
        {
            // hope this works (fix heal UI being broken
            if (battleSystem.state != BattleState.PLAYERATTACK)
            {
                FindObjectOfType<AudioManager>().Play("confirm");
                // check which state is active then do the things
                switch (menuState)
                {
                    case BattleMenuState.MAIN:
                        switch (main_selectIndex)
                        {
                            case 0:
                                menuState = BattleMenuState.FIGHT;
                                fightPrompt.SetActive(true);
                                break;
                            case 1:
                                menuState = BattleMenuState.ITEM;
                                itemPrompt.SetActive(true);
                                break;
                            case 2:
                                // call run method
                                battleSystem.StartRunning();
                                break;
                            case 3:
                                menuState = BattleMenuState.DETAIL;
                                detailPrompt.SetActive(true);
                                break;
                        }
                        break;
                    case BattleMenuState.FIGHT:
                        // select attack (maybe also call a method that initialises the sub-sub prompt with the correct info
                        LoadAttackSubMenuData(fight_selectIndex);
                        fightSubPrompt.SetActive(true);
                        menuState = BattleMenuState.FIGHTSUB;
                        break;
                    case BattleMenuState.ITEM:
                        // item junk
                        battleSystem.CallHealPlayer();
                        HideMenu();
                        break;
                    case BattleMenuState.DETAIL:
                        // return to MAIN
                        menuState = BattleMenuState.MAIN;
                        detailPrompt.SetActive(false);
                        break;
                    case BattleMenuState.FIGHTSUB:
                        // initiate attack
                        battleSystem.StartPlayerAttack(fight_selectIndex);
                        HideMenu();
                        break;
                }
            }
        }
            
    }

    private void CancelSelection()
    {
        FindObjectOfType<AudioManager>().Play("cancel");
        // go back a menu unless on MAIN
        switch (menuState)
        {
            case BattleMenuState.FIGHT:
                menuState = BattleMenuState.MAIN;
                fightPrompt.SetActive(false);
                break;
            case BattleMenuState.FIGHTSUB:
                menuState = BattleMenuState.FIGHT;
                fightSubPrompt.SetActive(false);
                break;
            case BattleMenuState.ITEM:
                menuState = BattleMenuState.MAIN;
                itemPrompt.SetActive(false);
                break;
            case BattleMenuState.DETAIL:
                // kinda pointless but eh, consistency 
                menuState = BattleMenuState.MAIN;
                detailPrompt.SetActive(false);
                break;
        }
        
    }

    // updates the selected + deselected text in the UI
    
    private void UpdateText()
    {
        switch (menuState)
        {
            case BattleMenuState.MAIN:
                // text in the main box
                mainPromptText[main_selectIndex].fontSize = 65;
                mainPromptText[main_lastSelection].fontSize = 45;
                break;
            case BattleMenuState.ITEM:
                // TODO: item text
                break;
            case BattleMenuState.FIGHT:
                attackNames[fight_selectIndex].fontSize = 40;
                attackNames[fight_lastSelection].fontSize = 35;
                break;
        }
    }

    // call when attack names are needed
    private void LoadAttackData()
    {
        Debug.Log("Loading attack names...");
        // load scriptableobject data from unit

        foreach (var move in battleSystem.playerUnit.listOfUsablePlayerMoves)
        {
            // get position of the active move in the list
            int currentIndex = battleSystem.playerUnit.listOfUsablePlayerMoves.IndexOf(move);

            Debug.Log("index = " + currentIndex);
            Debug.Log("found " + move.MoveName);
            attackNames[currentIndex].text = move.MoveName;
            
        }
    }

    // the voices are getting louder
    private void LoadDetailData()
    {
        Debug.Log("Loading attack details...");
        // load detail text from scriptableObject
        detail_enemyNameLabel.text = battleSystem.enemyUnit.unitName;
        detail_enemyDescriptionLabel.text = battleSystem.enemyUnit.unitDescription;
        detail_enemyLevelLabel.text = "Level: " + battleSystem.enemyUnit.level;
        detail_enemyAttackLabel.text = "Damage: " + battleSystem.enemyUnit.damage;
        detail_enemyDefenceLabel.text = "Defence: " + battleSystem.enemyUnit.defence;
        detail_enemySpeedLabel.text = "Speed: " + battleSystem.enemyUnit.agility;
        detail_enemyLuckLabel.text = "Luck: " + battleSystem.enemyUnit.luck;
        detail_enemyEXPLabel.text = "EXP: " + battleSystem.enemyUnit.currentXP;
    }

    IEnumerator LoadOnDelay()
    {
        Debug.Log("LoadOnDelay() called!");

        yield return new WaitForSeconds(0.5f);

        LoadAttackData();
        LoadDetailData();
        // any more that get added should be called here
    }

    // load attack details for submenu
    // attackPointer should refer to which attack the player selected previously
    private void LoadAttackSubMenuData(int attackPointer)
    {
        Debug.Log("Loading details for fight submenu");
        sub_AttackName.text = battleSystem.playerUnit.listOfUsablePlayerMoves[attackPointer].MoveName;
        sub_attackDescription.text = battleSystem.playerUnit.listOfUsablePlayerMoves[attackPointer].Description;
        sub_AttackCombo.sprite = battleSystem.playerUnit.listOfUsablePlayerMoves[attackPointer].ComboPreview;
    }

    // when an attack starts/an item is used, we should make the menu reset its state and pointers
    public void ResetMenu()
    {
        menuState = BattleMenuState.MAIN;
        main_lastSelection = main_selectIndex;
        fight_lastSelection = fight_selectIndex;
        item_lastSelection = item_selectIndex;
        main_selectIndex = 0;
        fight_selectIndex = 0;
        item_selectIndex = 0;
        ShowMenu();
    }

    // hide menu when player attacks
    public void HideMenu()
    {
        fightSubPrompt.SetActive(false);
        fightPrompt.SetActive(false);
        itemPrompt.SetActive(false);
        battlePrompt.SetActive(false);
    }

    // bring it back please
    public void ShowMenu()
    {
        battlePrompt.SetActive(true);
    }
}
