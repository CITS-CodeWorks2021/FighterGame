using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public Transform player;
    public float attackDistance, attackChance, blockChance;

    public float attackDelay, blockDelay;
    public bool attackCooling, blockCooling;

    PlayerControl controller;

    public bool playerAttacking;

    bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerControl>();
        player.GetComponent<PlayerControl>().OnAttack.AddListener(HandleOnAttack);
        GameLogic.OnStart.AddListener(GameStart);
        GameLogic.OnEnd.AddListener(GameEnd);
    }

    void GameStart()
    {
        isPlaying = true;
    }

    void GameEnd(int whoLost)
    {
        isPlaying = false;
    }

    private void HandleOnAttack(bool plAttacking)
    {
        playerAttacking = plAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if(distance > attackDistance)
        {
            // move closer
            controller.Move(true);
        }

        if(distance < attackDistance * 0.7f)
        {
            // move further
            controller.Move(false);
        }

        if(distance > attackDistance *0.7f && distance < attackDistance)
        {
            float ranNum = Random.Range(0f, 1f);
            if(playerAttacking)
            {
                if(ranNum <= blockChance && !blockCooling)
                {
                    controller.anims.SetBool("isBlocking", true);
                    blockCooling = true;
                    StartCoroutine(BlockDelay());
                }
            }
            else
            {
                if(ranNum <= attackChance && !attackCooling)
                {
                    ranNum = Random.Range(0.0f, 1.0f);
                    if(ranNum >= 0.5f)
                        controller.anims.SetTrigger("OnPunchOne");
                    else controller.anims.SetTrigger("OnPunchTwo");

                    attackCooling = true;
                    StartCoroutine( AttackDelay() );
                }
            }
        }
    }

    IEnumerator BlockDelay()
    {
        yield return new WaitForSeconds(blockDelay);
        blockCooling = false;
        controller.anims.SetBool("isBlocking", false);
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        attackCooling = false;
    }
}