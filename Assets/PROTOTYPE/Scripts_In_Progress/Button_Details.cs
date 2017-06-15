using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Details : MonoBehaviour {

    public string currentWord, scrambledWord;
    public bool scramble, isTranslating;
    static bool weAreTranslating, clickedWasRight, addedACharacter, removedACharacter;
    public static string currentScrambleWord, tempTranslation, currentCharacter, cheatWord;
    public static GameObject currentButton, oldButton;
    public static int currentWordCounter, oldWordCounter, currentLetter, tempCounter, totalScrambledWords;
    public GameObject panel;
    Reveal_Cheat cheatComponent;

    
	// Use this for initialization
	void Start () {
        cheatComponent = FindObjectOfType<Reveal_Cheat>();
        cheatWord = "";
        currentScrambleWord = "";
        currentCharacter = "";
        currentLetter = 0;
        for (int i = 0; i < Translation.keyWords.Length; i++)
        {
            if (currentWord.Trim().Equals(Translation.keyWords[i])) //&& !Translation.translated[i])
            {
                //Set the word in the button's text object as the scrambled word.
                for (int j = 0; j < currentWord.Length; j++)
                {
                    scrambledWord += Translation.scrambledLetters[Random.Range(0, Translation.scrambledLetters.Length)];
                }
                totalScrambledWords += 1;
                //Make sure to set the scrambled word to the text object within the Button.
                //break out of the for loop since we got a hit with the translation thing
                scramble = true;
                GetComponentInChildren<Text>().text = scrambledWord;
                GetComponentInChildren<Text>().color = Color.red;
                cheatComponent.scrambledList(currentWord);
                break;
            }
        }
       // Debug.Log("The total number of scrambled words in this puzzle is: " + totalScrambledWords);
    }
	
	// Update is called once per frame
	void Update () {
        //Go through the key words and check if the current button word is the same as that word
        /*for (int i = 0; i < Translation.keyWords.Length; i++)
        {
            if (currentWord.Trim().Equals(Translation.keyWords[i]))
            {
                //Set the word in the button's text object as the scrambled word.
                for(int j = 0; j < currentWord.Length; j++)
                {
                    scrambledWord += Translation.scrambledLetters[Random.Range(0, Translation.scrambledLetters.Length)];
                }
                //Make sure to set the scrambled word to the text object within the Button.
                //break out of the for loop since we got a hit with the translation thing
                scramble = true;
                GetComponentInChildren<Text>().text = scrambledWord;
                break;
            }
        }*/
        if(scramble && currentWord.Trim().Equals(cheatWord.Trim()))
        {
            scramble = false;
            GetComponentInChildren<Text>().text = currentWord;
            GetComponentInChildren<Text>().color = Color.green;
            //Translation.wasTranslated(currentWord);

        }
        if (clickedWasRight && scramble)
        {
            Debug.Log(oldWordCounter) ;
            if(oldWordCounter == 0)
            {
                clickedWasRight = false;
                oldButton = null;
            }
            
            if (currentWord.Trim().Equals(oldButton.GetComponent<Button_Details>().currentWord.Trim()))
            {
                Debug.Log("does the other word ever change?");
                
                scramble = false;
                GetComponentInChildren<Text>().text = currentWord;
                GetComponentInChildren<Text>().color = Color.green;
                //Translation.wasTranslated(currentWord);
                oldWordCounter -= 1;

            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && currentWord.Trim().Equals(currentScrambleWord.Trim()))
        {
            currentLetter = 0;
            isTranslating = false;
            weAreTranslating = false;
            if (tempTranslation.Trim().Equals(currentWord.Trim()))
            {
                scramble = false;
                GetComponentInChildren<Text>().text = currentWord;
                GetComponentInChildren<Text>().color = Color.green;
                //Translation.wasTranslated(currentWord);
            }
            else
            {
                GetComponentInChildren<Text>().text = scrambledWord;
            }
            //currentScrambleWord = "";
            StartCoroutine(waitToResetCurrentScrambledWord());
        }
        if(scramble && currentWord.Trim().Equals(currentScrambleWord.Trim()) && addedACharacter)
        {
            //Debug.Log("Is this getting accessed?");
            //GetComponentInChildren<Text>().text = tempTranslation;
            Debug.Log(currentWordCounter);
            GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text.Remove(currentLetter, 1);
            GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text.Insert(currentLetter, currentCharacter);
            GetComponentInChildren<Text>().color = Color.blue;
            scrambledWord = GetComponentInChildren<Text>().text;
            tempCounter += 1;
            if(tempCounter == currentWordCounter)
            {
                addedACharacter = false;
                currentLetter += 1;
                tempCounter = 0;
            }
            
        }
        else if (scramble && currentWord.Trim().Equals(currentScrambleWord.Trim()) && removedACharacter)
        {
            //currentLetter -= 1;
            GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text.Remove(currentLetter-1, 1);
            GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text.Insert(currentLetter-1, Translation.scrambledLetters[Random.Range(0, Translation.scrambledLetters.Length)]);
            scrambledWord = GetComponentInChildren<Text>().text;
            
            tempCounter += 1;
            if (tempCounter == currentWordCounter)
            {
                removedACharacter = false;
                currentLetter -= 1;
                tempCounter = 0;
            }

            //currentLetter -= 1;
        }else if(scramble && currentWord.Trim().Equals(currentScrambleWord.Trim()))
        {
            GetComponentInChildren<Text>().color = Color.blue;
        }
        else if (scramble)
        {
            GetComponentInChildren<Text>().text = scrambledWord;
            GetComponentInChildren<Text>().color = Color.red;
        } else
        {
            GetComponentInChildren<Text>().text = currentWord;
        }
        if (isTranslating)
        {
            currentScrambleWord = currentButton.GetComponent<Button_Details>().currentWord;
            InputWord();
        }
	}
    public void editWord()
    {
        currentLetter = 0;
        if (weAreTranslating)
        {
            currentButton.GetComponent<Button_Details>().isTranslating = false;
            weAreTranslating = false;
            
            if (tempTranslation.Trim().Equals(currentScrambleWord.Trim()))
            {
                currentButton.GetComponent<Button_Details>().scramble = false;
                currentButton.GetComponentInChildren<Text>().text = currentWord;
                currentButton.GetComponentInChildren<Text>().color = Color.green;
                //Translation.wasTranslated(currentWord);
                oldButton = currentButton;
                oldWordCounter = currentWordCounter;
                clickedWasRight = true;
            }
            else
            {
                currentButton.GetComponentInChildren<Text>().text = scrambledWord;
            }
            currentScrambleWord = "";
        }
        if (scramble)
        {
            currentWordCounter = panel.GetComponent<Translation>().instanceOfWord(currentWord);
            tempCounter = 0;
            currentButton = this.gameObject;
            isTranslating = true;
            weAreTranslating = true;
            tempTranslation = "";
            Debug.Log("Does the temp get reset");
        }
    }


    public IEnumerator waitToResetCurrentScrambledWord()
    {
        yield return new WaitForEndOfFrame();
        currentScrambleWord = "";
    }

























    public void InputWord()
    {
        if(tempTranslation.Length < currentWord.Length)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //then we add that letter to the user definition string
                tempTranslation += 'a';
                currentCharacter = "a";
                addedACharacter = true; 
                //we remove the definition offered in the list

                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                tempTranslation += 'b';
                currentCharacter = "b";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                tempTranslation += 'c';
                currentCharacter = "c";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                tempTranslation += 'd';
                currentCharacter = "d";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                tempTranslation += 'e';
                currentCharacter = "e";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                tempTranslation += 'f';
                currentCharacter = "f";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                tempTranslation += 'g';
                currentCharacter = "g";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                tempTranslation += 'h';
                currentCharacter = "h";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                tempTranslation += 'i';
                currentCharacter = "i";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                tempTranslation += 'j';
                currentCharacter = "j";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                tempTranslation += 'k';
                currentCharacter = "k";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                tempTranslation += 'l';
                currentCharacter = "l";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                tempTranslation += 'm';
                currentCharacter = "m";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                tempTranslation += 'n';
                currentCharacter = "n";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                tempTranslation += 'o';
                currentCharacter = "o";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                tempTranslation += 'p';
                currentCharacter = "p";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                tempTranslation += 'q';
                currentCharacter = "q";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                tempTranslation += 'r';
                currentCharacter = "r";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                tempTranslation += 's';
                currentCharacter = "s";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                tempTranslation += 't';
                currentCharacter = "t";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                tempTranslation += 'u';
                currentCharacter = "u";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                tempTranslation += 'v';
                currentCharacter = "v";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                tempTranslation += 'w';
                currentCharacter = "w";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                tempTranslation += 'x';
                currentCharacter = "x";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                tempTranslation += 'y';
                currentCharacter = "y";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                tempTranslation += 'z';
                currentCharacter = "z";
                addedACharacter = true;
                //definitionOffered.RemoveAt(currentPage);
                //and add the one with the extra letter in its place
                //definitionOffered.Insert(currentPage, tempTranslation);

            }

        }
        if (Input.GetKeyDown(KeyCode.Backspace) && tempTranslation.Length > 0)
        {
            //then we remove the last character from the string
            tempTranslation = tempTranslation.Substring(0, tempTranslation.Length - 1);
            removedACharacter = true;
            //we remove the definition offered at that index 

            //definitionOffered.RemoveAt(currentPage);
            //and add the one with the extra letter in its place
            //definitionOffered.Insert(currentPage, tempTranslation);
            //moveTyperBackward();
        }

    }


}
