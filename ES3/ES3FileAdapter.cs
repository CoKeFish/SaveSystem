namespace Marmary.SaveSystem.ES3
{
    /// <summary>
    ///     Easy Save 3 backed <see cref="IFile" />. Write-through: every mutation is synced
    ///     to disk immediately.
    /// </summary>
    /// <remarks>
    ///     ES3 behavior: syncing an empty file deletes it from disk, so removing the last
    ///     key removes the physical file.
    /// </remarks>
    public sealed class ES3FileAdapter : IFile
    {
        #region Fields

        /// <summary>
        ///     The underlying ES3 file cache used for storage operations.
        /// </summary>
        private readonly ES3File _file;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Creates an adapter over the file described by the given ES3 settings.
        /// </summary>
        /// <param name="settings">The ES3 settings describing the file.</param>
        public ES3FileAdapter(ES3Settings settings)
        {
            _file = new ES3File(settings);
        }

        /// <summary>
        ///     Creates an adapter over the file at the given path (extension included).
        /// </summary>
        /// <param name="path">The file path, extension included.</param>
        public ES3FileAdapter(string path)
        {
            _file = new ES3File(path);
        }

        #endregion

        #region IFile Members

        /// <inheritdoc />
        public void SaveData<T>(string key, T value)
        {
            _file.Save(key, value);
            _file.Sync();
        }

        /// <inheritdoc />
        public T LoadData<T>(string key, T defaultValue = default)
        {
            return _file.Load(key, defaultValue);
        }

        /// <inheritdoc />
        public bool KeyExists(string key)
        {
            return _file.KeyExists(key);
        }

        /// <inheritdoc />
        public void DeleteKey(string key)
        {
            // ES3File.DeleteKey only touches the in-memory cache; Sync persists the removal.
            _file.DeleteKey(key);
            _file.Sync();
        }

        #endregion
    }
}