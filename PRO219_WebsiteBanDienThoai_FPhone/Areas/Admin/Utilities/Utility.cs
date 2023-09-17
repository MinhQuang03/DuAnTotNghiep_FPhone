using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Utilities
{
    public class Utility
    {
        private IHttpContextAccessor _contextAccessor;

        public Utility(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public List<Claim> GetClaimsFromTokenInCookie(string key)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = _contextAccessor.HttpContext.Request.Cookies[key];
            if (string.IsNullOrEmpty(token))
            {
                // Handle the case where token is not available in the cookie.
                return null;
            }
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToList();
        }
    }
}
