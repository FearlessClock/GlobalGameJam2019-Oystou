using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleController : MonoBehaviour
{
    public float destroyTime = 2;
    public void SetBubble(MemorableItem item)
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = item.worldItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(DestroyBubble());
    }

    IEnumerator DestroyBubble()
    {
        yield return new WaitForSeconds(destroyTime);
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
    }

    public void Destroy()
    {
        PlayerController.instance.hasSpeechBubble = false;
        Destroy(transform.parent.gameObject, 0.1f);
    }
}
