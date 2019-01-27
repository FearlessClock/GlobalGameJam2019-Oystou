using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoyerHouseSpriteSwap : MonoBehaviour
{
    public Sprite placedSprite;
    public Sprite pickedUpSprite;
    ParticleSystem particles;
    public SpriteRenderer spriteRenderer;
    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        PlayerController.OnItemCarried += OnItemCarried;
        PlayerController.OnItemDropped += OnItemDropped;
    }

    private void OnDestroy()
    {
        PlayerController.OnItemCarried -= OnItemCarried;
        PlayerController.OnItemDropped -= OnItemDropped;
    }

    private void OnItemDropped(GameObject item)
    {
        if (item.CompareTag(this.tag))
        {
            spriteRenderer.sprite = placedSprite;
            particles.Stop();
            particles.Play();
        }
    }

    private void OnItemCarried(GameObject item)
    {
        if (item.CompareTag(this.tag))
        {
            spriteRenderer.sprite = pickedUpSprite;
            particles.Stop();
            particles.Play();
        }
    }
}
