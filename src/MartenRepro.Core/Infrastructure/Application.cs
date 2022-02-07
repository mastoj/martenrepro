using MartenRepro.Core.Domain;
using MartenRepro.Core.Domain.TemplateFolder;

namespace MartenRepro.Core.Infrastructure;

public class Application {
    private Handler<TemplateFolderCommand, TemplateFolder, TemplateFolderEvent> templateFolderHandler;

    public Application(IEventStore eventStore, Func<string, bool> checkFolderExists)
    {
        templateFolderHandler = new HandlerBuilder { EventStore = eventStore, FolderExists = checkFolderExists}.Build();
    }

    public async Task<object> Execute(Command command)
    {
        var result = command switch {
            TemplateFolderCommand templateFolderCommand => 
                (await templateFolderHandler.Handle(templateFolderCommand)).Select(y => (object)y),
            _ => throw new System.ArgumentException("Unknown command", nameof(command))
        };
        return result;
    }
}
