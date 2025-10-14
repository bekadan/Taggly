using MediatR;

namespace Taggly.SharedKernel.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
