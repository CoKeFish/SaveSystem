using System;
using System.Collections.Generic;

namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Default <see cref="ISaveOrchestrator" /> implementation.
    /// </summary>
    /// <remarks>
    ///     Not thread-safe: intended for main-thread use. <see cref="SaveAll" /> and
    ///     <see cref="LoadAll" /> iterate over a snapshot, so participants may safely
    ///     unregister while a pass is running. Participants run in registration order,
    ///     which follows the construction order of the scopes that register them.
    /// </remarks>
    public sealed class SaveOrchestrator : ISaveOrchestrator
    {
        #region Fields

        /// <summary>
        ///     Currently registered save participants, in registration order.
        /// </summary>
        private readonly List<IAutoSave> _savers = new();

        /// <summary>
        ///     Currently registered load participants, in registration order.
        /// </summary>
        private readonly List<IAutoLoad> _loaders = new();

        #endregion

        #region ISaveOrchestrator Members

        /// <inheritdoc />
        public IDisposable Register(IAutoSave participant)
        {
            if (participant == null) throw new ArgumentNullException(nameof(participant));

            _savers.Add(participant);
            return new Handle(() => _savers.Remove(participant));
        }

        /// <inheritdoc />
        public IDisposable Register(IAutoLoad participant)
        {
            if (participant == null) throw new ArgumentNullException(nameof(participant));

            _loaders.Add(participant);
            return new Handle(() => _loaders.Remove(participant));
        }

        /// <inheritdoc />
        public void SaveAll()
        {
            foreach (var saver in _savers.ToArray()) saver.AutoSave();
        }

        /// <inheritdoc />
        public void LoadAll()
        {
            foreach (var loader in _loaders.ToArray()) loader.AutoLoad();
        }

        #endregion

        /// <summary>
        ///     Registration handle: removes the participant on dispose. Double-dispose is a no-op.
        /// </summary>
        private sealed class Handle : IDisposable
        {
            /// <summary>
            ///     Pending unregister action; null once disposed.
            /// </summary>
            private Action _unregister;

            public Handle(Action unregister)
            {
                _unregister = unregister;
            }

            public void Dispose()
            {
                _unregister?.Invoke();
                _unregister = null;
            }
        }
    }
}