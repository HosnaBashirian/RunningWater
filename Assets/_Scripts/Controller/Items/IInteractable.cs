namespace _Scripts.Controller.Items
{
    public interface IInteractable
    {
        bool CheckCollision();
        void Interact();
    }
}