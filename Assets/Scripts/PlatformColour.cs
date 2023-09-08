using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// BRETT WILL NEVER KNOW I WAS STILL HELPING HAHHAHAHAHAHHAHAHAH
public class PlatformColour : MonoBehaviour
{
    // stores the rgb values
    public Vector3[] availableColours;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PickColour()
    {
        // pick random number
        int randomPick = Random.Range(0, 14);

        // change objects colour
        GetComponent<SpriteRenderer>().color = new Color(availableColours[randomPick].x, availableColours[randomPick].y, availableColours[randomPick].z);
    }
}
