using System;
using UnityEngine;

namespace Typewriter
{
    public class DisplayNextButton : MonoBehaviour
    {
        [SerializeField] private GameObject nextButton;

        private void OnEnable()
        {
            TypewriterEffect.CompleteTextRevealed += ShowNextButton;
        }

        private void OnDisable()
        {
            TypewriterEffect.CompleteTextRevealed -= ShowNextButton;
        }

        private void ShowNextButton()
        {
            nextButton.SetActive(true);
        }

        private void HideNextButton()
        {
            nextButton.SetActive(false);
        }
    }

}