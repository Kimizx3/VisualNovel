using System;
using System.Collections.Generic;
using UnityEngine.UI;

[Serializable]
public class TextSegment
{
    public string id;
    public string chapter;
    public List<string> lines = new ();
    public List<Choice> choices = new ();
}

[Serializable]
public class Choice
{
    public string text;
    public string nextId;
}

public class TextData
{
    public Dictionary<string, TextSegment> segments = new();
}

public static class TextParser
{
    public static TextData Parse(string rawText)
    {
        var textData = new TextData();
        string[] blocks = rawText.Split(new[] { "---" }, StringSplitOptions.RemoveEmptyEntries);

        string currentChapter = "";

        foreach (var block in blocks)
        {
            var lines = block.Trim().Split('\n');
            TextSegment segment = new TextSegment();

            foreach (var line in lines)
            {
                string trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed)) continue;
                
                if (trimmed.StartsWith("#"))
                {
                    segment.chapter = trimmed.Substring(1).Trim();
                }
                else if (trimmed.StartsWith("[id:"))
                {
                    // * "Choice text" -> nextId
                    int start = trimmed.IndexOf(":") + 1;
                    int end = trimmed.IndexOf("]");
                    segment.id = trimmed.Substring(start, end - start).Trim();
                    segment.chapter = currentChapter;
                }
                else if (trimmed.StartsWith(">"))
                {
                    // > Look around -> look_around
                    string[] parts = trimmed.Substring(1).Split(
                        new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        segment.choices.Add(new Choice
                        {
                            text = parts[0].Trim(),
                            nextId = parts[1].Trim()
                        });
                    }
                }
                else
                {
                    segment.lines.Add(line);
                }
            }
            if (!string.IsNullOrEmpty(segment.id))
            {
                textData.segments[segment.id] = segment;
            }
        }
        return textData;
    }
}
