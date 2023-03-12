namespace FroggyDefense.Weapons
{
    public interface IUseWeapon
    {
        /// <summary>
        /// Gets the user's direct damage value.
        /// </summary>
        /// <returns></returns>
        public float GetDirectDamage();

        /// <summary>
        /// Gets the user's splash damage value.
        /// </summary>
        /// <returns></returns>
        public float GetSplashDamage();
    }
}