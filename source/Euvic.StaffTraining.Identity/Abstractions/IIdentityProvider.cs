namespace Euvic.StaffTraining.Identity.Abstractions
{
    public interface IIdentityProvider
    {
        Task<long> CreateUserAsync(string email, string password);
    }
}
