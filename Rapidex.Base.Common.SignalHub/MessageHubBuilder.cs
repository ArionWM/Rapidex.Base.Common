using Mapster;
using Microsoft.Extensions.Configuration;
using Rapidex.MessageHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex.MessageHub;
internal class MessageHubBuilder : IManager
{
    public void CreatePredefinedContent(IMessageHub hub)
    {
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Editing, "Editing", "Entity", true));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Importing, "Importing", "Entity", true));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Imported, "Imported", "Entity", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Exported, "Exported", "Entity", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Archived, "Archived", "Entity + Behavior", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Unarchived, "Unarchived", "Entity + Behavior", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_TimeArrived, "Time Arrived", "Automation", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Login, "Login", "Authorization", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_Logout, "Logout", "Authorization", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_OnError, "On Error", "System", true));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_SystemStarting, "System Starting", "System", true));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_SystemStarted, "System Started", "System", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_SystemStopping, "System Stopping", "System", true));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_WorkspaceCreated, "Workspace Created", "System", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_WorkspaceDeleted, "Workspace Deleted", "System", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_ModuleInstalled, "Module Installed", "System", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_ModuleActivated, "Module Activated", "System", false));
        hub.RegisterSignalDefinition(new SignalDefinition(SignalConstants.Signal_ModuleDeactivated, "Module Deactivated", "System", false));
    }

    public void Setup(IServiceCollection services)
    {
        if (Rapidex.Common.MessageHub == null)
        {
            MessageHub hub = new MessageHub();
            Rapidex.Common.MessageHub = hub;
        }

        services.AddSingletonForProd<IMessageHub>(Rapidex.Common.MessageHub);
    }

    public void Start(IServiceProvider serviceProvider)
    {

        IMessageHub hub = serviceProvider.GetRequiredService<IMessageHub>();
        this.CreatePredefinedContent(hub);

        //TODO: App.Messages = hub;
        hub.Start(serviceProvider);
    }
}
