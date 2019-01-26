using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    public void SetBubble(SpeechBubble item)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.objectPhoto;
        StartCoroutine(DestroyBubble());
    }

    IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
    }

    public void Destroy()
    {
        PlayerController.instance.hasSpeechBubble = false;
        Destroy(transform.parent.gameObject, 0.1f);
    }
}
