namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Factory abstraction for creating and deleting save files.
    ///     This abstraction allows decoupling from specific persistence implementations,
    ///     making it easy to switch between different save systems (ES3, JSON, PlayerPrefs, etc.)
    /// </summary>
    /// <remarks>
    ///     Identifiers are backend-agnostic relative paths WITHOUT extension
    ///     (e.g. <c>"Slots/abc123/player"</c>); the concrete backend owns the extension.
    /// </remarks>
    public interface IFileFactory
    {
        /// <summary>
        ///     Creates the file for the given identifier.
        /// </summary>
        /// <param name="identifier">Relative path for the file, without extension.</param>
        /// <returns>A new instance of <see cref="IFile" />.</returns>
        IFile CreateFile(string identifier);

        /// <summary>
        ///     Deletes the stored file for the identifier. Does nothing when it does not exist.
        /// </summary>
        /// <param name="identifier">Relative path for the file, without extension.</param>
        void DeleteFile(string identifier);

        /// <summary>
        ///     Deletes the directory for the identifier and everything below it (e.g. a whole slot).
        /// </summary>
        /// <param name="identifier">Relative path for the directory.</param>
        void DeleteDirectory(string identifier);
    }
}