namespace FroggyDefense.Core
{
    public interface IUsable
    {
        public virtual bool Use()
        {
            return false;
        }
    }
}