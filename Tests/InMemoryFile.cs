using System.Collections.Generic;

namespace Marmary.SaveSystem.Tests
{
    /// <summary>
    ///     In-memory <see cref="IFile" /> double: stores values by key without serializing.
    /// </summary>
    internal sealed class InMemoryFile : IFile
    {
        private readonly Dictionary<string, object> _data = new();

        public void SaveData<T>(string key, T value) => _data[key] = value;

        public T LoadData<T>(string key, T defaultValue = default) =>
            _data.TryGetValue(key, out var value) ? (T)value : defaultValue;

        public bool KeyExists(string key) => _data.ContainsKey(key);

        public void DeleteKey(string key) => _data.Remove(key);
    }

    /// <summary>
    ///     <see cref="IFileFactory" /> double: returns one <see cref="InMemoryFile" /> per
    ///     identifier and records every create/delete call for assertions.
    /// </summary>
    internal sealed class RecordingFileFactory : IFileFactory
    {
        public readonly List<string> CreatedIdentifiers = new();
        public readonly List<string> DeletedFiles = new();
        public readonly List<string> DeletedDirectories = new();

        private readonly Dictionary<string, InMemoryFile> _files = new();

        public IFile CreateFile(string identifier)
        {
            CreatedIdentifiers.Add(identifier);
            if (!_files.TryGetValue(identifier, out var file))
            {
                file = new InMemoryFile();
                _files[identifier] = file;
            }

            return file;
        }

        public void DeleteFile(string identifier)
        {
            DeletedFiles.Add(identifier);
            _files.Remove(identifier);
        }

        public void DeleteDirectory(string identifier)
        {
            DeletedDirectories.Add(identifier);
        }
    }
}