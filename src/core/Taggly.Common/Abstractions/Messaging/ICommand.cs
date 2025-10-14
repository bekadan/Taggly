using MediatR;

namespace Taggly.Common.Abstractions.Messaging;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
