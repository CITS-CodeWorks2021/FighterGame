using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    public int startingHealth, playerNumber;
    public HealthGauge myGauge;
    public string attackOne, attackTwo, Block;
    public bool isAI, isLeft;
    public string horizAxis;
    public float moveSpeed, immuneTimer, wallDist, minDist;
    public Transform otherPlayer;

    int damageToInflict = 5, curHealth;
    float curSpeed;

    public bool isAttacking, isBlocking, isImmune = false, isPlaying = false;

    public Animator anims;

    public List<Hitter> hitters = new List<Hitter>();

    public UnityEvent<bool> OnAttack = new UnityEvent<bool>();

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        anims = GetComponent<Animator>();
        GameLogic.OnStart.AddListener(StartGame);
        GameLogic.OnEnd.AddListener(EndGame);

        startingPos = transform.position;
    }

    public void StartGame()
    {
        myGauge.SetStartHealth(startingHealth);
        curHealth = startingHealth;
        isPlaying = true;
        anims.SetBool("isPlaying", true);
        isAttacking = false;
        transform.position = startingPos;
    }

    public void EndGame(int whoLost)
    {
        if (curHealth > 0) anims.SetTrigger("OnWin");
        else anims.SetTrigger("OnLost");
        isPlaying = false;
        anims.SetBool("isPlaying", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAI) return;
        if (!isPlaying) return;
        if (Input.GetAxis(attackOne) > 0 && !isAttacking)
        {
            anims.SetTrigger("OnPunchOne");
            isAttacking = true;
            OnAttack.Invoke(true);
        }
        if (Input.GetAxis(attackTwo) > 0 && !isAttacking)
        {
            anims.SetTrigger("OnPunchTwo");
            isAttacking = true;
            OnAttack.Invoke(true);
        }

        if (Input.GetAxis(Block) > 0)
        {
            anims.SetBool("isBlocking", true);
            isBlocking = true;
        }
        else
        {
            anims.SetBool("isBlocking", false);
            isBlocking = false;
        }

        // Double checking we are STILL not moving/attacking
        if (!isBlocking && !isAttacking)
        {
            // Setting speed based on current input
            curSpeed = Input.GetAxis(horizAxis) * moveSpeed * Time.deltaTime;

            bool shouldMove = CheckCanMove(curSpeed);
            if(shouldMove)
            {
                anims.SetFloat("speed", Input.GetAxis(horizAxis));
                transform.Translate(curSpeed, 0, 0);
            }
        }
        
    }

    public void SetDamage(int amount)
    {
        damageToInflict = amount;
    }

    public void TakeHit(PlayerControl fromWhom)
    {
        Debug.Log("Taken Hit from: " + fromWhom);
        if (isImmune) return;
        int dReceived = isBlocking ? 1 :fromWhom.damageToInflict;

        if (!isBlocking)
        {
            isImmune = true;
            StartCoroutine(TimedImmunity());
        }

        curHealth -= dReceived;

        // Need to do damage. . . but to where?
        Debug.Log("Took Damage: " + dReceived.ToString());
        anims.SetTrigger("OnHit");
        myGauge.UpdateGauge(curHealth);
        if (curHealth <= 0) GameLogic.OnEnd.Invoke(playerNumber);
    }

    IEnumerator TimedImmunity()
    {
        yield return new WaitForSeconds(immuneTimer);
        isImmune = false;
    }

    public void AttackEnd()
    {

        isAttacking = false;
        if (!isAI) OnAttack.Invoke(false);
        foreach (Hitter h in hitters)
        {
            h.hasHit = false;
        }
    }

    public void Move(bool forward)
    {
        curSpeed = forward ? -moveSpeed : moveSpeed;
        bool shouldMove = CheckCanMove(curSpeed);
        if (shouldMove)
        {
            anims.SetFloat("speed", -curSpeed);

            transform.Translate(curSpeed * Time.deltaTime, 0, 0);
        }
    }

    bool CheckCanMove(float movement)
    {
        bool canMove = true;
        if( (isLeft && movement > 0) || (!isLeft && movement < 0) )
        {
            // check if too close to opponent to move
            if(Vector3.Distance(transform.position, otherPlayer.position) < minDist)
            {
                canMove = false;
            }
        }
        else
        {
            // check if too close to wall to move
            if( Mathf.Abs(transform.position.x) > (wallDist - 1) )
            {
                canMove = false;
            }
        }

        return canMove;
    }

}
