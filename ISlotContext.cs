namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Per-slot save context: resolves game-defined file ids to <see cref="IFile" />
    ///     instances stored under the canonical slot path, and supports deleting individual
    ///     files or the whole slot.
    /// </summary>
    /// <remarks>
    ///     File ids are defined by the consuming game (e.g. "meta", "player"), keeping the
    ///     library free of game-specific file types. This is the piece intended to be
    ///     registered in the DI scope that represents an open slot.
    /// </remarks>
    public interface ISlotContext
    {
        #region Properties

        /// <summary>
        ///     Gets the slot identifier this context is bound to.
        /// </summary>
        string SlotId { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns the file for the given file id, creating and caching it on first request.
        /// </summary>
        /// <param name="fileId">Game-defined file identifier (e.g. "meta", "player").</param>
        /// <returns>The <see cref="IFile" /> bound to the id within this slot.</returns>
        IFile GetFile(string fileId);

        /// <summary>
        ///     Deletes the backing file for the file id and evicts it from the cache.
        /// </summary>
        /// <param name="fileId">Game-defined file identifier.</param>
        void DeleteFile(string fileId);

        /// <summary>
        ///     Deletes the whole slot directory and clears the cache.
        /// </summary>
        void DeleteSlot();

        #endregion
    }
}