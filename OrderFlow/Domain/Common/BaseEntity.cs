using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        [NotMapped]
        private readonly List<BaseDomainEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyCollection<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(BaseDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }

    public abstract class BaseDomainEvent
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.Now;
    }

}
