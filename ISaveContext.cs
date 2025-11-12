namespace Marmary.HellmenRaaun.Application.Save
{
    /// <summary>
    /// Defines methods for saving and loading data using a key-value mechanism.
    /// </summary>
    public interface ISaveContext
    {
        /// <summary>
        /// Saves a value of type <typeparamref name="T"/> with the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to save.</typeparam>
        /// <param name="key">The key to associate with the value.</param>
        /// <param name="value">The value to save.</param>
        void SaveData<T>(string key, T value);
    
        /// <summary>
        /// Loads a value of type <typeparamref name="T"/> associated with the specified key.
        /// Returns <paramref name="defaultValue"/> if the key does not exist.
        /// </summary>
        /// <typeparam name="T">The type of the value to load.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="defaultValue">The default value to return if the key does not exist.</param>
        /// <returns>The loaded value or the default value.</returns>
        T LoadData<T>(string key, T defaultValue = default);
    
        /// <summary>
        /// Checks if a value exists for the specified key.
        /// </summary>
        /// <param name="key">The key to check for existence.</param>
        /// <returns>True if the key exists; otherwise, false.</returns>
        bool KeyExists(string key);
    }
    
    //TODO: Refactor this move to separate file
    
    /// <summary>
    /// Represents a global save context for application-wide data.
    /// </summary>
    public interface IGlobalSaveContext : ISaveContext { }
    
    /// <summary>
    /// Represents a player-specific save context for user data.
    /// </summary>
    public interface IPlayerSaveContext : ISaveContext { }

}