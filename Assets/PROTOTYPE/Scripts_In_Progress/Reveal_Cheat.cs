using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal_Cheat : MonoBehaviour {
    
    
    bool firstFrameGone;
    public List<string> scrambledWords = new List<string>();
    //public string[] scrambledWords;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Space) && scrambledWords.Count != 0)
        {
            string word = scrambledWords[Random.Range(0, scrambledWords.Count)];
            //Debug.Log(word);
            Button_Details.cheatWord = word;
            scrambledWords.Remove(word.Trim());
        }
	}
   
    public void scrambledList(string word)
    {
        bool alreadyThere = false;
        for( int i = 0; i < scrambledWords.Count ; i++)
        {
            if (word.Trim().Equals(scrambledWords[i].Trim()))
            {
                alreadyThere = true;
                break;
            }
        }
        if (!alreadyThere)
        {
            scrambledWords.Add(word);
        }
    }
}
