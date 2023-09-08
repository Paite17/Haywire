using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CANNED FEATURE IGNORE THIS
public class GridMovement : MonoBehaviour
{
    
    [SerializeField] private List<Transform> gridPos;
    [SerializeField] private BattleSystem battleSystem;
    private float horizontal;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This is quite possibly the worst code i've ever made but i'm short on time and have no clue how to do it right
        // i feel ashamed, everyone will be so disappointed in me if they saw this

        // Set centre horizontal position
        /*
        if (battleSystem.state == BattleState.ENEMYTURN)
        {
            if (horizontal == 0f)
            {
                // check vertical
                if (vertical == 0f)
                {
                    // centre position
                    GetComponent<Transform>().position = gridPos[0].position;
                }

                if (vertical < 0f)
                {
                    // lower position
                    GetComponent<Transform>().position = gridPos[5].position;
                }
                else if (vertical > 0f)
                {
                    // upper position
                    GetComponent<Transform>().position = gridPos[4].position;
                }
            }
            else if (horizontal > 0f)
            {
                if (vertical == 0)
                {
                    // rightmost position
                    GetComponent<Transform>().position = gridPos[8].position;
                }

                if (vertical > 0f)
                {
                    // upper right position
                    GetComponent<Transform>().position = gridPos[6].position;
                }
                else if (vertical < 0f)
                {
                    // lower right position
                    GetComponent<Transform>().position = gridPos[7].position;
                }
            }
            else if (horizontal < 0f)
            {
                if (vertical == 0)
                {
                    // leftmost position
                    GetComponent<Transform>().position = gridPos[3].position;
                }

                if (vertical > 0f)
                {
                    // upper left position
                    GetComponent<Transform>().position = gridPos[2].position;
                }
                else if (vertical < 0f)
                {
                    // lower left position
                    GetComponent<Transform>().position = gridPos[1].position;
                }
            }
        } */
    }

    private void FixedUpdate()
    {
        // get horizontal and vertical positions
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
}
