using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    
    public SpeechBubble bubbleObject;

    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = bubbleObject.objectPhoto;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
