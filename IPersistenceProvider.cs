namespace Marmary.SaveSystem
{
    /// <summary>
    /// Represents a provider interface for data persistence operations,
    /// allowing saving, loading, and existence checking of data associated with specific keys.
    /// </summary>
    public interface IPersistenceProvider<in TS>
    {
        #region Methods

        /// <summary>
        /// Saves data to the context using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the data to save.</typeparam>
        /// <param name="setting">The setting context in which to save the data.</param>
        /// <param name="key">The key to associate with the saved data.</param>
        /// <param name="value">The value to save.</param>
        public void SaveData<T>(TS setting, string key, T value);

        /// <summary>
        /// Loads data from the context using the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the data to load.</typeparam>
        /// <param name="setting">The setting context from which to load the data.</param>
        /// <param name="key">The key associated with the data to load.</param>
        /// <param name="defaultValue">The default value to return if the key does not exist.</param>
        /// <returns>The loaded data of type <typeparamref name="T"/>.</returns>
        public T LoadData<T>(TS setting, string key, T defaultValue = default);

        /// <summary>
        /// Checks if a key exists in the current save context.
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="key">The key to check for existence.</param>
        /// <returns><c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
        public bool KeyExists(TS setting, string key);

        #endregion
    }
}