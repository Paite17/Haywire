using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // my game isn't really gonna actually use music, this is just to help the system work
    // since it'll likely be based on the length of the track (which wouldn't have any actual audio just like white noise or something)
    public AudioSource music;

    public bool startPlaying;

    public BeatScroller beatScroller;

    public static GameManager instance;

    public int currentScore;

    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;
    public float accuracy;

    public BattleSystem battleSystem;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            // TODO: Change this to be when a move is chosen
            /*if (Input.anyKeyDown)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;

                music.Play(); 
            } */
        }
        else
        {
            // check if song has ended
            // may need to only occur in playerturn state
            if (!music.isPlaying)
            {
                // Get results and deal damage

                // end playing
                beatScroller.hasStarted = false;

                // reset note holder position
                beatScroller.gameObject.transform.position = new Vector3(0, 0, 0);

                // get accuracy
                float totalHits = normalHits + goodHits + perfectHits;
                accuracy = (totalHits / totalNotes) * 100;
                // this shouldn't be necessary but i once got an accuracy of 433
                if (accuracy > 100)
                {
                    accuracy = 100;
                }

                Debug.Log("Accuracy = " + accuracy);
                // pefect hit
                if (accuracy > 99)
                {
                    GameObject obj = GameObject.Find("Player");
                    Unit player = obj.GetComponent<Unit>();

                    if (player.miss)
                    {
                        // don't play audio
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("perfect");
                    }
                    
                }

                startPlaying = false;

                // reset
                missedHits = 0;
                goodHits = 0;
                perfectHits = 0;
                normalHits = 0;
                totalNotes = 0;
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Hit");
        FindObjectOfType<AudioManager>().Play("left");
        //currentScore += scorePerNote;
    }

    public void NoteMissed()
    {
        Debug.Log("Missed");
        missedHits++;
    }

    public void NormalHit()
    {
        currentScore += scorePerNote;
        normalHits++;
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote;
        goodHits++;
        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote;
        perfectHits++;
        NoteHit();
    }

    // set bool to start rhythm system - called from other scripts
    public void StartAttack()
    {
        startPlaying = true;
        beatScroller.hasStarted = true;

        music.Play();
    }

    public void GetCurrentAmountOfNotes()
    {
        // discover how many notes there are to hit
        totalNotes = FindObjectsOfType<Note>().Length;
    }
}
