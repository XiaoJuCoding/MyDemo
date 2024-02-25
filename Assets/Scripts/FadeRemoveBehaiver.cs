using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeRemoveBehaiver : StateMachineBehaviour
{
    public float fadeTime= 3f;
    public float fadeDelay = 0f;
    private float fadeDelayElapsed = 0f;
    private float timeElapsed = 0f;
    SpriteRenderer spriteRenderer;
    GameObject gameObjectRemove;
    Color starcolor;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
        starcolor = spriteRenderer.color;
        gameObjectRemove = animator.gameObject;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(fadeDelay > fadeDelayElapsed)
        {
            fadeDelayElapsed += Time.deltaTime;
        }
        else
        {
            timeElapsed += Time.deltaTime;

            float newAlpha = starcolor.a * (1 - (timeElapsed / fadeTime));

            spriteRenderer.color = new Color(starcolor.r, starcolor.g, starcolor.b, newAlpha);

            if (timeElapsed > fadeTime)
            {
                Destroy(gameObjectRemove);
            }
        }


    }
}
