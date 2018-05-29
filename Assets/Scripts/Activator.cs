using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    public KeyCode keycode;
    public Color initialColor;
   
    Renderer rend;




    // public PlayerScore playerscore;
    //private GameObject note;
    //private bool isCollisionActive = false;
      
    void Start () {
        rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", initialColor);
        //playerscore.ShowScore(currentScore);
        //playerscore = GetComponent<PlayerScore>();
    }
	
	// Update is called once per frame
	void Update () {

        ChangeColor();
       		
	}

    public bool IsKeyCodeHit() {
        if (Input.GetKeyDown(keycode)) {
            return true;
        }

        return false;
    }


    void ChangeColor() {

        if (Input.GetKeyDown(keycode)) {
            StartCoroutine(ChangeColorForSeconds());
        }
    }
    
    IEnumerator ChangeColorForSeconds() {
        rend.material.SetColor("_Color", new Color(0.1f, 0.3f, 0.4f, 0.6f));
        yield return new WaitForSeconds(0.1f);
        rend.material.SetColor("_Color", initialColor);



    }

    /*
     void OnTriggerEnter(Collider col)
    {
        
        isCollisionActive = true;
        if (col.gameObject.tag == "Note") {
            note = col.gameObject;
        }
       
    }


     void OnTriggerEnterExit(Collider col)
    {
        isCollisionActive = false;
    }
    */

  
}
