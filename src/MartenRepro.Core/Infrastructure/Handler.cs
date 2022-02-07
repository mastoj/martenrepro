namespace MartenRepro.Core.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Handler<TCommand, TDocument, TEvent>
    where TEvent : Event
    where TCommand : Command
    where TDocument : new()
//    where TDocument : Document
{
    public Func<TCommand, TDocument, IEnumerable<TEvent>> Decide { get; }
    public IEventStore EventStore { get; }
    public Func<TDocument, TEvent, TDocument> Build { get; }
    // public TDocument InitialState { get; }

    public Handler(
        Func<TCommand, TDocument, IEnumerable<TEvent>> decide,
        Func<TDocument, TEvent, TDocument> build,
        IEventStore eventStore)
    {
        Decide = decide;
        EventStore = eventStore;
        Build = build;
        // InitialState = initialState;
    }

    public async Task<IEnumerable<TEvent>> Handle(TCommand command)
    {
        var streamId = command.Id;
        Console.WriteLine("==> Reading events");
        var readResult = await EventStore.ReadEventsAsync<TEvent>(streamId);
        var expectedVersion = readResult.Version;
        // Events should never be null
        var events = readResult.Events.Select(y => y!);
        Console.WriteLine("==> Building");
        var currentState = events.Aggregate(new TDocument(), Build);
        Console.WriteLine("==> Deciding");
        var outcome = Decide(command, currentState);
        Console.WriteLine($"==> Writing to stream : {streamId}");
        await EventStore.AppendEventsAsync(streamId, expectedVersion, outcome);

        return outcome.Cast<TEvent>();
    }
}
