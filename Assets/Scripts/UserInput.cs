using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour {

    public KeyCode[] keycodes;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        IsKeyHit();
		
	}



    public bool IsKeyHit() {
        for (int i = 0; i < keycodes.Length; i++)
        {
            if (Input.GetKeyDown(keycodes[i]))
            {
                return true;
            }

        }

        return false;
    }
    
}
