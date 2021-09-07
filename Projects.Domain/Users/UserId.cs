using System;
using System.Threading.Tasks;
using Eventuous;

namespace Projects.Domain.Users {
    public record UserId {
        public string UserIdString { get; }

        internal UserId(string value) => UserIdString = Ensure.NotEmptyString(value, nameof(value));

        public static async Task<UserId> FromString(string userId, IsUserValid isUserValid) {
            var userExists = await isUserValid(new UserId(userId));

            return !userExists ? throw new InvalidOperationException("User does not exist") : new UserId(userId);
        }
    }
}