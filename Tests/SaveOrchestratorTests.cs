using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Marmary.SaveSystem.Tests
{
    /// <summary>
    ///     Semantics of <see cref="SaveOrchestrator" /> and <see cref="AutoSaveRegistration{T}" />:
    ///     registration order, handle-based unregistration and scope glue.
    /// </summary>
    [TestFixture]
    public class SaveOrchestratorTests
    {
        private sealed class RecordingSaver : IAutoSave
        {
            private readonly List<string> _log;
            private readonly string _name;

            public RecordingSaver(List<string> log, string name)
            {
                _log = log;
                _name = name;
            }

            public void AutoSave() => _log.Add(_name);
        }

        private sealed class RecordingLoader : IAutoLoad
        {
            public int Calls;

            public void AutoLoad() => Calls++;
        }

        private sealed class SelfUnregisteringSaver : IAutoSave
        {
            public IDisposable Handle;
            public int Calls;

            public void AutoSave()
            {
                Calls++;
                Handle?.Dispose();
            }
        }

        private sealed class SaverAndLoader : IAutoSave, IAutoLoad
        {
            public int Saves;
            public int Loads;

            public void AutoSave() => Saves++;

            public void AutoLoad() => Loads++;
        }

        private sealed class Neither
        {
        }

        [Test]
        public void SaveAll_InvokesInRegistrationOrder()
        {
            var log = new List<string>();
            var orchestrator = new SaveOrchestrator();
            orchestrator.Register(new RecordingSaver(log, "first"));
            orchestrator.Register(new RecordingSaver(log, "second"));

            orchestrator.SaveAll();

            CollectionAssert.AreEqual(new[] { "first", "second" }, log);
        }

        [Test]
        public void DisposeHandle_Unregisters()
        {
            var log = new List<string>();
            var orchestrator = new SaveOrchestrator();
            var handle = orchestrator.Register(new RecordingSaver(log, "gone"));
            orchestrator.Register(new RecordingSaver(log, "stays"));

            handle.Dispose();
            orchestrator.SaveAll();

            CollectionAssert.AreEqual(new[] { "stays" }, log);
        }

        [Test]
        public void DoubleDispose_IsNoOp()
        {
            var log = new List<string>();
            var orchestrator = new SaveOrchestrator();
            var saver = new RecordingSaver(log, "twice");
            var first = orchestrator.Register(saver);
            orchestrator.Register(saver);

            // Disposing the first handle twice must not remove the second registration.
            first.Dispose();
            first.Dispose();
            orchestrator.SaveAll();

            CollectionAssert.AreEqual(new[] { "twice" }, log);
        }

        [Test]
        public void LoadAll_InvokesLoaders()
        {
            var loader = new RecordingLoader();
            var orchestrator = new SaveOrchestrator();
            var handle = orchestrator.Register(loader);

            orchestrator.LoadAll();
            handle.Dispose();
            orchestrator.LoadAll();

            Assert.AreEqual(1, loader.Calls);
        }

        [Test]
        public void ParticipantUnregisteringDuringSaveAll_DoesNotBreakIteration()
        {
            var orchestrator = new SaveOrchestrator();
            var saver = new SelfUnregisteringSaver();
            saver.Handle = orchestrator.Register(saver);

            orchestrator.SaveAll();
            orchestrator.SaveAll();

            Assert.AreEqual(1, saver.Calls, "The participant unregistered itself during the first pass");
        }

        [Test]
        public void AutoSaveRegistration_RegistersBothInterfaces_AndUnregistersOnDispose()
        {
            var orchestrator = new SaveOrchestrator();
            var target = new SaverAndLoader();

            var registration = new AutoSaveRegistration<SaverAndLoader>(target, orchestrator);
            orchestrator.SaveAll();
            orchestrator.LoadAll();
            registration.Dispose();
            orchestrator.SaveAll();
            orchestrator.LoadAll();

            Assert.AreEqual(1, target.Saves);
            Assert.AreEqual(1, target.Loads);
        }

        [Test]
        public void AutoSaveRegistration_Throws_WhenTargetImplementsNeitherInterface()
        {
            var orchestrator = new SaveOrchestrator();

            Assert.Throws<ArgumentException>(() => _ = new AutoSaveRegistration<Neither>(new Neither(), orchestrator));
        }
    }
}