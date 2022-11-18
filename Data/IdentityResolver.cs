using AutoMapper;
using Microsoft.IdentityModel.JsonWebTokens;
using SupportAPI.Auth.Model;
using System.Security.Claims;

namespace SupportAPI.Data
{
    public class IdentityResolver : IValueResolver<object, object, string>
    {
        private IHttpContextAccessor _httpContextAccessor;

        public IdentityResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            destMember = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return destMember;
        }
    }
}
