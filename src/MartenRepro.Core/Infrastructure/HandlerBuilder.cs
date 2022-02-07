namespace MartenRepro.Core.Infrastructure;

public abstract record HandlerBuilder<TCommand, TDocument, TEvent> 
    where TEvent : Event
    where TCommand : Command
    where TDocument : new()
{
    public Func<TCommand, TDocument?, IEnumerable<TEvent>> Decide { get; init; }
    public IEventStore EventStore { get; init; }

    public HandlerBuilder<TCommand, TDocument, TEvent> With(IEventStore eventStore) {
        return this with { EventStore = eventStore };
    }

    public HandlerBuilder<TCommand, TDocument, TEvent> With(Func<TCommand, TDocument?, IEnumerable<TEvent>> decide) {
        return this with { Decide = decide };
    }

    public abstract Handler<TCommand, TDocument, TEvent> Build();
}
