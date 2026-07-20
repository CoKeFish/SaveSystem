using System.Text;

namespace Marmary.SaveSystem.ES3
{
    /// <summary>
    ///     Builds the canonical <see cref="ES3Settings" /> used by this backend: UTF-8,
    ///     no encryption, file storage under PersistentDataPath.
    /// </summary>
    public sealed class ES3SaveSettings
    {
        #region Properties

        /// <summary>
        ///     Gets the configured ES3 settings.
        /// </summary>
        public ES3Settings Value { get; }

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Creates settings for the given file name (extension included).
        /// </summary>
        /// <param name="fileName">The file path, extension included.</param>
        public ES3SaveSettings(string fileName)
        {
            Value = new ES3Settings
            {
                encryptionType = global::ES3.EncryptionType.None,
                encoding = Encoding.UTF8,
                directory = global::ES3.Directory.PersistentDataPath,
                path = fileName,
                location = global::ES3.Location.File
            };
        }

        #endregion
    }
}