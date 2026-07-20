using System;
using NUnit.Framework;

namespace Marmary.SaveSystem.Tests
{
    /// <summary>
    ///     Semantics of <see cref="SaveRepository{T}" />: Value is the single gate, eagerly
    ///     loaded, persisted only on Save, refreshed on Reload, removed on Delete.
    /// </summary>
    [TestFixture]
    public class SaveRepositoryTests
    {
        [Test]
        public void Ctor_LoadsExistingValue()
        {
            var file = new InMemoryFile();
            file.SaveData("key", 42);

            var repo = new SaveRepository<int>("key", file, 0);

            Assert.AreEqual(42, repo.Value);
        }

        [Test]
        public void Ctor_MissingKey_UsesDefault()
        {
            var repo = new SaveRepository<int>("key", new InMemoryFile(), 7);

            Assert.AreEqual(7, repo.Value);
            Assert.IsFalse(repo.Exists());
        }

        [Test]
        public void Ctor_Guards()
        {
            Assert.Throws<ArgumentException>(() => _ = new SaveRepository<int>("", new InMemoryFile(), 0));
            Assert.Throws<ArgumentNullException>(() => _ = new SaveRepository<int>("key", null, 0));
        }

        [Test]
        public void ValueSet_DoesNotPersist_UntilSave()
        {
            var file = new InMemoryFile();
            var repo = new SaveRepository<int>("key", file, 0);

            repo.Value = 5;

            Assert.IsFalse(file.KeyExists("key"), "Setting Value must not touch storage");
        }

        [Test]
        public void Save_PersistsValue()
        {
            var file = new InMemoryFile();
            var repo = new SaveRepository<int>("key", file, 0);

            repo.Value = 5;
            repo.Save();

            Assert.IsTrue(repo.Exists());
            Assert.AreEqual(5, file.LoadData<int>("key"));
        }

        [Test]
        public void Reload_UpdatesValueFromStorage_AndReturnsIt()
        {
            var file = new InMemoryFile();
            var repo = new SaveRepository<int>("key", file, 0);
            repo.Value = 5;

            // Storage changes behind the repository's back.
            file.SaveData("key", 99);
            var reloaded = repo.Reload();

            Assert.AreEqual(99, reloaded);
            Assert.AreEqual(99, repo.Value, "Reload must refresh Value, not bypass it");
        }

        [Test]
        public void Delete_RemovesKey_AndResetsValueToDefault()
        {
            var file = new InMemoryFile();
            var repo = new SaveRepository<int>("key", file, 7);
            repo.Value = 5;
            repo.Save();

            repo.Delete();

            Assert.IsFalse(repo.Exists());
            Assert.AreEqual(7, repo.Value, "Delete must reset Value to the default");
        }
    }
}