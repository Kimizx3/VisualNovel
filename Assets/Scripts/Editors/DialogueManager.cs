using System;
using System.Collections.Generic;
using UnityEngine;
using Typewriter;

public class DialogueManager : MonoBehaviour
{
    private TypewriterEffect _typewriter;

    private void Start()
    {
        _typewriter = GetComponent<TypewriterEffect>();
    }
}
