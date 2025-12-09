namespace Marmary.SettingsSystem
{
    /// <summary>
    /// Marker interface for objects capable of persisting their state.
    /// </summary>
    public interface IAutoSave
    {
        /// <summary>
        /// Persists the current state to the underlying repository.
        /// </summary>
        void AutoSave();
    }
}

