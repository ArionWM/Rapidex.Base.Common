using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


[assembly: InternalsVisibleTo("Rapidex.UnitTest.Base.MessageHub")]
[assembly: InternalsVisibleTo("Rapidex.UnitTest.Data")]
[assembly: InternalsVisibleTo("Rapidex.UnitTest.Base.Data")]


namespace Rapidex.MessageHub;
internal class Library : AssemblyDefinitionBase, IRapidexAssemblyDefinition
{
    public override string Name => "Message Hub Library";
    public override string TablePrefix => "msg";
    public override int Index => 1;

    public override void SetupMetadata(IServiceCollection services)
    {

    }

    public override void SetupServices(IServiceCollection services)
    {
        MessageHubBuilder hubBuilder = new MessageHubBuilder();
        services.AddSingletonForProd<MessageHubBuilder>(hubBuilder);
        hubBuilder.Setup(services);
    }

    public override void Start(IServiceProvider serviceProvider)
    {
        MessageHubBuilder builder = serviceProvider.GetRequiredService<MessageHubBuilder>();
        builder.Start(serviceProvider);
    }
}
