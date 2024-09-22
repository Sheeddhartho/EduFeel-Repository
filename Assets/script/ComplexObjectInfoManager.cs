using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;  // For cleaning input

public class ComplexObjectInfoManager : MonoBehaviour
{
    public TMP_Text responseText; // UI text to display the response
    public TMP_InputField inputField; // UI input field for user input
    public UnityEngine.UI.Button searchButton; // UI button to trigger the search

    private Dictionary<string, Entry> objectInfoDatabase;

    void Start()
    {
        LoadDatabase();
        searchButton.onClick.AddListener(OnSearchButtonClicked); // Add listener for button click
    }

    // Load and parse the JSON database from the Resources folder
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

    // Triggered when the search button is clicked
    public void OnSearchButtonClicked()
    {
        string userInput = inputField.text;
        GetInfo(userInput);
    }

    // Function to process the input and return relevant information
    public void GetInfo(string userInput)
    {
        // Clean the input by removing punctuation
        string cleanedInput = CleanInput(userInput);

        // Split the cleaned input into words
        string[] inputWords = cleanedInput.ToLower().Split(' ');

        // Find the best match based on the keywords in the input
        var bestMatch = FindBestMatch(inputWords);

        // Display the best match response, or a default message if no match is found
        if (bestMatch != null)
        {
            responseText.text = bestMatch.response;
        }
        else
        {
            responseText.text = "I couldn't find an exact answer, but here's some general information on related topics.";
        }
    }

    // Clean the input to remove punctuation and special characters
    private string CleanInput(string input)
    {
        // Regex to remove all punctuation marks, except spaces
        return Regex.Replace(input, @"[^\w\s]", "");
    }

    // Find the best matching entry from the database
    private Entry FindBestMatch(string[] inputWords)
    {
        Entry bestMatch = null;
        int maxKeywordMatches = 0;

        // Iterate over each entry in the database
        foreach (var entry in objectInfoDatabase)
        {
            // Count how many of the input words match the keywords for this entry
            int matchCount = entry.Value.keywords.Count(keyword => inputWords.Contains(keyword.ToLower()));

            // If this entry has more matches than the previous best, set it as the best match
            if (matchCount > maxKeywordMatches)
            {
                maxKeywordMatches = matchCount;
                bestMatch = entry.Value;
            }
        }

        return bestMatch;
    }

    // Data structure for each entry in the JSON database
    [System.Serializable]
    public class Entry
    {
        public List<string> keywords;  // List of keywords for each entry
        public string response;        // Corresponding response for the entry
    }
}
