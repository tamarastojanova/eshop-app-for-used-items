using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;

namespace eshop_app.Models
{
    public class CustomUserStore : Microsoft.AspNet.Identity.IUserStore<User>, Microsoft.AspNet.Identity.IUserPasswordStore<User>, Microsoft.AspNet.Identity.IUserEmailStore<User>
    {
        private readonly ShopEntities _dbContext;
        //private readonly UserLockoutService _userLockoutService;
        public CustomUserStore(ShopEntities dbContext)
        {
            _dbContext = dbContext;
            //_userLockoutService = userLockoutService;
        }
        public Task CreateAsync(User user)
        {
            /* _dbContext.Users.Add(user);
             try
             {
                 _dbContext.SaveChanges();
             }
             catch (DbEntityValidationException ex)
             {
                 foreach (var validationErrors in ex.EntityValidationErrors)
                 {
                     foreach (var validationError in validationErrors.ValidationErrors)
                     {
                         Console.WriteLine($"Entity of type {validationErrors.Entry.Entity.GetType().Name} " +
                                           $"in state {validationErrors.Entry.State} has validation error: " +
                                           $"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                     }
                 }
                 throw; // Re-throw the exception to maintain the original behavior
             } */
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);
            return _dbContext.SaveChangesAsync();
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return _dbContext.Users.FindAsync(userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public void Dispose()
        {
            // Dispose resources if needed
        }

        // IUserPasswordStore<TUser> methods
        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(User user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = email;
            // Save the changes to the user store (if using Entity Framework, for example)
            return UpdateAsync(user);
        }

        public Task<string> GetEmailAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //return Task.FromResult(user.EmailConfirmed);
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //user.EmailConfirmed = confirmed;
            // Save the changes to the user store
            return UpdateAsync(user);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            // Your implementation to find a user by email
            // Example: If using Entity Framework:
            return _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            // Replace the above line with the actual logic for finding a user by email
            throw new NotImplementedException();
        }
        /*
        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to retrieve lockout end date
            // For now, let's assume there's no lockout end date
            return Task.FromResult<DateTimeOffset?>(null);
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            // Implement your logic to set lockout end date
            // For now, we won't perform any action
            return Task.CompletedTask;
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to increment access failed count
            // For now, we won't perform any action
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to reset access failed count
            // For now, we won't perform any action
            return Task.CompletedTask;
        }

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to retrieve access failed count
            // For now, let's assume it's always zero
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to retrieve lockout status
            // We'll use the UserLockoutService for this
            return Task.FromResult(_userLockoutService.GetLockoutEnabled(user));
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            // Implement your logic to set lockout status
            // We'll use the UserLockoutService for this
            _userLockoutService.SetLockoutEnabled(user, enabled);
            return Task.CompletedTask;
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to retrieve user ID
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to retrieve user name
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            // Implement your logic to set user name
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to retrieve normalized user name
            return Task.FromResult(user.UserName.ToUpperInvariant());
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            // Implement your logic to set normalized user name
            // For simplicity, we'll assume normalized name is the same as user name
            user.UserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to create a new user
            // For now, we won't perform any action
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to update user information
            // For now, we won't perform any action
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            // Implement your logic to delete a user
            // For now, we won't perform any action
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            // Implement your logic to find a user by ID
            // For now, we won't perform any action
            return Task.FromResult<User>(null);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            // Implement your logic to find a user by normalized user name
            // For now, we won't perform any action
            return Task.FromResult<User>(null);
        }

        Task<Microsoft.AspNetCore.Identity.IdentityResult> Microsoft.AspNetCore.Identity.IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
        {
            return (Task<Microsoft.AspNetCore.Identity.IdentityResult>)Task.CompletedTask;
        }

        async Task<Microsoft.AspNetCore.Identity.IdentityResult> Microsoft.AspNetCore.Identity.IUserStore<User>.UpdateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                int result = await _dbContext.SaveChangesAsync(cancellationToken);

                return result > 0
                    ? Microsoft.AspNetCore.Identity.IdentityResult.Success
                    : Microsoft.AspNetCore.Identity.IdentityResult.Failed(new IdentityError { Code = "UpdateFailed", Description = "Failed to update user." });
            }
            catch (Exception ex)
            {
                return Microsoft.AspNetCore.Identity.IdentityResult.Failed(new IdentityError { Code = "Exception", Description = ex.Message });
            }
        }

        async Task<Microsoft.AspNetCore.Identity.IdentityResult> Microsoft.AspNetCore.Identity.IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext.Users.Remove(user);
                int result = await _dbContext.SaveChangesAsync(cancellationToken);

                return result > 0
                    ? Microsoft.AspNetCore.Identity.IdentityResult.Success
                    : Microsoft.AspNetCore.Identity.IdentityResult.Failed(new IdentityError { Description = "Failed to delete user." });
            }
            catch (Exception ex)
            {
                return Microsoft.AspNetCore.Identity.IdentityResult.Failed(new IdentityError { Code = "Exception", Description = ex.Message });
            }
        }
        */

    }
}