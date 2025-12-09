namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Abstract base class for save settings, providing a generic value and initialization logic.
    /// </summary>
    /// <typeparam name="T">The type of the value to be saved.</typeparam>
    public abstract class SaveSettingsBase<T>
    {
        #region Properties

        /// <summary>
        ///     Gets the value associated with the save settings.
        /// </summary>
        public T Value { get; protected set; }

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="SaveSettingsBase{T}" /> class.
        /// </summary>
        /// <param name="fileName">The file name used for initialization.</param>
        protected SaveSettingsBase(string fileName)
        {
            Initialize(fileName);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the save settings with the specified file name.
        /// </summary>
        /// <param name="fileName">The file name to initialize from.</param>
        protected abstract void Initialize(string fileName);

        #endregion
    }
}