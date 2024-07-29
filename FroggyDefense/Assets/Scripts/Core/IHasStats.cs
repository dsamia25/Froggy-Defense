namespace Core
{
    public interface IHasStats
    {
        /// <summary>
        /// Gets the unit's stat sheet.
        /// </summary>
        /// <returns></returns>
        public StatSheet Stats { get; }
    }
}