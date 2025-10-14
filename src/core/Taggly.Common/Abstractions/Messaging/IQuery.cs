using MediatR;

namespace Taggly.Common.Abstractions.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
