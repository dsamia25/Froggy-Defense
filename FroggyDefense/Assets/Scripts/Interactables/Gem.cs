namespace FroggyDefense.Interactables
{
    public class Gem : GroundCurrency
    {
        public void SetGem(GemObject info)
        {
            _amount = info.GemValue;
            _spriteRenderer.color = info.GemColor;
        }
    }
}