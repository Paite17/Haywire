using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum BattleState
{
    START,
    PLAYERTURN,
    PLAYERATTACK,
    PLAYERHEAL,
    ENEMYTURN,
    WON,
    LOST,
    EXP
}


public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    // characters
    public GameObject playerPrefab;
    public List<GameObject> enemyPrefabs;

    // enemy position to spawn
    public Transform enemySpritePosition;

    // unit references - making them public whats the worst that could happen?
    public Unit playerUnit;
    public Unit enemyUnit;
    private int random;

    public GameManager rhythmManager;

    public GameObject theGrid;

    public Transform noteHolder;

    public GameObject arrowHolder;

    public GameObject enemyAttackingNotif;

    public BattleUI UISystem;

    // pointers that'll be sent over from the UI script
    // specifically to see which attack the player chooses specifically
    private int attackIndex;
    private int itemIndex;


    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        FindObjectOfType<AudioManager>().Play("battle");

        // get player unit data
        playerUnit = playerPrefab.GetComponent<Unit>();
        Unit.LoadPlayer(playerUnit);

        //enemy moment (will probably only have 2 enemies atm)
        random = Random.Range(0, enemyPrefabs.Count);
        GameObject enemyObj = new GameObject();
        enemyUnit = enemyObj.GetComponent<Unit>();
        enemyObj = Instantiate(enemyPrefabs[random], enemySpritePosition);
        enemyUnit = enemyObj.GetComponent<Unit>();
        Debug.Log("Loaded enemy prefab " + random);

        // set base stats
        playerUnit.damage = playerUnit.baseDamage;
        playerUnit.defence = playerUnit.baseDefence;
        playerUnit.agility = playerUnit.baseAgility;
       

        enemyUnit.damage = enemyUnit.baseDamage;
        enemyUnit.defence = enemyUnit.baseDefence;
        enemyUnit.agility = enemyUnit.baseAgility;

        // set moves (when implemented)

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    private void PlayerTurn()
    {
        // check if effects need to revert
    }

    // add current move as an argument when added
    public void StartPlayerAttack(int index)
    {
        attackIndex = index;
        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        state = BattleState.PLAYERATTACK;
        bool isDead = false;
        // This is for loading notes - it acts as a pointer for which note has which position
        int index = 0;
        // load notes from selected move (dear god please work thx)
        foreach (GameObject note in playerUnit.listOfUsablePlayerMoves[attackIndex].NoteChart)
        {
            
            Debug.Log("Index of note in NoteChart list = " + index);

            Debug.Log("Loading chart for: " + playerUnit.listOfUsablePlayerMoves[attackIndex].MoveName);

            // set position for current note to be spawned
            Vector3 currentPos = new Vector3(playerUnit.listOfUsablePlayerMoves[attackIndex].PosChart[index].x, playerUnit.listOfUsablePlayerMoves[attackIndex].PosChart[index].y, note.transform.position.z);

            // spawn note
            GameObject currentNote = Instantiate(note, currentPos, note.transform.rotation, noteHolder);

            index++;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.GetCurrentAmountOfNotes();
        GameManager.instance.StartAttack();

        // I should probably move the code for the rhythm segment into its own thing so it can run before the rest of the coroutine runs but oh well
        yield return new WaitForSeconds(5f);


        // run attack 
        isDead = enemyUnit.TakeDamage(playerUnit.damage, enemyUnit.defence, playerUnit.agility, playerUnit.luck, enemyUnit.agility, rhythmManager.accuracy);

        yield return new WaitForSeconds(0.5f);

        // check if dead
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        bool isDead = false;

        // decide the move to use
        int attackDecide = Random.Range(0, 3);

        yield return new WaitForSeconds(0.5f);

        // atkStrength need to have a better value for atkStrength since enemies aren't using actual moves anymore...
        isDead = playerUnit.TakeDamage(enemyUnit.damage, playerUnit.defence, enemyUnit.agility, enemyUnit.luck, playerUnit.agility, rhythmManager.accuracy);

        enemyAttackingNotif.SetActive(true);
        FindObjectOfType<AudioManager>().Play("enemy_hit");

        yield return new WaitForSeconds(0.5f);

        // check if dead
        if (isDead)
        {
            enemyAttackingNotif.SetActive(false);
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            // gotta reset the accuracy and hit notes methinks
            enemyAttackingNotif.SetActive(false);
            state = BattleState.PLAYERTURN;
            UISystem.ResetMenu();
            PlayerTurn();
        }
    }

    private void EndBattle()
    {
        if (state == BattleState.LOST)
        {
            // gameover
            Debug.Log("game over :(");
            SceneManager.LoadScene("GameOver");
        }
        else if (state == BattleState.WON)
        {
            // winning
            Debug.Log("player won :)");
            SceneManager.LoadScene("WinScene");
        }
    }
    
    // initiate running coroutine
    public void StartRunning()
    {
        StartCoroutine(RunAttempt());
    }

    IEnumerator RunAttempt()
    {
        int runChance = Random.Range(1, 10);
        Debug.Log("Run attempt");

        yield return new WaitForSeconds(1f);

        if (runChance == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("Run failed");
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void CallHealPlayer()
    {
        StartCoroutine(HealPlayer());
    }

    IEnumerator HealPlayer()
    {
        // i mean its technically an attack ¯\_(?)_/¯
        FindObjectOfType<AudioManager>().Play("heal");
        state = BattleState.PLAYERATTACK;
        playerUnit.Heal(playerUnit.baseDamage * 3);

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    // Update is called once per frame
    void Update()
    {
        // make the grid invisible until needed
        // going unused cus grid is getting the axe
        /*
        if (state != BattleState.ENEMYTURN)
        {
            theGrid.SetActive(false);
        }
        else
        {
            theGrid.SetActive(true);
        } */

        // make arrows appear/disappear

        if (state != BattleState.PLAYERATTACK)
        {
            arrowHolder.SetActive(false);
        }
        else
        {
            arrowHolder.SetActive(true);
        }
    }
}
