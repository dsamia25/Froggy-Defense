namespace FroggyDefense.Core
{
    public interface IInteractable
    {
        public bool IsInteractable { get; set; }

        public void Interact();
        //public void SetInteractable(bool interactable);
    }
}