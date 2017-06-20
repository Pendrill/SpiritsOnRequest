using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translation : MonoBehaviour {
    public GameObject theTestButton, buttonPrefab, tempButton, otherPanel_1, otherPanel_2, otherPanel_3;
    public Transform canvas;
    public GameObject[] testButtons;
    public string[] buttonWords;
    public static string[] keyWords, scrambledLetters;
    public static bool[] translated;
    public TextAsset buttonWordTextFile, keyWordsTextFile, scrambledLettersTextFiles;
    public GameObject panel;
    float panelWidth, panelHeight, panelStartPosX, panelStartPosY, maxWidth, currentWidth, typeSpeed;
    public Vector2 WordOffset;
    bool once, isTyping, coroutineIsHappening, reShowWordsOnce, movedHappened;
    public bool fourPanel;
    public string thought;

	// Use this for initialization
	void Start () {
        panelWidth = panel.GetComponent<RectTransform>().rect.width;
        panelHeight = panel.GetComponent<RectTransform>().rect.height;
        panelStartPosY = panel.GetComponent<RectTransform>().anchoredPosition3D.y + (panelHeight/2);
        panelStartPosX = panel.GetComponent<RectTransform>().anchoredPosition3D.x - (panelWidth/2);
        maxWidth = panelWidth - 150;
        currentWidth = WordOffset.x;

        if (buttonWordTextFile != null)
        {
            buttonWords = (buttonWordTextFile.text.Split(' '));
            testButtons = new GameObject[buttonWords.Length];
        }
        //CURRENTLY THESE WILL ALL GET RESET WHEN ENTERING A NEW SCENE
        if(keyWordsTextFile != null)
        {
            keyWords = keyWordsTextFile.text.Split(' ');
            
        }
        translated = new bool[keyWords.Length];
        for( int i = 0; i < keyWords.Length; i++)
        {
            translated[i] = false;
        }
        if(scrambledLettersTextFiles != null)
        {
            scrambledLetters = scrambledLettersTextFiles.text.Split(' ');
        }
        for (int i = 0; i < buttonWords.Length; i++)
        {
            tempButton = Instantiate(buttonPrefab,new Vector3 (10000,0,0), Quaternion.identity, canvas);
            tempButton.GetComponentInChildren<Text>().text = buttonWords[i];
            tempButton.GetComponent<Button_Details>().currentWord = buttonWords[i];
            testButtons[i] = tempButton;

        }

        StartCoroutine(waitAFrame());

        

    }
	
	// Update is called once per frame
	void Update () {

        if (!thought.Trim().Equals(MANAGER_Translator.currentThought.Trim())  )
        {
            if (!movedHappened)
            {
                for (int i = 0; i < testButtons.Length; i++)
                {
                    //testButtons[i].SetActive(false);
                    testButtons[i].GetComponent<RectTransform>().anchoredPosition3D += new Vector3(0, 10000, 0);
                }
                reShowWordsOnce = false;
                movedHappened = true;
            }
            return;
        }
        else if(!reShowWordsOnce)
        {
            for (int i = 0; i < testButtons.Length; i++)
            {
                //testButtons[i].SetActive(true);
                testButtons[i].GetComponent<RectTransform>().anchoredPosition3D -= new Vector3(0, 10000, 0);
                if (testButtons[i].GetComponent<Button_Details>().scramble)
                {
                    for(int j = 0; j < Translation.keyWords.Length; j++)
                    {
                        if (testButtons[i].GetComponent<Button_Details>().currentWord.Trim().Equals(Translation.keyWords[j]))
                        {
                            if (Translation.translated[j])
                            {
                                testButtons[i].GetComponent<Button_Details>().scramble = false;
                                testButtons[i].GetComponentInChildren<Text>().text = testButtons[i].GetComponent<Button_Details>().currentWord;
                                testButtons[i].GetComponentInChildren<Text>().color = Color.green;
                                break;
                            }
                        }
                    }
                }
            }
            //Debug.Log("How many times is this getting accessed");
            reShowWordsOnce = true;
            movedHappened = false;
        }
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
           /* for( int i = 0; i < testButtons.Length; i++)
            {
                Destroy(testButtons[i]);
            }
            testButtons = new GameObject[1];
            buttonWords = new string[1];*/
        }
         
       /* if (!once)
        {
            StartCoroutine(waitAFrame());
        }*/
        if (once)
        {
            for (int i = 0; i < testButtons.Length; i++)
            {
                theTestButton = testButtons[i];
                if (i == 0)
                {
                    
                    theTestButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(panelStartPosX + WordOffset.x, panelStartPosY + WordOffset.y);
                    theTestButton.GetComponent<Button_Details>().panel = this.gameObject;
                    //theTestButton.transform.parent = this.gameObject.transform;
                }
                else
                {
                    if (currentWidth + theTestButton.GetComponent<RectTransform>().rect.width > maxWidth)
                    {
                        WordOffset.y -= 25;
                        StartCoroutine(typeWord(theTestButton, theTestButton, true));
                        //theTestButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(testButtons[0].GetComponent<RectTransform>().anchoredPosition3D.x, panelStartPosY + WordOffset.y) + new Vector3(theTestButton.GetComponent<RectTransform>().rect.width / 2, 0, 0) - new Vector3(testButtons[0].GetComponent<RectTransform>().rect.width / 2, 0, 0) ;
                        //go down one
                        //currentWidth = WordOffset.x;
                        //Debug.Log("Is the reset ever accessed?");
                    }
                    else
                    {
                        StartCoroutine(typeWord(theTestButton, testButtons[i - 1], false));
                        //theTestButton.GetComponent<RectTransform>().anchoredPosition3D = testButtons[i - 1].GetComponent<RectTransform>().anchoredPosition3D + new Vector3(testButtons[i - 1].GetComponent<RectTransform>().rect.width / 2, 0, 0) + new Vector3(theTestButton.GetComponent<RectTransform>().rect.width / 2 + 10, 0, 0);
                        //currentWidth += theTestButton.GetComponent<RectTransform>().rect.width + 10;
                    }
                }
                //theTestButton = testButtons[i];
                //theTestButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(panelStartPosX + WordOffset.x, panelStartPosY + WordOffset.y);
                //WordOffset.x += testButtons[i].GetComponent<RectTransform>().rect.width;
                //Debug.Log("current Width; " + currentWidth +" vs. max Width: " + maxWidth);
                //StartCoroutine(waitAFrame(testButtons[i]));
                //Debug.Log(testButtons[i].GetComponent<RectTransform>().rect.height);
            }
            once = false;
        }

    }

    public IEnumerator waitAFrame()
    {
        yield return new WaitForEndOfFrame();
        once = true;
        //Debug.Log(testButton.GetComponent<RectTransform>().rect.width);
            
    }
    public IEnumerator typeWord(GameObject currentButton, GameObject previousButton, bool newLine)
    {
        currentButton.GetComponent<Button_Details>().panel = this.gameObject;
        coroutineIsHappening = true;
        isTyping = true;
        int currentLetter = 0;
        Text tempText = currentButton.GetComponentInChildren<Text>();
        string copyOfWord = tempText.text;
        currentButton.GetComponentInChildren<Text>().text = "";
        if (newLine)
        {
            theTestButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(testButtons[0].GetComponent<RectTransform>().anchoredPosition3D.x, panelStartPosY + WordOffset.y) + new Vector3(theTestButton.GetComponent<RectTransform>().rect.width / 2, 0, 0) - new Vector3(testButtons[0].GetComponent<RectTransform>().rect.width / 2, 0, 0);
            currentWidth = WordOffset.x;
        }
        else
        {
            theTestButton.GetComponent<RectTransform>().anchoredPosition3D = previousButton.GetComponent<RectTransform>().anchoredPosition3D + new Vector3(previousButton.GetComponent<RectTransform>().rect.width / 2, 0, 0) + new Vector3(theTestButton.GetComponent<RectTransform>().rect.width / 2 + 10, 0, 0);
            currentWidth += theTestButton.GetComponent<RectTransform>().rect.width + 10;
        }
        while (currentLetter < copyOfWord.Length)
        {
            currentButton.GetComponentInChildren<Text>().text += copyOfWord[currentLetter];
            currentLetter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        currentButton.GetComponentInChildren<Text>().text = copyOfWord;
        coroutineIsHappening = false;
    }
    
    public int instanceOfWord(string currentWord)
    {
        int counter = 0;
        for(int i = 0; i < testButtons.Length; i++)
        {
            if (currentWord.Trim().Equals(testButtons[i].GetComponent<Button_Details>().currentWord.Trim()))
            {
                counter += 1;
            }
        }
        for( int i = 0; i < otherPanel_1.GetComponent<Translation>().testButtons.Length; i++)
        {
            if (currentWord.Trim().Equals(otherPanel_1.GetComponent<Translation>().testButtons[i].GetComponent<Button_Details>().currentWord.Trim()))
            {
                counter += 1;
            }
        }
        for (int i = 0; i < otherPanel_2.GetComponent<Translation>().testButtons.Length; i++)
        {
            if (currentWord.Trim().Equals(otherPanel_2.GetComponent<Translation>().testButtons[i].GetComponent<Button_Details>().currentWord.Trim()))
            {
                counter += 1;
            }
        }
        if(fourPanel)
        {
            for (int i = 0; i < otherPanel_3.GetComponent<Translation>().testButtons.Length; i++)
            {
                if (currentWord.Trim().Equals(otherPanel_3.GetComponent<Translation>().testButtons[i].GetComponent<Button_Details>().currentWord.Trim()))
                {
                    counter += 1;
                }
            }
        }
        return counter;
    }
    public static void wasTranslated(string word)
    {
        for (int i = 0; i < keyWords.Length; i++)
        {
            if (word.Trim().Equals(keyWords[i].Trim()))
            {
                translated[i] = true;
                break;
            }
        }
    }
        
 }
