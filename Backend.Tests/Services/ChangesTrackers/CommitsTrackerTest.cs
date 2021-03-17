using System;
using System.Collections.Generic;
using GitMonitor.Objects.Changes;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitMonitor.Services.ChangesTrackers
{
    [TestClass]
    public class CommitsTrackerTest
    {
        public CommitsTrackerTest()
        {
            Commits = new List<Commit>();

            var commitCollection = new Mock<IQueryableCommitLog>();

            commitCollection.Setup(c => c.GetEnumerator()).Returns(() => Commits.GetEnumerator());
            commitCollection.Setup(c => c.QueryBy(It.IsAny<CommitFilter>())).Returns(() => commitCollection.Object);

            Repository = Mock.Of<IRepository>(r =>
                r.Commits == commitCollection.Object &&
                r.Refs == new Mock<ReferenceCollection>().Object);

            Changes = new List<Change>();
        }

        private List<Commit> Commits { get; }

        private IRepository Repository { get; }

        private List<Change> Changes { get; }

        [TestCleanup]
        public void Cleanup()
        {
            Commits.Clear();
            Changes.Clear();
        }

        [TestMethod]
        public void AddedCommit()
        {
            Commits.Add(MockCommit("Message1", "1", "U", "E"));

            using (var tracker = new CommitsTracker(Repository, Changes))
            {
                Commits.Clear();
                Commits.Add(MockCommit("Message1", "1", "U", "E"));
                Commits.Add(MockCommit("Message2", "2", "User", "test@example.com"));
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Commit, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Created, Changes[0].Type);
            Assert.AreEqual("Message2", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(CommitChange));
            Assert.AreEqual("2", ((CommitChange)Changes[0]).Hash);
            Assert.AreEqual("Message2", ((CommitChange)Changes[0]).Message);
            Assert.AreEqual("User", ((CommitChange)Changes[0]).User?.Name);
            Assert.AreEqual("test@example.com", ((CommitChange)Changes[0]).User?.Email);
        }

        [TestMethod]
        public void RemovedCommit()
        {
            Commits.Add(MockCommit("Message1", "1", "User", "test@example.com"));

            using (var tracker = new CommitsTracker(Repository, Changes))
            {
                Commits.Clear();
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Commit, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Deleted, Changes[0].Type);
            Assert.AreEqual("Message1", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(CommitChange));
            Assert.AreEqual("1", ((CommitChange)Changes[0]).Hash);
            Assert.AreEqual("Message1", ((CommitChange)Changes[0]).Message);
            Assert.AreEqual("User", ((CommitChange)Changes[0]).User?.Name);
            Assert.AreEqual("test@example.com", ((CommitChange)Changes[0]).User?.Email);
        }

        private Commit MockCommit(string message, string sha, string user, string email)
        {
            return Mock.Of<Commit>(c =>
                c.Message == message &&
                c.MessageShort == message &&
                c.Sha == sha &&
                c.Committer == new Signature(user, email, DateTimeOffset.FromUnixTimeSeconds(1)));
        }
    }
}
