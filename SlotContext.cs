using System;
using System.Collections.Generic;

namespace Marmary.SaveSystem
{
    /// <summary>
    ///     Default <see cref="ISlotContext" /> implementation. Composes canonical paths as
    ///     <c>{root}/{slotId}/{fileId}</c> and caches created files per file id.
    /// </summary>
    /// <remarks>
    ///     The file extension is owned by the backend <see cref="IFileFactory" />; this class
    ///     never appends one.
    /// </remarks>
    public sealed class SlotContext : ISlotContext
    {
        #region Fields

        /// <summary>
        ///     Default root folder that groups every slot directory.
        /// </summary>
        private const string DefaultRoot = "Slots";

        /// <summary>
        ///     Backend factory used to create and delete the slot files.
        /// </summary>
        private readonly IFileFactory _fileFactory;

        /// <summary>
        ///     Root folder under which this slot's directory lives.
        /// </summary>
        private readonly string _root;

        /// <summary>
        ///     Cache of created files, one per file id.
        /// </summary>
        private readonly Dictionary<string, IFile> _files = new();

        #endregion

        #region Properties

        /// <inheritdoc />
        public string SlotId { get; }

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Creates a context for the given slot on top of the backend factory.
        /// </summary>
        /// <param name="slotId">The slot identifier this context is bound to.</param>
        /// <param name="fileFactory">Backend factory that creates and deletes files.</param>
        /// <param name="root">Root folder grouping every slot directory.</param>
        public SlotContext(string slotId, IFileFactory fileFactory, string root = DefaultRoot)
        {
            if (string.IsNullOrEmpty(slotId)) throw new ArgumentException("Slot id is required.", nameof(slotId));
            if (string.IsNullOrEmpty(root)) throw new ArgumentException("Root folder is required.", nameof(root));

            SlotId = slotId;
            _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
            _root = root;
        }

        #endregion

        #region ISlotContext Members

        /// <inheritdoc />
        public IFile GetFile(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) throw new ArgumentException("File id is required.", nameof(fileId));

            if (_files.TryGetValue(fileId, out var file)) return file;

            file = _fileFactory.CreateFile(PathFor(fileId));
            _files[fileId] = file;
            return file;
        }

        /// <inheritdoc />
        public void DeleteFile(string fileId)
        {
            if (string.IsNullOrEmpty(fileId)) throw new ArgumentException("File id is required.", nameof(fileId));

            _fileFactory.DeleteFile(PathFor(fileId));
            _files.Remove(fileId);
        }

        /// <inheritdoc />
        public void DeleteSlot()
        {
            _fileFactory.DeleteDirectory($"{_root}/{SlotId}");
            _files.Clear();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Composes the canonical extension-less path for a file id within this slot.
        /// </summary>
        /// <param name="fileId">Game-defined file identifier.</param>
        /// <returns>The relative path handed to the backend factory.</returns>
        private string PathFor(string fileId)
        {
            return $"{_root}/{SlotId}/{fileId}";
        }

        #endregion
    }
}