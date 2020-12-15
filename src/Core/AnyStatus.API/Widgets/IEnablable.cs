namespace AnyStatus.API.Widgets
{
    //make sure it does not conflict with remote enable/disable
    //should be explicit
    public interface IEnablable
    {
        /// <summary>
        ///  When this is true and other requisites are satisfied, the node is in a proper state to be executed.
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
