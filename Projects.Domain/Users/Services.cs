using System.Threading.Tasks;

namespace Projects.Domain.Users {
    public delegate Task<bool> IsUserValid(UserId userId);
}