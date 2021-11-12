using AnyStatus.API.Dialogs;
using MediatR.Pipeline;
using System;

namespace AnyStatus.Core.Pipeline.Exceptions
{
    /// <summary>
    /// Catch all exceptions.
    /// This class is last in pipeline because it starts with the letter "U".
    /// </summary>
    public class UnhandledExceptionHandler<TRequest, TResponse> : RequestExceptionHandler<TRequest, TResponse>
    {
        private readonly IDialogService _dialogService;

        public UnhandledExceptionHandler(IDialogService dialogService) => _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

        protected override void Handle(TRequest request, Exception exception, RequestExceptionHandlerState<TResponse> state)
        {
            state.SetHandled();

            if (request is ITransientRequest || (exception is AnyStatusException asx && asx.Transient))
            {
                return;
            }

            _dialogService.ShowMessageBox(new ErrorDialog(exception.Message));
        }
    }
}
