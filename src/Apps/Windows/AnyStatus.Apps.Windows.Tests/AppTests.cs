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

        [Fact, Priority(1)]
        public void RefreshAllTest()
        {
            Click("RefreshAllButton");
        }

        [Fact, Priority(2)]
        public void ExpandAllAllTest()
        {
            Click("ExpandAllButton");
        }

        [Fact, Priority(3)]
        public void CollapseAllAllTest()
        {
            Click("CollapseAllButton");
        }

        [Fact, Priority(4)]
        public void AddWidgetTest()
        {
            Click("AddWidgetButton");

            Click("BackButton");
        }

        [Fact, Priority(5)]
        public void AddFolderTest()
        {
            Click("AddFolderButton");
        }

        [Fact, Priority(6)]
        public void ShowActivityTest()
        {
            Click("ShowActivityButton");
        }

        [Fact, Priority(8)]
        public void NewSessionTest()
        {
            ToggleMenu();

            Click("NewSessionButton");
        }

        [Fact, Priority(9)]
        public void SettingsTest()
        {
            ToggleMenu();

            Click("SettingsButton");

            Click("BackButton");
        }

        [Fact, Priority(10)]
        public void EndpointsTest()
        {
            ToggleMenu();

            Click("EndpointsButton");

            Click("BackButton");
        }

        [Fact, Priority(11)]
        public void HelpTest()
        {
            ToggleMenu();

            Click("HelpButton");

            Click("BackButton");
        }

        [Fact, Priority(100)]
        public void ExitAppTest()
        {
            ToggleMenu();

            Click("ExitButton");

            Click("ContentDialogSecondaryButton");
        }

        private void ToggleMenu() => Click("ToggleMenuButton");

        private void Click(string accessibilityId) => _fixture.Session.FindElementByAccessibilityId(accessibilityId).Click();
    }
}
