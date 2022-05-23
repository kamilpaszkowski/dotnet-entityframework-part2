using Euvic.StaffTraining.Identity.Abstractions;
using Euvic.StaffTraining.Identity.Infrastructure.EntityFramework;

namespace Euvic.StaffTraining.Identity.Infrastructure.Identity
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IdentityContext _identityContext;

        public IdentityProvider(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<long> CreateUserAsync(string email, string password)
        {
            var user = new Entities.User()
            {
                Email = email,
                Password = password
            };

            _identityContext.Users.Add(user);
            await _identityContext.SaveChangesAsync();

            return user.Id;
        }
    }
}
