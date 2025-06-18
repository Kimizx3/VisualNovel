using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using TMPro;
using Typewriter;


public class TextDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text novelText;
    
    [Header("Text Source")]
    [SerializeField] private string fileName = "test";

    private List<string> _paragraph = new();
    private int _currentIndex = 0;

    private bool waitingForNext = false;
    
    private void Start()
    {
        LoadTextFile();
        TypewriterEffect.CompleteTextRevealed += OnTextFullyRevealed;
        ShowCurrentParagraph();
    }

    private void Update()
    {
        if (waitingForNext && Input.GetMouseButtonDown(0))
        {
            waitingForNext = false;
            _currentIndex++;
            if (_currentIndex < _paragraph.Count)
            {
                ShowCurrentParagraph();
            }
            else
            {
                novelText.text = "<i>End of dialogue.</i>";
            }
        }
    }

    void LoadTextFile()
    {
        TextAsset textFile = Resources.Load<TextAsset>(fileName);
        if (textFile == null)
        {
            return;
        }

        string[] blocks = textFile.text.Split(new[] { "\n\n", "\r\n\r\n" }, System.StringSplitOptions.None);
        foreach (var block in blocks)
        {
            string trimmed = block.Trim();
            if (!string.IsNullOrEmpty(trimmed))
            {
                _paragraph.Add(trimmed);
            }
        }
    }

    private void ShowCurrentParagraph()
    {
        novelText.text = _paragraph[_currentIndex];
    }

    void OnTextFullyRevealed()
    {
        waitingForNext = true;
    }

    private void OnDestroy()
    {
        TypewriterEffect.CompleteTextRevealed -= OnTextFullyRevealed;
    }
}

