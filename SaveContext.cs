namespace Marmary.SaveSystem
{
    /// <summary>
    /// Abstract base class for managing save contexts, providing methods for saving and loading data.
    /// </summary>
    public sealed class SaveContext<SettingsType>: ISaveContext
    {
        #region Fields

        /// <summary>
        /// Settings used for configuring the save context, such as file path and encryption.
        /// </summary>
        private readonly SaveSettingsBase<SettingsType> _contextSettings;

        /// <summary>
        /// Provides access to the persistence provider responsible for handling data
        /// saving, loading, and key existence checks within a save context.
        /// </summary>
        private readonly IPersistenceProvider<SettingsType> _persistenceProvider;
        
        public SaveContext(SaveSettingsBase<SettingsType> contextSettings, IPersistenceProvider<SettingsType> persistenceProvider)
        {
            _contextSettings = contextSettings;
            _persistenceProvider = persistenceProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves data to the context using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the data to save.</typeparam>
        /// <param name="key">The key to associate with the saved data.</param>
        /// <param name="value">The value to save.</param>
        public void SaveData<T>(string key, T value)
        {
            _persistenceProvider.SaveData(_contextSettings.Value, key, value);
        }

        /// <summary>
        /// Loads data from the context using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the data to load.</typeparam>
        /// <param name="key">The key associated with the data to load.</param>
        /// <param name="defaultValue">The default value to return if the key does not exist.</param>
        /// <returns>The loaded data of type <typeparamref name="T"/>.</returns>
        public T LoadData<T>(string key, T defaultValue = default)
        {
            return _persistenceProvider.LoadData(_contextSettings.Value, key, defaultValue);
        }

        /// <summary>
        /// Checks if a key exists in the current save context.
        /// </summary>
        /// <param name="key">The key to check for existence.</param>
        /// <returns><c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
        public bool KeyExists(string key)
        {
            return _persistenceProvider.KeyExists(_contextSettings.Value, key);
        }

        #endregion
    }
}