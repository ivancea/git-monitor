using System.Collections.Generic;
using FluentAssertions;
using GitMonitor.Objects.Changes;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitMonitor.Services.ChangesTrackers
{
    [TestClass]
    public class BranchesTrackerTest
    {
        public BranchesTrackerTest()
        {
            Branches = new List<Branch>();

            var branchCollection = new Mock<BranchCollection>();

            branchCollection.Setup(b => b.GetEnumerator()).Returns(() => Branches.GetEnumerator());

            Repository = Mock.Of<IRepository>(r => r.Branches == branchCollection.Object);

            Changes = new List<Change>();
        }

        private List<Branch> Branches { get; }

        private IRepository Repository { get; }

        private List<Change> Changes { get; }

        [TestCleanup]
        public void Cleanup()
        {
            Branches.Clear();
            Changes.Clear();
        }

        [TestMethod]
        public void AddedBranch()
        {
            Branches.Add(MockBranch("original", "1"));

            using (var tracker = new BranchesTracker(Repository, Changes))
            {
                Branches.Clear();
                Branches.Add(MockBranch("original", "1"));
                Branches.Add(MockBranch("new", "1"));
            }

            Changes.Should().HaveCount(1)
                .And.SatisfyRespectively(c =>
                {
                    c.ObjectType.Should().Be(ChangeObjectType.Branch);
                    c.Type.Should().Be(ChangeType.Created);
                    c.ObjectName.Should().Be("new");
                    c.Should().BeOfType<BranchChange>()
                        .Which.TargetCommit.Should().Be("1");
                });
        }

        [TestMethod]
        public void RemovedBranch()
        {
            Branches.Add(MockBranch("original", "1"));

            using (var tracker = new BranchesTracker(Repository, Changes))
            {
                Branches.Clear();
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Branch, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Deleted, Changes[0].Type);
            Assert.AreEqual("original", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(BranchChange));
            Assert.AreEqual("1", ((BranchChange)Changes[0]).TargetCommit);
        }

        [TestMethod]
        public void UpdatedBranch()
        {
            Branches.Add(MockBranch("original", "1"));

            using (var tracker = new BranchesTracker(Repository, Changes))
            {
                Branches.Clear();
                Branches.Add(MockBranch("original", "2"));
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Branch, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Updated, Changes[0].Type);
            Assert.AreEqual("original", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(BranchChange));
            Assert.AreEqual("2", ((BranchChange)Changes[0]).TargetCommit);
        }

        private Branch MockBranch(string name, string sha)
        {
            return Mock.Of<Branch>(b =>
                b.IsRemote == true &&
                b.RemoteName == "origin" &&
                b.CanonicalName == name &&
                b.FriendlyName == "origin/" + name &&
                b.Tip == Mock.Of<Commit>(c => c.Sha == sha));
        }
    }
}
