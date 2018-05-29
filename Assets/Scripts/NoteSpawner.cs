using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour {

    public GameObject note;

    /* User Input */
    public Input[] keycodes;
    

    /* Auido */
    public AudioClip[] clips;
    public AudioSource source;
    public int clipBPM = 59;

    /* Lerp the note */
    public Transform[] startPos;
    public Transform[] endPos;
    private float totalDistanceToDestination;
    private float noteTimer;

    private int postionIndex;
    private int clipSelect = 0;
    private int samplePerBeat;
    private int beatTimer;
    private float lerpPercentage;
    

    private List<float[]> audioBuffers = new List<float[]>();
    private int audioIndex = 0;

    private List<int> spawnCount = new List<int>();
    private List<GameObject> spawnedNodes = new List<GameObject>();
    private List<int> spawnStartEndTargetIndex = new List<int>();
    private List<float> timeSinceStarted = new List<float>();
    private List<float> timeDuringLerp = new List<float>();
    private List<int> keyIndex = new List<int>();
    private List<int> beatToHit = new List<int>();
    private List<int> beatToHitIndex = new List<int>();
    private int hitIndex;


    private UserInput userInput;

    //private float timer = 0;
    //private float[] timeStamp = new float[] { 0.3f, 0.9f, 1.5f, 2.2f, 3f, 3.5f, 6f };
    //private int[] activatorIdx = new int[] { 0, 2, 1, 4, 6, 3, 3 };


    // Use this for initialization
    void Start () {
        userInput = GetComponent<UserInput>();

        samplePerBeat = 60 / clipBPM * AudioSettings.outputSampleRate; //Get how many sample per beat
        //Debug.Log(samplePerBeat);

        // Add beatnterval //
        beatToHit.Add(5);
        beatToHit.Add(1);
        beatToHit.Add(1);
        beatToHit.Add(1);
        beatToHit.Add(5);
        beatToHit.Add(3);
        beatToHit.Add(4);
        beatToHit.Add(3);
        beatToHit.Add(4);

        /* AudioFilter Method
        float[] emptySecond = new float[48000]; // Creat a empty 1 second                       
        for (int i = 0; i < clips.Length; i++) {
            float[] newBuffer = new float[clips[i].samples]; // Define the length of the newBuffer[]
            clips[i].GetData(newBuffer,0); // Use clip data to fill the audioBuffer array. 0 => noOffset
            audioBuffers.Add(newBuffer); // add the float[] newBuffer to the audioBuffers list
            audioBuffers.Add(emptySecond); // add the empty second after every clip
        }
        
        source.Play(); // Play the AudioClip;

        */


    }


    int passPosition = 0;
    int deltaSum = 0;
    int deltaSumNoteHit = 0;
    float deletNoteTimer = 0;
    
    private void Update()
    {
        //postionIndex = Random.Range(0, startPos.Length); //random pick the note spawn position
        //hitIndex = Random.Range(0, beatToHit.Count); //random pick the beatInterval
      

        BeatOnAudioSource();
       
    }
  
    void BeatOnAudioSource()   // Samples from the Playing AudioSource 
    {
       
       
        if (source.isPlaying)
        {
            // print(source.timeSamples);
            int deltaPosition = source.timeSamples - passPosition;  // delta Postion on the source sample. smallest slices       
            deltaSum += deltaPosition; // accumulate the delta posision slices

            if (deltaSum > samplePerBeat) // to check the deltaSum > the sampleBeat   >> 1 beat
            {
                print("1beat");
                SpawnTheNote();
                deltaSum = 0; // reset the accmulation
               
            }
          
             // To loop all the element in the spawnedNodes List, Postiion list, Time List based on the spawnedNote List size
             for (int i = spawnedNodes.Count - 1; i >= 0; i--)
             {


                Transform startPosTarget = startPos[spawnStartEndTargetIndex[i]]; // Lerp startPos = startPos[index[i]]
                Transform endPosTarget = endPos[spawnStartEndTargetIndex[i]]; // Lerp endPos = endPos[index[i]]
                lerpPercentage = timeSinceStarted[i] / (float)(beatToHit[beatToHitIndex[i]] * samplePerBeat);
                
                //start to Lerp      
                spawnedNodes[i].GetComponent<Transform>().position = Vector3.Lerp(startPosTarget.position, endPosTarget.position, lerpPercentage);
                // accumulate the deltaPosition slices. Once the (timeSinceStarted[i] += deltaPosition) = samplePerBeat, Lerping is over.
                timeSinceStarted[i] += deltaPosition;


                
                KeyCode keyToHit = userInput.keycodes[keyIndex[i]];

                if (lerpPercentage <= 1.6 && lerpPercentage >= 0.8 && Input.GetKeyDown(keyToHit)) //destory the note when lerping is over 
                {
                    Destroy(spawnedNodes[i]); // Delet the GameObject first
                    spawnedNodes.RemoveAt(i); // Remove the GameObject in the list
                    spawnStartEndTargetIndex.RemoveAt(i); // Remove the positionIndext
                    timeSinceStarted.RemoveAt(i); // Remove the timer
                    keyIndex.RemoveAt(i); // Remove the keyindex from the list
                    beatToHitIndex.RemoveAt(i);

                } else if ((lerpPercentage > 1.6)) { // Timeout
                    Destroy(spawnedNodes[i]); // Delet the GameObject first
                    spawnedNodes.RemoveAt(i); // Remove the GameObject in the list
                    spawnStartEndTargetIndex.RemoveAt(i); // Remove the positionIndext
                    timeSinceStarted.RemoveAt(i); // Remove the timer
                    keyIndex.RemoveAt(i); // Remove the keyindex from the list
                    beatToHitIndex.RemoveAt(i);

                }


            }
                      
        }

        
        passPosition = source.timeSamples; // make the passPosition 1 frame later then the sourceSamples
        
    }

   
    // the function that spawns new notes
    void SpawnTheNote()
    {
        GameObject newNote = Instantiate(note, startPos[1].position, Quaternion.identity) as GameObject;  //spawn note 
        newNote.transform.parent = transform;
        spawnedNodes.Add(newNote);  // add the GameObject newNote to the spawnNode list
        timeSinceStarted.Add(0); // set the delta timer to 
        spawnCount.Add(1); // calculate how many time the node is spawned
        if (spawnCount.Count > 2 * startPos.Length) {  // clear the spawnCount list as every 2*startPos[].Length
            spawnCount.Clear();
        }

        /* assing the beat interval to the spawned note in order */
        if (spawnCount.Count % (beatToHit.Count) != 0)
        {
            hitIndex = (spawnCount.Count) % (beatToHit.Count) - 1;
            beatToHitIndex.Add(hitIndex);
        }
        else
        {
            hitIndex = beatToHit.Count - 1;
            beatToHitIndex.Add(hitIndex);
        }

        /* assing the startPos to the spawned note in order */
        if (spawnCount.Count % (startPos.Length) != 0)
        {
            postionIndex = (spawnCount.Count) % (startPos.Length) - 1;
            spawnStartEndTargetIndex.Add(postionIndex);
        }
        else
        {
            postionIndex = startPos.Length - 1;
            spawnStartEndTargetIndex.Add(postionIndex);
        }
        keyIndex.Add(postionIndex); // add the same index as the position to the list

    }











    /*

    void TimeStamp() {

        timer += Time.deltaTime;
        if (postion < timeStamp.Length && timer > timeStamp[postion])
        {
            SpawnAt();
            postion++;
        }
    }
    
    */


    /*
   void SpawnAt() {
           Transform spawnBase = activators[postion];
           float offset = 5.0f;            
           GameObject newNote = Instantiate(note, spawnBase.position + new Vector3(0, offset, 0), Quaternion.identity) as GameObject;
           newNote.transform.parent = transform;
   }
   */


    /*
    void SpawnNoteRandomly()
    {
        float offset = 5.0f;
        int i = Random.Range(0, activators.Length);
        Transform spawnBase = activators[i];
        GameObject newNote = Instantiate(note, spawnBase.position + new Vector3(0, offset, 0), Quaternion.identity) as GameObject;
        newNote.transform.parent = transform;
    }
    */

    /*
   
    private void OnAudioFilterRead(float[] data, int channels)
    {


        float[] currentClip = audioBuffers[clipSelect]; 

        for (int i = 0; i < data.Length; i += 2) {  // Pick L
         
            data[i] = currentClip[audioIndex];
            data[i+1] = currentClip[audioIndex]; //Pick R
            audioIndex++;


            
            
            if (audioIndex % samplePerBeat == 0) {
                  Debug.Log("1 beat");               
            }
      

            
            if (audioIndex >= currentClip.Length) {
                audioIndex = 0;
                
                clipSelect++;
                if (clipSelect == audioBuffers.Count)
                {
                    clipSelect = 0;
                }
         

            }

        }
        

    }
    */



    /*
        spawnInterval = Random.Range(0.5f, 3f);

        timer += Time.deltaTime;
        if(timer > spawnInterval)
        {
            timer = 0;
            SpawnNoteRandomly();
        }
    */

}
