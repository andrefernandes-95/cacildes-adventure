using System;

namespace AF
{
    [System.Serializable]
    public class ItemInstance
    {
        public string id;
        public Item item;

        public ItemInstance(string id, Item item)
        {
            this.id = id;
            this.item = item;
        }

        public bool Exists()
        {
            return item != null;
        }

        public bool IsEmpty()
        {
            return !Exists();
        }

        public virtual T GetItem<T>() where T : Item
        {
            return item as T;
        }

        public bool IsEqualTo(ItemInstance itemInstance)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            return id.Equals(itemInstance.id);
        }

        public bool HasItem(Item item)
        {
            return this.item == item;
        }

        public void Clear()
        {
            this.id = "";
            this.item = null;
        }

        public ItemInstance Clone() => new ItemInstance(this.id, this.item);

    }
}
