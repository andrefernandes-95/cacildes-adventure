namespace AF
{
    public class UI_Healthbar : UI_Statbar
    {
        public CharacterBaseManager character;

        bool hasInitializedUI = false;

        void Start()
        {
            character.health.onHealthChange.AddListener(UpdateUI);

            UpdateUI();
        }

        void UpdateUI()
        {
            SetCurrentValue(character.health.GetCurrentHealth() / character.health.GetMaxHealth());

            RefreshCurrentAndMaxValues((int)character.health.GetCurrentHealth(), character.health.GetMaxHealth());

            if (hasInitializedUI)
            {
                UIUtils.PlayPopEffect(this.gameObject, 1.05f);
            }

            hasInitializedUI = true;
        }
    }
}
