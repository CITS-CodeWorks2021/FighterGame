using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltControl : MonoBehaviour
{
    public Text curAction, curButton;
    public Combo[] combos;

    Combo currentCombo;
    int nextChainIndex;


    bool canChain = false;
    bool canStart = true;
    bool hasHitNextInChain = false;

    void Start()
    {
        curAction.text = "Idle";
        curButton.text = "";
    }

    void Update()
    {
        // If being hit is true. . . return;
        if (canStart)
        {
            for (int i = 0; i < combos.Length; i++)
            {
                if (Input.GetKeyDown(combos[i].attacks[0].key))
                {
                    curAction.text = combos[i].attacks[0].animTrigger;
                    curButton.text = combos[i].attacks[0].key.ToString();
                    // Set Animator Trigger Here

                    nextChainIndex = 1;
                    currentCombo = combos[i];
                    StartCoroutine(AttackChain());
                    return;
                }
            }
        }

        if (canChain)
        {

            if (currentCombo.attacks.Length > nextChainIndex)
            {
                Debug.Log("Looking for Chain Input!");
                if (!hasHitNextInChain) hasHitNextInChain = Input.GetKeyDown(currentCombo.attacks[nextChainIndex].key);
                else curButton.text = currentCombo.attacks[nextChainIndex].key.ToString();

            }
            else hasHitNextInChain = false;
        }
    }

    IEnumerator AttackChain()
    {
        canChain = false;
        canStart = false;
        yield return
            new WaitForSeconds(currentCombo.attacks[nextChainIndex - 1].preChainTime);

        canChain = true;
        canStart = false;
        yield return
            new WaitForSeconds(currentCombo.attacks[nextChainIndex - 1].chainTime);

        if (hasHitNextInChain)
        {
            curAction.text = currentCombo.attacks[nextChainIndex].animTrigger;
            // Set Animator Trigger Here
            nextChainIndex++;
            hasHitNextInChain = false;
            StartCoroutine(AttackChain());
        }
        else
        {
            curAction.text = "Idle";
            // All attack anims should have an "Exit Time" transition back to Idle
            canChain = false;
            canStart = true;
            nextChainIndex = 0;

            // Reset Animator Triggers here
        }

    }

    // Some means of determining if character has been hit
    // Likely a public OnHit function the opponent calls
    //public void OnHit()
    //{
    // sets an ifHit to true
    // then starts the HitCountdown Coroutine
    //}

    // IEnumerator HitCountDown()
    //{
    // yield return new WaitForSeconds( duration of hit react )
    // sets the ifHit to false
    //}
}

[System.Serializable]
public class Combo
{
    public Attack[] attacks;
}

[System.Serializable]
public class Attack
{
    public KeyCode key;
    public string animTrigger;
    public float preChainTime, chainTime;
}
