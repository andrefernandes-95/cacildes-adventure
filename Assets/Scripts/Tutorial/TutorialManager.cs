
using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace AF.Tutorial
{
    [Preserve] // PreserveAttribute prevents byte code stripping from removing a class, method, field, or property. This class has unity event references only, so unity is ignoring it when producing the build :O

    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialSection[] tutorialSections;
        [SerializeField] private TutorialSection startingTutorial;

        private TutorialSection activeTutorialSection;

        void Start()
        {
            InitializeTutorials();
        }

        private void InitializeTutorials()
        {
            DisableAllTutorials();

            if (startingTutorial != null)
            {
                activeTutorialSection = startingTutorial;
            }
            else if (tutorialSections.Length > 0)
            {
                activeTutorialSection = tutorialSections[0];
            }

            if (activeTutorialSection != null)
            {
                activeTutorialSection.gameObject.SetActive(true);
                activeTutorialSection.Activate();
            }
        }

        private void DisableAllTutorials()
        {
            foreach (var tutorial in tutorialSections)
            {
                if (tutorial != null)
                {
                    tutorial.gameObject.SetActive(false);
                }
            }
        }

        public void Advance()
        {
            ChangeTutorialSection(1);
        }

        public void Return()
        {
            ChangeTutorialSection(-1);
        }

        private void ChangeTutorialSection(int direction)
        {
            int currentIndex = Array.IndexOf(tutorialSections, activeTutorialSection);

            if (currentIndex >= 0)
            {
                int newIndex = currentIndex + direction;

                if (newIndex >= 0 && newIndex < tutorialSections.Length)
                {
                    activeTutorialSection.gameObject.SetActive(false);
                    activeTutorialSection = tutorialSections[newIndex];
                    activeTutorialSection.gameObject.SetActive(true);
                    activeTutorialSection.Activate();
                }
            }
        }
    }
}
