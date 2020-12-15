using AnyStatus.API.Widgets;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyStatus.Plugins.Roslyn
{
    [Category("Roslyn")]
    [DisplayName("Inline Roslyn Health Check")]
    [Description("Run an inline Roslyn script. The script output is used to set the status of the widget")]
    public class InlineRoslynHealthCheck : StatusWidget, IStandardWidget, IPollable
    {
        public string Script { get; set; }
    }

    public class AsyncRoslynScriptHealthCheck : AsyncStatusCheck<InlineRoslynHealthCheck>
    {
        protected override async Task Handle(StatusRequest<InlineRoslynHealthCheck> request, CancellationToken cancellationToken)
        {
            var options = ScriptOptions.Default.WithReferences(typeof(IWidget).Assembly);

            var result = await CSharpScript.EvaluateAsync(request.Context.Script, options).ConfigureAwait(false);

            var status = Parse(result);

            request.Context.Status = status;
        }

        private static Status Parse(object result)
        {
            var status = Status.Unknown;

            if (result is Status enumStatus)
            {
                status = enumStatus;
            }
            else if (result is int numericStatus)
            {
                Status.TryParse(numericStatus, out status);
            }
            else if (result is string strStatus)
            {
                if (int.TryParse(strStatus, out numericStatus))
                {
                    Status.TryParse(numericStatus, out status);
                }
                else
                {
                    status = Status.GetAll().FirstOrDefault(k => k.Metadata.DisplayName == strStatus);
                }
            }

            return status;
        }
    }
}
