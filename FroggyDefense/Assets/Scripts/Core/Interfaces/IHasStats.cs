namespace FroggyDefense.Core
{
    public interface IHasStats
    {
        // TODO: Maybe chaneg this to a property.
        /// <summary>
        /// Gets the unit's stat sheet.
        /// </summary>
        /// <returns></returns>
        public StatSheet GetStats();
    }
}