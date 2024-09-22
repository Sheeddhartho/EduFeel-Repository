using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

public class ComplexObjectInfoManager2 : MonoBehaviour
{
    public TMP_Text responseTextOriginal;   // UI text to display response for original input
    public TMP_Text responseTextDuplicate;  // UI text to display response for duplicate input

    public TMP_InputField originalInputField;    // Original input field
    public TMP_InputField duplicateInputField;   // Duplicate input field

    public UnityEngine.UI.Button originalSearchButton;  // Original search button
    public UnityEngine.UI.Button duplicateSearchButton; // Duplicate search button

    private Dictionary<string, Entry> objectInfoDatabase;

    void Start()
    {
        LoadDatabase();

        // Add listeners for input fields to synchronize both fields
        originalInputField.onValueChanged.AddListener(OnOriginalInputChanged);
        duplicateInputField.onValueChanged.AddListener(OnDuplicateInputChanged);

        // Add listeners for buttons
        originalSearchButton.onClick.AddListener(OnOriginalSearchButtonClicked);
        duplicateSearchButton.onClick.AddListener(OnDuplicateSearchButtonClicked);
    }

    // Load the database
    void LoadDatabase()
    {
        TextAsset databaseText = Resources.Load<TextAsset>("database");
        if (databaseText != null)
        {
            objectInfoDatabase = JsonConvert.DeserializeObject<Dictionary<string, Entry>>(databaseText.text);
        }
        else
        {
            Debug.LogError("Database file not found. Make sure 'database.json' is placed in the Resources folder.");
        }
    }

    // Called when the original input field's text is changed
    private void OnOriginalInputChanged(string userInput)
    {
        duplicateInputField.text = userInput;  // Sync to the duplicate input field
    }

    // Called when the duplicate input field's text is changed
    private void OnDuplicateInputChanged(string userInput)
    {
        originalInputField.text = userInput;   // Sync to the original input field
    }

    // Triggered when the original search button is clicked
    public void OnOriginalSearchButtonClicked()
    {
        string userInput = originalInputField.text;
        GetInfo(userInput, responseTextOriginal, responseTextDuplicate);
    }

    // Triggered when the duplicate search button is clicked
    public void OnDuplicateSearchButtonClicked()
    {
        string userInput = duplicateInputField.text;
        GetInfo(userInput, responseTextOriginal, responseTextDuplicate);
    }

    // Get info and update both response texts
    public void GetInfo(string userInput, TMP_Text responseTextOriginal, TMP_Text responseTextDuplicate)
    {
        string cleanedInput = CleanInput(userInput);
        string[] inputWords = cleanedInput.ToLower().Split(' ');

        var bestMatch = FindBestMatch(inputWords);

        if (bestMatch != null)
        {
            responseTextOriginal.text = bestMatch.response;
            responseTextDuplicate.text = bestMatch.response;
        }
        else
        {
            responseTextOriginal.text = "No exact match found.";
            responseTextDuplicate.text = "No exact match found.";
        }
    }

    // Clean input by removing punctuation
    private string CleanInput(string input)
    {
        return Regex.Replace(input, @"[^\w\s]", "");
    }

    // Find the best matching entry from the database
    private Entry FindBestMatch(string[] inputWords)
    {
        Entry bestMatch = null;
        int maxKeywordMatches = 0;

        foreach (var entry in objectInfoDatabase)
        {
            int matchCount = entry.Value.keywords.Count(keyword => inputWords.Contains(keyword.ToLower()));

            if (matchCount > maxKeywordMatches)
            {
                maxKeywordMatches = matchCount;
                bestMatch = entry.Value;
            }
        }

        return bestMatch;
    }

    [System.Serializable]
    public class Entry
    {
        public List<string> keywords;
        public string response;
    }
}
