using MediatR;
using RiraProject.Framework.Domain.Domain;

namespace RiraProject.Framework.Application.Events;

public record DomainEventNotification<TEvent>(TEvent DomainEvent) : INotification
    where TEvent : IDomainEvent;
