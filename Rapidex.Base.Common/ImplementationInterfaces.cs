using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Rapidex;

//TODO: Not possible for this level, must remove and use another approach.
public interface IModuleNavigationInfo
{
    string Tenant { get; }
    string Workspace { get; }
    string Module { get; }
}

public interface IImplementHost
{
    IModuleNavigationInfo ModuleNavigationInfo { get; }
}

//See: JsonImplementerInterfaceConverter
//[JsonConverter(typeof(JsonImplementerInterfaceConverter))]
[JsonDerivedBase]
public interface IImplementer
{
    string[] SupportedTags { get; }
    bool Implemented { get; }
    IUpdateResult Implement(IImplementHost host, IImplementer parentImplementer, ref object target);
}



public interface IImplementer<T> : IImplementer where T : IImplementTarget
{
    void Implement(IImplementTarget parentModule, ref T target)
    {
        this.Implement(parentModule, ref target);
    }
}



public interface IImplementTargetEnumerable<T> : IList<T> where T : IImplementTarget
{

}

public interface IImplementTarget
{

}
