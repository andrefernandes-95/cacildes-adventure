namespace AF
{
    using UnityEngine;

    public class BlacksmithAnvilTrigger : GenericTrigger
    {
        [SerializeField] BlacksmithAnvil blacksmithAnvil;

        UI_Blacksmith uI_Blacksmith;

        protected override void OnActivate(CharacterBaseManager character)
        {
            character.characterBaseBlacksmithManager.BeginJob(blacksmithAnvil);

            if (character is PlayerManager playerManager)
            {
                GetUI_Blacksmith().gameObject.SetActive(true);
            }
        }

        UI_Blacksmith GetUI_Blacksmith()
        {
            if (uI_Blacksmith == null)
            {
                uI_Blacksmith = FindAnyObjectByType<UI_Blacksmith>(FindObjectsInactive.Include);
            }

            return uI_Blacksmith;
        }
    }
}
