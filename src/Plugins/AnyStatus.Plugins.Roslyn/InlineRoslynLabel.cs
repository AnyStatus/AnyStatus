using AnyStatus.API.Widgets;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Roslyn
{
    [Category("Roslyn")]
    [DisplayName("Inline Roslyn Text Label")]
    [Description("Run an inline Roslyn script to update the label of a widget")]
    public class InlineRoslynLabel : TextLabelWidget, IStandardWidget, IPollable
    {
        public string Script { get; set; }

        public string Expected { get; set; }
    }

    public class AsyncInlineRoslynLabelUpdater : AsyncStatusCheck<InlineRoslynLabel>
    {
        protected override async Task Handle(StatusRequest<InlineRoslynLabel> request, CancellationToken cancellationToken)
        {
            var result = await CSharpScript.EvaluateAsync(request.Context.Script).ConfigureAwait(false);

            request.Context.Text = result.ToString();

            request.Context.Status = string.IsNullOrWhiteSpace(request.Context.Expected) || request.Context.Text == request.Context.Expected ?
                Status.OK :
                Status.Failed;
        }
    }
}
