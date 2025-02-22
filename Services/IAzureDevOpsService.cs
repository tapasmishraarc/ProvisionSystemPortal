namespace SystemProvisioningPortal.Services;

public interface IAzureDevOpsService
{
    Task<object> TriggerProvisioningPipelineAsync(Models.ProvisioningRequest request);
}