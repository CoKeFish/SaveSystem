using System;

namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Central registry that fans out save/load requests to every currently registered
    ///     participant, regardless of which DI scope owns it.
    /// </summary>
    /// <remarks>
    ///     Participants register on construction and unregister by disposing the returned
    ///     handle, so registration can follow the lifetime of nested DI scopes.
    /// </remarks>
    public interface ISaveOrchestrator
    {
        #region Methods

        /// <summary>
        ///     Registers a save participant. Dispose the returned handle to unregister.
        /// </summary>
        /// <param name="participant">The participant to notify on <see cref="SaveAll" />.</param>
        /// <returns>A handle whose disposal unregisters the participant.</returns>
        IDisposable Register(IAutoSave participant);

        /// <summary>
        ///     Registers a load participant. Dispose the returned handle to unregister.
        /// </summary>
        /// <param name="participant">The participant to notify on <see cref="LoadAll" />.</param>
        /// <returns>A handle whose disposal unregisters the participant.</returns>
        IDisposable Register(IAutoLoad participant);

        /// <summary>
        ///     Invokes <see cref="IAutoSave.AutoSave" /> on every registered participant,
        ///     in registration order.
        /// </summary>
        void SaveAll();

        /// <summary>
        ///     Invokes <see cref="IAutoLoad.AutoLoad" /> on every registered participant,
        ///     in registration order.
        /// </summary>
        void LoadAll();

        #endregion
    }
}