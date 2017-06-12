using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animations : MonoBehaviour {

    //public Animation player_Animation;
    public Animator player_Animator;
	// Use this for initialization
	void Start () {
        player_Animator = GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!player_Animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Walk"))
        {
            //player_Animator.Play("Player_Walk");
        }else
        {
           // Debug.Log("is it set to false?");
           // player_Animator.SetBool("Walk 0", false);
        }
	}
}
