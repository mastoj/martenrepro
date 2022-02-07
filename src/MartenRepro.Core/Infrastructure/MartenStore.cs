using Marten;
using Marten.Exceptions;

namespace MartenRepro.Core.Infrastructure;

public class MartenStore : IEventStore
{
    private readonly IDocumentSession _session;

    public MartenStore(IDocumentSession session)
    {
        _session = session;
    }

    public Task AppendEventsAsync<TEvent>(Guid streamId, long expectedVersion, IEnumerable<TEvent> events)
    {
        Console.WriteLine("==> Appending events: " + streamId);

        foreach (var @event in events)
        {
            if (@event == null)
            {
                continue;
            }

            _session.Events.Append(streamId, expectedVersion++, @event);
        }

        return _session.SaveChangesAsync();
    }

    public async Task<ReadResult<TEvent>> ReadEventsAsync<TEvent>(Guid streamId) where TEvent : class
    {
        Console.WriteLine("==> Fetching events for: " + streamId);
        var eventData = _session.Events.FetchStream(streamId);

        Console.WriteLine("==> Read events for: " + streamId);
        foreach (var e in eventData)
        {
            Console.WriteLine($"ID: {e.Id}, Version: {e.Version}");
        }

        var version = eventData.MaxBy(e => e.Version)?.Version ?? 0;

        return new ReadResult<TEvent>(version, eventData.Select(y => y.Data as TEvent).ToArray());
    }

    public void Subscribe(Action<object> subscriber)
    {
        throw new NotImplementedException();
    }
}
