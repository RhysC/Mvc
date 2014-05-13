using System;

namespace CodeConverters.Core.Persistence
{
    //TODO - change this to a method call that must be called explicitly via the interface and have the props as get only
    public interface IAuditable
    {
        DateTimeOffset CreatedOn { get; set; }
        DateTimeOffset ModifiedOn { get; set; }
    }
}
