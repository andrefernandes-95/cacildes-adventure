namespace AF
{
    [System.Serializable]
    public class SerializedItem
    {
        public string id;
        public string itemPath;

        public int level = 1;
        public bool wasUsed = false;
    }
}
