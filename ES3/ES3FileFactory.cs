namespace Marmary.SaveSystem.ES3
{
    /// <summary>
    ///     ES3 implementation of <see cref="IFileFactory" />. Owns the ".es3" extension:
    ///     identifiers received from the core are extension-less relative paths.
    /// </summary>
    public sealed class ES3FileFactory : IFileFactory
    {
        #region Fields

        /// <summary>
        ///     The file extension owned by this backend.
        /// </summary>
        private const string Extension = ".es3";

        #endregion

        #region IFileFactory Members

        /// <inheritdoc />
        public IFile CreateFile(string identifier)
        {
            return new ES3FileAdapter(new ES3SaveSettings(identifier + Extension).Value);
        }

        /// <inheritdoc />
        public void DeleteFile(string identifier)
        {
            global::ES3.DeleteFile(new ES3SaveSettings(identifier + Extension).Value);
        }

        /// <inheritdoc />
        public void DeleteDirectory(string identifier)
        {
            global::ES3.DeleteDirectory(new ES3SaveSettings(identifier).Value);
        }

        #endregion
    }
}