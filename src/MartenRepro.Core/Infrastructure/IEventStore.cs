namespace MartenRepro.Core.Infrastructure;

public record ReadResult<TEvent>(long Version, IEnumerable<TEvent?> Events);

public interface IEventStore
{
//    Task<DocumentWrapper<TDocument>> AggregateAsync<TDocument>(Guid streamId) where TDocument : Document;
    Task AppendEventsAsync<TEvent>(Guid streamId, long expectedVersion, IEnumerable<TEvent> events);
    Task<ReadResult<TEvent>> ReadEventsAsync<TEvent>(Guid streamId) where TEvent : class;
    void Subscribe(Action<object> subscriber);
}

