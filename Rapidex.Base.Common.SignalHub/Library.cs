using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


[assembly: InternalsVisibleTo("Rapidex.UnitTest.Base.SignalHub")]
[assembly: InternalsVisibleTo("Rapidex.UnitTest.Data")]
[assembly: InternalsVisibleTo("Rapidex.UnitTest.Base.Data")]


namespace Rapidex.SignalHub;
internal class Library : AssemblyDefinitionBase, IRapidexAssemblyDefinition
{
    public override string Name => "Signal Hub Library";
    public override string TablePrefix => "sgn";
    public override int Index => 1;

    public override void SetupMetadata(IServiceCollection services)
    {

    }

    public override void SetupServices(IServiceCollection services)
    {
        SignalHubBuilder hubBuilder = new SignalHubBuilder();
        services.AddSingletonForProd<SignalHubBuilder>(hubBuilder);
        hubBuilder.Setup(services);
    }

    public override void Start(IServiceProvider serviceProvider)
    {
        SignalHubBuilder builder = serviceProvider.GetRequiredService<SignalHubBuilder>();
        builder.Start(serviceProvider);
    }
}
