using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MANAGER_Translator : MonoBehaviour {

    public static string currentThought = "New School";
    public static string Name;
    public Text thoughtDisplay;
    // Use this for initialization

    void Start()
    {
        thoughtDisplay.text = currentThought;
    }

    // Update is called once per frame
    void Update()
    {
        thoughtDisplay.text = currentThought;
    }
}
