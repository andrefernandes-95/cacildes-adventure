namespace AF
{

    [System.Serializable]
    public class DialogueObject
    {
        /// <summary>
        /// Can be optional, useful to allow moments to jump to another ones
        /// </summary>
        public string id;
        /// <summary>
        /// The type of moment. i.e. "dialogue", "wait"
        /// </summary>
        public string type;
        /// <summary>
        /// The data of the moment. will vary based on the type of moment
        /// </summary>
        public DialogueData data;
    }

    [System.Serializable]
    public class DialogueData
    {
        public string actor_id;
        public string pt;
        public string en;
    }

    [System.Serializable]
    public class DialogueWrapper
    {
        public DialogueObject[] dialogues;
    }
}
