using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestoryer : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }
}
