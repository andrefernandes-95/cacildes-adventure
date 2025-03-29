using System;
using System.Collections;
using AF.Dialogue;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace AF
{
    public class EV_Dialogue : EventBase
    {
        [Header("Databases")]
        [SerializeField] ActorsDatabase actorsDatabase;

        [SerializeField] TextAsset dialogueFile;

        UIDocumentDialogueWindow uIDocumentDialogueWindow;

        public override IEnumerator Dispatch()
        {
            if (dialogueFile != null)
            {
                DialogueWrapper wrapper = JsonUtility.FromJson<DialogueWrapper>(dialogueFile.text);
                DialogueObject[] dialogues = wrapper.dialogues;
                // Use the moments array

                yield return DispatchDialogues(dialogues);
            }

            yield return null;
        }

        IEnumerator DispatchDialogues(DialogueObject[] dialogues)
        {
            Debug.Log("Start...");

            foreach (DialogueObject dialogueData in dialogues)
            {
                yield return ParseDialogue(dialogueData);
            }

            Debug.Log("End...");
        }
        IEnumerator ParseDialogue(DialogueObject dialogue)
        {
            DialogueData data = dialogue.data;

            // Only consider responses that are active - we hide responses based on composition of nested objects
            Response[] filteredResponses = Array.Empty<Response>();

            Character character = actorsDatabase.GetCharacterById(data.actor_id);

            yield return GetUIDocumentDialogueWindow().DisplayMessage(
                character, data.en, filteredResponses);
        }


        private void OnDisable()
        {
            StopAllCoroutines();
        }

        UIDocumentDialogueWindow GetUIDocumentDialogueWindow()
        {
            if (uIDocumentDialogueWindow == null)
            {
                uIDocumentDialogueWindow = FindAnyObjectByType<UIDocumentDialogueWindow>(FindObjectsInactive.Include);
            }

            return uIDocumentDialogueWindow;
        }
    }
}
