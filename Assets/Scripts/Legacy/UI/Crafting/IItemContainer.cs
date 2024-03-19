
public interface IItemContainer
{
    bool Contains(Item item);
    bool RemoveItem(Item item);
    bool AddItem(Item item);
    bool isFull();

}
