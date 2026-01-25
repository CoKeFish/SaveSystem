namespace Marmary.SaveSystem
{
    /// <summary>
    /// Manages the creation and organization of save files within a single slot context,
    /// allowing for modular and efficient handling of different file types, such as meta,
    /// player, lobby, dungeon, and battle.
    /// </summary>
    /// <remarks>
    /// This class provides mechanisms to:
    /// - Facilitate creation of specialized save files for various game contexts.
    /// - Consolidate save data under a unified slot while maintaining separate files for performance.
    /// - Support transient file types, enabling temporary data usage and deletion.
    /// - Streamline batching and caching operations within a slot.
    /// </remarks>
    public class SlotFileFactory
    {
        /// <summary>
        /// Responsible for managing save files within a specific slot.
        /// Provides functionality to load, unload, and handle the lifecycle of various save file types.
        /// </summary>
        /// <remarks>
        /// - Manages save data by organizing it into separate file contexts within a single slot.
        /// - Allows precise control over save data with contextual loading and unloading.
        /// - Supports specialized files (e.g., transient files like battles) that can be handled independently.
        /// </remarks>
        public SlotFileFactory(string slotId, IFileFactory fileFactory)
        {
            SlotId = slotId;
            FileFactory = fileFactory;
        }

        /// <summary>
        /// Represents the unique identifier associated with a specific slot,
        /// used for organizing and managing saved data within that slot.
        /// </summary>
        private string SlotId { get; }

        /// <summary>
        /// Provides an abstraction for creating and managing save files
        /// within a slot-based save system.
        /// </summary>
        /// <remarks>
        /// Acts as a factory for generating save file objects tied to specific slots, enabling:
        /// - Efficient handling of multiple save file types.
        /// - Context-sensitive file management, such as temporary or permanent file creation.
        /// - Decoupling from the underlying file creation implementation via the use of an IFileFactory.
        /// </remarks>
        private IFileFactory FileFactory { get; }


        /// <summary>
        /// Gets the full file path for a specific file type (for debugging or external operations).
        /// </summary>
        /// <param name="fileType">The file type.</param>
        /// <returns>The full path to the file.</returns>
        private string GetFilePath(SlotFileType fileType)
        {
            string fileName = fileType.ToString().ToLowerInvariant();
            return $"{SlotId}/{fileName}.es3";
        }

        /// <summary>
        /// Creates a new file of the specified slot file type using the file factory.
        /// </summary>
        /// <param name="fileType">The type of the file to create.</param>
        /// <returns>An instance of the created file.</returns>
        public IFile CreateFile(SlotFileType fileType)
        {
            var path = GetFilePath(fileType);
            return FileFactory.CreateFile(path);
        }
    }
}
