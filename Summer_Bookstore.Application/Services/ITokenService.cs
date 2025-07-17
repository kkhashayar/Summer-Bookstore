

using Summer_Bookstore_Domain.Entities;

namespace Summer_Bookstore.Application.Services;

public interface ITokenService
{
    string CreateToken(User user);
}
