using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    public float beatTempo;

    public bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        beatTempo /= 60;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!hasStarted)
        {
            /*
            // placeholder for starting
            // TODO: start upon player move activating
            if (Input.anyKeyDown)
            {
                hasStarted = true;
            } */

        }
        else
        {
            // move notes down based on tempo
            // could potentially make an upscroll setting
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        } 
    }


}
