using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Typewriter
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypewriterEffect : MonoBehaviour
    {
        private TMP_Text _textBox;
        
        
        // Basic Typewriter Functionality
        private int _currentVisibleCharacterIndex;
        private Coroutine _typewriterCoroutine;
        private bool _readyForNextText = true;

        private WaitForSeconds _simpleDelay;
        private WaitForSeconds _punctuationDelay;

        [Header("Typewriter Settings")]
        [SerializeField] private float characterPerSecond = 20;
        [SerializeField] private float punctuationDelay = 0.5f;
        
        // Skipping Functionality
        public bool CurrentlySkipping { get; private set; }
        private WaitForSeconds _skipDelay;

        [Header("Skip Options")]
        [SerializeField] private bool quickSkip;
        [SerializeField] private int skipSpeedup = 5;
        
        // Even Functionality
        private WaitForSeconds _textBoxFullEventDelay;
        [SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f;

        public static event Action CompleteTextRevealed;
        public static event Action<char> CharacterRevealed; 

        private void Awake()
        {
            _textBox = GetComponent<TMP_Text>();

            _simpleDelay = new WaitForSeconds(1 / characterPerSecond);
            _punctuationDelay = new WaitForSeconds(punctuationDelay);

            _skipDelay = new WaitForSeconds(1 / (characterPerSecond * skipSpeedup));
            _textBoxFullEventDelay = new WaitForSeconds(sendDoneDelay);
        }
        

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
                {
                    Skip();
                }
            }
        }

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNextText);
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNextText);
        }
        

        private void Skip()
        {
            if (CurrentlySkipping)
            {
                return;
            }

            CurrentlySkipping = true;

            if (!quickSkip)
            {
                StartCoroutine(SkipSpeedReset());
                return;
            }
            
            StopCoroutine(_typewriterCoroutine);
            _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
            _readyForNextText = true;
            CompleteTextRevealed?.Invoke();
        }

        private IEnumerator SkipSpeedReset()
        {
            yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
            CurrentlySkipping = false;
        }
        
        public void PrepareForNextText(Object obj)
        {
            if (!_readyForNextText)
            {
                return;
            }

            _readyForNextText = false;
            
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
            }
            
            _textBox.maxVisibleCharacters = 0;
            _currentVisibleCharacterIndex = 0;

            _typewriterCoroutine = StartCoroutine(Typewriter());
        }
        
        private IEnumerator Typewriter()
        {
            TMP_TextInfo textInfo = _textBox.textInfo;

            while (_currentVisibleCharacterIndex < textInfo.characterCount + 1)
            {
                var lastCharacterIndex = textInfo.characterCount - 1;
                if (_currentVisibleCharacterIndex == lastCharacterIndex)
                {
                    _textBox.maxVisibleCharacters++;
                    yield return _textBoxFullEventDelay;
                    CompleteTextRevealed?.Invoke();
                    _readyForNextText = true;
                    yield break;
                }
                

                char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

                _textBox.maxVisibleCharacters++;

                if (!CurrentlySkipping &&
                    (character == '?' || character == '.' || character == ',' || character == ':' ||
                    character == ';' || character == '!' || character == '-'))
                {
                    yield return _punctuationDelay;
                }
                else
                {
                    yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
                }
                CharacterRevealed?.Invoke(character);
                _currentVisibleCharacterIndex++;
            }
        }
    }
}
