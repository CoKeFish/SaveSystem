using System;

namespace Marmary.SaveSystem
{
    /// <summary>
    ///     A typed variable backed by a save file entry. <see cref="Value" /> is the single
    ///     gate to the data: eagerly loaded on construction, persisted explicitly with
    ///     <see cref="Save" />, re-read with <see cref="Reload" />.
    /// </summary>
    /// <typeparam name="T">The type of the backed value.</typeparam>
    public sealed class SaveRepository<T>
    {
        #region Fields

        /// <summary>
        ///     The file that backs this repository.
        /// </summary>
        private readonly IFile _file;

        /// <summary>
        ///     The key the value is stored under.
        /// </summary>
        private readonly string _key;

        /// <summary>
        ///     The value used when the key does not exist in storage.
        /// </summary>
        private readonly T _defaultValue;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the in-memory value backed by the file entry.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Creates the repository and eagerly loads <see cref="Value" /> from storage,
        ///     falling back to the mandatory <paramref name="defaultValue" />.
        /// </summary>
        /// <param name="key">The key the value is stored under.</param>
        /// <param name="file">The file that backs this repository.</param>
        /// <param name="defaultValue">The value used when the key does not exist.</param>
        public SaveRepository(string key, IFile file, T defaultValue)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Key is required.", nameof(key));

            _key = key;
            _file = file ?? throw new ArgumentNullException(nameof(file));
            _defaultValue = defaultValue;
            Value = _file.LoadData(_key, _defaultValue);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Persists the current <see cref="Value" /> under the key.
        /// </summary>
        public void Save()
        {
            _file.SaveData(_key, Value);
        }

        /// <summary>
        ///     Re-reads the value from storage, updates <see cref="Value" /> and returns it.
        /// </summary>
        /// <returns>The freshly loaded value.</returns>
        public T Reload()
        {
            Value = _file.LoadData(_key, _defaultValue);
            return Value;
        }

        /// <summary>
        ///     Checks if the key exists in storage.
        /// </summary>
        /// <returns><c>true</c> if the key exists; otherwise, <c>false</c>.</returns>
        public bool Exists()
        {
            return _file.KeyExists(_key);
        }

        /// <summary>
        ///     Deletes the key from storage and resets <see cref="Value" /> to the default.
        /// </summary>
        public void Delete()
        {
            _file.DeleteKey(_key);
            Value = _defaultValue;
        }

        #endregion
    }
}