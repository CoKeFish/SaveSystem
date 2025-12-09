namespace Marmary.SaveSystem
{
    /// <summary>
    /// Defines the contract for automatic loading functionality.
    /// </summary>
    public interface IAutoLoad
    {
        /// <summary>
        /// Loads the data or state of an object. This method is expected to handle
        /// the deserialization or retrieval of information to restore the object's state.
        /// </summary>
        void AutoLoad();
    }
}