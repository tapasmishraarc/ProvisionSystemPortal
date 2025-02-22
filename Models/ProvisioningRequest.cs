namespace SystemProvisioningPortal.Models;

public class ProvisioningRequest
{
    public required List<string> Systems { get; set; }
    public required string Version { get; set; }
    public required string Action { get; set; }
}

public class System
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}

public class SystemVersion
{
    public required string Id { get; set; }
    public required string Version { get; set; }
}