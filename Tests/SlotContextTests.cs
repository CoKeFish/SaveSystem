using System;
using NUnit.Framework;

namespace Marmary.SaveSystem.Tests
{
    /// <summary>
    ///     Semantics of <see cref="SlotContext" />: canonical extension-less paths, one cached
    ///     file per file id, and delete operations delegated to the backend factory.
    /// </summary>
    [TestFixture]
    public class SlotContextTests
    {
        [Test]
        public void GetFile_ComposesCanonicalPath()
        {
            var factory = new RecordingFileFactory();
            var context = new SlotContext("slot1", factory);

            context.GetFile("meta");

            Assert.AreEqual("Slots/slot1/meta", factory.CreatedIdentifiers[0]);
        }

        [Test]
        public void CustomRoot_ComposesPath()
        {
            var factory = new RecordingFileFactory();
            var context = new SlotContext("slot1", factory, "Custom");

            context.GetFile("meta");

            Assert.AreEqual("Custom/slot1/meta", factory.CreatedIdentifiers[0]);
        }

        [Test]
        public void GetFile_CachesPerFileId()
        {
            var factory = new RecordingFileFactory();
            var context = new SlotContext("slot1", factory);

            var first = context.GetFile("meta");
            var second = context.GetFile("meta");

            Assert.AreSame(first, second);
            Assert.AreEqual(1, factory.CreatedIdentifiers.Count, "Factory must be hit once per file id");
        }

        [Test]
        public void DeleteFile_DelegatesPath_AndEvictsCache()
        {
            var factory = new RecordingFileFactory();
            var context = new SlotContext("slot1", factory);
            context.GetFile("battle");

            context.DeleteFile("battle");
            context.GetFile("battle");

            Assert.AreEqual("Slots/slot1/battle", factory.DeletedFiles[0]);
            Assert.AreEqual(2, factory.CreatedIdentifiers.Count,
                "A deleted file id must be re-created on next request");
        }

        [Test]
        public void DeleteSlot_DelegatesDirectory_AndClearsCache()
        {
            var factory = new RecordingFileFactory();
            var context = new SlotContext("slot1", factory);
            context.GetFile("meta");

            context.DeleteSlot();
            context.GetFile("meta");

            Assert.AreEqual("Slots/slot1", factory.DeletedDirectories[0]);
            Assert.AreEqual(2, factory.CreatedIdentifiers.Count, "The cache must be empty after DeleteSlot");
        }

        [Test]
        public void Ctor_Guards()
        {
            var factory = new RecordingFileFactory();

            Assert.Throws<ArgumentException>(() => _ = new SlotContext("", factory));
            Assert.Throws<ArgumentNullException>(() => _ = new SlotContext("slot1", null));
            Assert.Throws<ArgumentException>(() => _ = new SlotContext("slot1", factory, ""));
        }

        [Test]
        public void GetFile_Guards()
        {
            var context = new SlotContext("slot1", new RecordingFileFactory());

            Assert.Throws<ArgumentException>(() => context.GetFile(""));
            Assert.Throws<ArgumentException>(() => context.DeleteFile(""));
        }
    }
}