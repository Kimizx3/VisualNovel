using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public string fileName = "NovelScript"; // scripts/Text/NovelScript
    public TextData MTextData;
    public string currentSegmentId = "intro";

    private void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Text/" + fileName);
        MTextData = TextParser.Parse(textAsset.text);
        ShowSegment(currentSegmentId);
    }

    void ShowSegment(string id)
    {
        TextSegment seg = MTextData.segments[id];
        foreach (var line in seg.lines)
        {
            Debug.Log(line);
        }

        foreach (var choice in seg.choices)
        {
            Debug.Log($"CHOICE: {choice.text} -> {choice.nextId}");
        }
    }
}
