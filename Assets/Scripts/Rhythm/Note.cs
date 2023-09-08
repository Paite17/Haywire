using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool canBePressed;

    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // constant allignment because some notes don't know how to allign properly
        // fix allignment (odd method)
        switch (keyToPress)
        {
            case KeyCode.LeftArrow:
                GameObject objL = GameObject.Find("Left");
                transform.position = new Vector3(objL.transform.position.x, transform.position.y, objL.transform.position.z);
                break;
            case KeyCode.RightArrow:
                GameObject objR = GameObject.Find("Right");
                transform.position = new Vector3(objR.transform.position.x, transform.position.y, objR.transform.position.z);
                break;
            case KeyCode.UpArrow:
                GameObject objU = GameObject.Find("Up");
                transform.position = new Vector3(objU.transform.position.x, transform.position.y, objU.transform.position.z);
                break;
            case KeyCode.DownArrow:
                GameObject objD = GameObject.Find("Down");
                transform.position = new Vector3(objD.transform.position.x, transform.position.y, objD.transform.position.z);
                break;
        }

        if (Input.GetKeyDown(keyToPress))
        {

            if (canBePressed)
            {
                Destroy(gameObject);

                //GameManager.instance.NoteHit();

                // might need to change the value its being compared to if i change the position of the UI
                // defining whats a 'good', 'normal' or 'perfect' hit
                if (Mathf.Abs(transform.position.y) > 0.25f)
                {
                    Debug.Log("Normal hit");
                    GameManager.instance.NormalHit();
                }
                else if (Mathf.Abs(transform.position.y) > 0.05f)
                {
                    Debug.Log("Good hit");
                    GameManager.instance.GoodHit();
                }
                else
                {
                    Debug.Log("Perfect hit");
                    GameManager.instance.PerfectHit();
                }
            }
        }
    }

    // colliding with player key object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = true;
        }

        // kill note
        if (collision.tag == "kill")
        {
            Debug.Log("kill");
            GameManager.instance.NoteMissed();
            Destroy(gameObject);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            canBePressed = false;

            // TODO: fix hits being recognised as misses as well
            //GameManager.instance.NoteMissed();
        }
    }
}
