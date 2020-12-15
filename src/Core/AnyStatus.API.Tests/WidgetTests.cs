using AnyStatus.API.Widgets;
using Xunit;

namespace AnyStatus.API.Tests
{
    public class WidgetTests
    {
        [Fact]
        public void HasChildren_ReturnsTrue_WhenChildAdded()
        {
            var widget = new MockWidget();

            Assert.False(widget.HasChildren);

            widget.Add(new MockWidget());

            Assert.True(widget.HasChildren);
        }

        [Fact]
        public void Status_IsNone_ByDefault()
        {
            var widget = new MockWidget();

            Assert.Equal(Status.None, widget.Status);
        }

        [Fact]
        public void PreviousStatus_IsNull_ByDefault()
        {
            var widget = new MockWidget();

            Assert.Null(widget.PreviousStatus);
        }

        [Fact]
        public void PreviousStatus_Updated_WhenStatusChange()
        {
            var widget = new MockWidget
            {
                Status = Status.OK
            };

            widget.Status = Status.Error;

            Assert.Equal(Status.OK, widget.PreviousStatus);
        }

        [Fact]
        public void Add_ShouldSetParent_WhenChildAdded()
        {
            var parent = new MockWidget();
            var child = new MockWidget();

            parent.Add(child);

            Assert.Same(parent, child.Parent);
        }

        [Fact]
        public void Add_ShouldContainChild_WhenChildAdded()
        {
            var parent = new MockWidget();
            var child = new MockWidget();

            Assert.DoesNotContain(child, parent);

            parent.Add(child);
            Assert.Contains(child, parent);
        }

        [Fact]
        public void Remove_ShouldRemoveParentChildRelationship()
        {
            var parent = new MockWidget();
            var child = new MockWidget();

            parent.Add(child);

            parent.Remove(child);

            Assert.Null(child.Parent);
            Assert.DoesNotContain(child, parent);
        }

        [Fact]
        public void Status_ShouldChange_WhenChildStatusChanged()
        {
            var parent = new MockWidget
            {
                Status = Status.None
            };

            var child = new MockWidget
            {
                Status = Status.None
            };

            parent.Add(child);

            child.Status = Status.OK;

            Assert.Equal(Status.OK, parent.Status);
        }

        [Fact]
        public void Status_ShouldNotChange_WhenRemovedChildStatusChanged()
        {
            var parent = new MockWidget
            {
                Status = Status.None
            };

            var child = new MockWidget
            {
                Status = Status.None
            };

            parent.Add(child);

            parent.Remove(child);

            child.Status = Status.OK;

            Assert.NotEqual(Status.OK, parent.Status);
        }
    }
}
