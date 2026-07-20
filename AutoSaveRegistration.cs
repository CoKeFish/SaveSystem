using System;

namespace Marmary.SaveSystem
{
    /// <summary>
    ///     DI-scope glue: registers the target with the orchestrator on construction and
    ///     unregisters it on <see cref="Dispose" />.
    /// </summary>
    /// <remarks>
    ///     Meant to be created and disposed by the DI scope that owns the target, so the
    ///     registration follows the scope lifetime even when the target itself is an
    ///     externally created instance the container never disposes.
    /// </remarks>
    /// <typeparam name="T">A type implementing <see cref="IAutoSave" /> and/or <see cref="IAutoLoad" />.</typeparam>
    public sealed class AutoSaveRegistration<T> : IDisposable where T : class
    {
        #region Fields

        /// <summary>
        ///     Handle for the save registration; null when <typeparamref name="T" /> is not <see cref="IAutoSave" />.
        /// </summary>
        private readonly IDisposable _saveHandle;

        /// <summary>
        ///     Handle for the load registration; null when <typeparamref name="T" /> is not <see cref="IAutoLoad" />.
        /// </summary>
        private readonly IDisposable _loadHandle;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Registers the target as save and/or load participant.
        /// </summary>
        /// <param name="target">The participant to bind to the orchestrator.</param>
        /// <param name="orchestrator">The orchestrator to register with.</param>
        /// <exception cref="ArgumentException">
        ///     The target implements neither <see cref="IAutoSave" /> nor <see cref="IAutoLoad" />.
        /// </exception>
        public AutoSaveRegistration(T target, ISaveOrchestrator orchestrator)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (orchestrator == null) throw new ArgumentNullException(nameof(orchestrator));

            if (target is IAutoSave saver) _saveHandle = orchestrator.Register(saver);
            if (target is IAutoLoad loader) _loadHandle = orchestrator.Register(loader);

            if (_saveHandle == null && _loadHandle == null)
                throw new ArgumentException(
                    $"{typeof(T).Name} implements neither IAutoSave nor IAutoLoad.", nameof(target));
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///     Unregisters the target. Safe to call multiple times.
        /// </summary>
        public void Dispose()
        {
            _saveHandle?.Dispose();
            _loadHandle?.Dispose();
        }

        #endregion
    }
}