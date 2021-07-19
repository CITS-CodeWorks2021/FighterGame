using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isAI, isLeft;
    public Transform otherPlayer;
    public string horizAxis;
    public float moveSpeed;

    Animator anims;
    float curSpeed;

    // Start is called before the first frame update
    void Start()
    {
        anims = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAI)
        {
            // Setting speed based on current input
            curSpeed = Input.GetAxis(horizAxis) * moveSpeed;

            // Setting animation direction based on input and if left
            // If not left, inverse
            if (isLeft) anims.SetFloat("speed", Input.GetAxis(horizAxis));
            else anims.SetFloat("speed", -Input.GetAxis(horizAxis));

            transform.Translate(curSpeed, 0, 0);
        }
    }

    public void Move(bool forward)
    {
        curSpeed = forward ? -moveSpeed : moveSpeed;

        // Setting animation direction based on input and if left
        // If not left, inverse
        if (isLeft) anims.SetFloat("speed", curSpeed);
        else anims.SetFloat("speed", -curSpeed);

        transform.Translate(curSpeed, 0, 0);
    }
}
