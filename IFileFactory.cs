namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Factory interface for creating save contexts.
    ///     This abstraction allows decoupling from specific persistence implementations,
    ///     making it easy to switch between different save systems (ES3, JSON, PlayerPrefs, etc.)
    /// </summary>
    public interface IFileFactory
    {
        /// <summary>
        ///     Creates a new save context with the specified identifier.
        /// </summary>
        /// <param name="identifier">
        ///     A unique identifier for the save context, typically used as the file name
        ///     or storage key (without extension).
        /// </param>
        /// <returns>A new instance of <see cref="IFile"/>.</returns>
        IFile CreateFile(string identifier);
    }
}
