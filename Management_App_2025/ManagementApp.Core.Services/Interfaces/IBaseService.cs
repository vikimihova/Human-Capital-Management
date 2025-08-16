namespace ManagementApp.Core.Services.Interfaces
{
    public interface IBaseService
    {
        bool IsGuidValid(string? id, ref Guid parsedGuid);
    }
}
