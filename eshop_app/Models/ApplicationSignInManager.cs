using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace eshop_app.Models
{
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(UserManager<User, string> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            // Customize the creation of the user's identity, including claims
            var userIdentity = base.CreateUserIdentityAsync(user);

            // Add custom claims if needed
            // userIdentity.AddClaim(new Claim("custom_claim_type", "custom_claim_value"));

            return userIdentity;
        }
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            var manager = new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
            // Configure manager settings...
            return manager;
        }
        public virtual async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {

            if (false)
            {
                return SignInStatus.LockedOut;
            }

            var user = await UserManager.FindByEmailAsync(userName).ConfigureAwait(false);

            if (user != null && await UserManager.CheckPasswordAsync(user, password).ConfigureAwait(false))
            {
                // Your custom logic for successful sign-in
                // ...

                await SignInAsync(user, isPersistent, shouldLockout).ConfigureAwait(false);

                return SignInStatus.Success;
            }

            // Other logic...

            return SignInStatus.Failure;
        }
    }
}