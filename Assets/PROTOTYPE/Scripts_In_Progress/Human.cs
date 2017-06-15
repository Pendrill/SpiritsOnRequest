using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

    public int startingThoughts, hiddenThoughts, currentThought;
    public string[] startingThoughtsNames, hiddenThoughtsNames;
    public GameObject[] thoughtPanels;
    public string Name, thought;

	// Use this for initialization
	void Start () {
        currentThought = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //THIS STUFF IS GOING TO NEED TO GO IN THE MANAGER, HOWEVER WE NEED TO INFORM THE MANAGER WHAT THE THOUGHTS ARE.
    //Or I could send it to the button??? I don't think that will work...
    public void moveThoughtRight()
    {
        currentThought += 1;
        if (currentThought > startingThoughtsNames.Length - 1)
        {
            currentThought = 0;
            MANAGER_Translator.currentThought = startingThoughtsNames[currentThought];
        }
        else
        {
            
            MANAGER_Translator.currentThought = startingThoughtsNames[currentThought];
        }
        
    }
    public void moveThoughtLeft()
    {
        currentThought -= 1;
        if(currentThought < 0)
        {
            currentThought = startingThoughtsNames.Length - 1;
            MANAGER_Translator.currentThought = startingThoughtsNames[currentThought];
        }
        else
        {
            MANAGER_Translator.currentThought = startingThoughtsNames[currentThought];
        }
    } 
}
