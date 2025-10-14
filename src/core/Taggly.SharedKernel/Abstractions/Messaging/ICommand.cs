using MediatR;

namespace Taggly.SharedKernel.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
