using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeSoundBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;
    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelaySoundPlay = false;

    //OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(soundToPlay,GameObject.Find("Virtual Camera").transform.position
                /*animator.gameObject.transform.position*/,volume);
        }
        timeSinceEntered = 0f;
        hasDelaySoundPlay = false;
    }

    //OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDelaySoundPlay)
        {
            timeSinceEntered += Time.deltaTime;
            if(timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, GameObject.Find("Virtual Camera").transform.position
                    /* animator.gameObject.transform.position*/, volume);
                hasDelaySoundPlay = true;
            }
        }

    }

    //OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }
    }

    
}
