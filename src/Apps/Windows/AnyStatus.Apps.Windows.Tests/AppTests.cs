using System;
using Xunit;
using Xunit.Priority;

namespace AnyStatus.Apps.Windows.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class AppTests : IClassFixture<AppFixture>
    {
        private readonly AppFixture _fixture;

        public AppTests(AppFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        [Fact(Skip = "Unable to change view state"), Priority(0)]
        public void ToggleMenuTest()
        {
            var menu = _fixture.Session.FindElementByAccessibilityId("Menu");

            var isEnabled = menu.Enabled;

            var toggleButton = _fixture.Session.FindElementByAccessibilityId("ToggleMenuButton");

            toggleButton.Click();

            Assert.NotEqual(menu.Enabled, isEnabled);

            toggleButton.Click();

            Assert.Equal(menu.Enabled, isEnabled);
        }

        [Fact, Priority(1)]
        public void RefreshAllTest()
        {
            _fixture.Session.FindElementByAccessibilityId("RefreshAllButton").Click();
        }

        [Fact, Priority(2)]
        public void ExpandAllAllTest()
        {
            _fixture.Session.FindElementByAccessibilityId("ExpandAllButton").Click();
        }

        [Fact, Priority(3)]
        public void CollapseAllAllTest()
        {
            _fixture.Session.FindElementByAccessibilityId("CollapseAllButton").Click();
        }

        [Fact, Priority(4)]
        public void AddWidgetTest()
        {
            _fixture.Session.FindElementByAccessibilityId("AddWidgetButton").Click();

            _fixture.Session.FindElementByAccessibilityId("BackButton").Click();
        }

        [Fact, Priority(5)]
        public void AddFolderTest()
        {
            _fixture.Session.FindElementByAccessibilityId("AddFolderButton").Click();
        }

        [Fact, Priority(6)]
        public void ShowActivityTest()
        {
            _fixture.Session.FindElementByAccessibilityId("ShowActivityButton").Click();
        }

        [Fact, Priority(8)]
        public void NewSessionTest()
        {
            _fixture.Session.FindElementByAccessibilityId("ToggleMenuButton").Click();

            _fixture.Session.FindElementByAccessibilityId("NewSessionButton").Click();
        }

        [Fact, Priority(9)]
        public void OptionsTest()
        {
            _fixture.Session.FindElementByAccessibilityId("ToggleMenuButton").Click();

            _fixture.Session.FindElementByAccessibilityId("OptionsButton").Click();

            _fixture.Session.FindElementByAccessibilityId("BackButton").Click();
        }

        [Fact, Priority(10)]
        public void ExitAppTest()
        {
            _fixture.Session.FindElementByAccessibilityId("ToggleMenuButton").Click();

            _fixture.Session.FindElementByAccessibilityId("ExitButton").Click();
            
            _fixture.Session.FindElementByAccessibilityId("ContentDialogSecondaryButton").Click();
            
            //_fixture.Session.FindElementByName("Save Changes?").FindElementByName("No").Click();
        }
    }
}
