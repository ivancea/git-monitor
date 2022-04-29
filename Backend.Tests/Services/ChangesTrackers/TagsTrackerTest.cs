using System.Collections.Generic;
using GitMonitor.Objects.Changes;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitMonitor.Services.ChangesTrackers
{
    [TestClass]
    public class TagsTrackerTest
    {
        public TagsTrackerTest()
        {
            Tags = new List<Tag>();

            var tagCollection = new Mock<TagCollection>();

            tagCollection.Setup(t => t.GetEnumerator()).Returns(() => Tags.GetEnumerator());

            Repository = Mock.Of<IRepository>(r => r.Tags == tagCollection.Object);

            Changes = new List<Change>();
        }

        private List<Tag> Tags { get; }

        private IRepository Repository { get; }

        private List<Change> Changes { get; }

        [TestCleanup]
        public void Cleanup()
        {
            Tags.Clear();
            Changes.Clear();
        }

        [TestMethod]
        public void AddedTag()
        {
            Tags.Add(MockTag("original", "1"));

            using (var tracker = new TagsTracker(Repository, Changes))
            {
                Tags.Clear();
                Tags.Add(MockTag("original", "1"));
                Tags.Add(MockTag("new", "1"));
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Tag, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Created, Changes[0].Type);
            Assert.AreEqual("new", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(TagChange));
            Assert.AreEqual("1", ((TagChange)Changes[0]).TargetCommit);
        }

        [TestMethod]
        public void RemovedTag()
        {
            Tags.Add(MockTag("original", "1"));

            using (var tracker = new TagsTracker(Repository, Changes))
            {
                Tags.Clear();
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Tag, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Deleted, Changes[0].Type);
            Assert.AreEqual("original", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(TagChange));
            Assert.AreEqual("1", ((TagChange)Changes[0]).TargetCommit);
        }

        [TestMethod]
        public void UpdatedTag()
        {
            Tags.Add(MockTag("original", "1"));

            using (var tracker = new TagsTracker(Repository, Changes))
            {
                Tags.Clear();
                Tags.Add(MockTag("original", "2"));
            }

            Assert.AreEqual(1, Changes.Count);
            Assert.AreEqual(ChangeObjectType.Tag, Changes[0].ObjectType);
            Assert.AreEqual(ChangeType.Updated, Changes[0].Type);
            Assert.AreEqual("original", Changes[0].ObjectName);
            Assert.IsInstanceOfType(Changes[0], typeof(TagChange));
            Assert.AreEqual("2", ((TagChange)Changes[0]).TargetCommit);
        }

        private Tag MockTag(string name, string sha)
        {
            return Mock.Of<Tag>(t =>
                t.CanonicalName == name &&
                t.FriendlyName == name &&
                t.Target == Mock.Of<GitObject>(c => c.Sha == sha) &&
                t.PeeledTarget == Mock.Of<GitObject>(c => c.Sha == sha));
        }
    }
}
