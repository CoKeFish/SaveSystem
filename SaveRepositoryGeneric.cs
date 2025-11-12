namespace Marmary.SaveSystem
{
    /// <summary>
    ///     A generic repository class for managing save data associated with a specific key.
    ///     Provides functionality to persist, load, and manage the existence of data in a save context.
    /// </summary>
    /// <typeparam name="T">The type of data to be managed by the repository.</typeparam>
    public class SaveRepositoryGeneric<T>
    {
        #region Fields

        /// <summary>
        ///     Represents the context used to save, load. Contains with path and settings for the save data.
        /// </summary>
        private readonly ISaveContext _context;

        /// <summary>
        ///     The key to save the data under.
        /// </summary>
        private readonly string _key;

        /// <summary>
        ///     The default value to return if the key does not exist.
        /// </summary>
        private readonly T _defaultValue;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the value of type T managed by the repository. This property represents
        ///     the data associated with the key in the save context.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Constructors and Injected

        public SaveRepositoryGeneric(string key, ISaveContext context, T defaultValue = default)
        {
            _key = key;
            _defaultValue = defaultValue;
            _context = context;
            Value = _context.LoadData(_key, defaultValue);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Saves the current value of type <typeparamref name="T" /> to the save context using the specified key.
        /// </summary>
        public void SaveData()
        {
            _context.SaveData(_key, Value);
        }

        /// <summary>
        ///     Loads the value of type <typeparamref name="T" /> from the save context using the specified key.
        ///     Returns the default value if the key does not exist.
        /// </summary>
        /// <returns>The loaded value of type <typeparamref name="T" /> or the default value.</returns>
        public T LoadData()
        {
            return _context.LoadData(_key, _defaultValue);
        }

        /// <summary>
        ///     Checks if the key exists in the save context.
        /// </summary>
        /// <returns><c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
        public bool Exists()
        {
            return _context.KeyExists(_key);
        }

        #endregion
    }
}