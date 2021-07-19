using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour
{
    public string targetTag;

    public bool hasHit;

    PlayerControl controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerControl>();
        controller.hitters.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == targetTag && !hasHit)
        {
            Debug.Log("Hit Something: " + collision.gameObject.tag);
            collision.gameObject.GetComponentInParent<PlayerControl>().TakeHit(controller);
            hasHit = true;
        }
    }
}
