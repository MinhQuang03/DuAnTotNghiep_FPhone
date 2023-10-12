using System.Security.Claims;

namespace PRO219_WebsiteBanDienThoai_FPhone.Areas.Admin.Filters;

public static class UserClaim
{
    public static bool HasRole(ClaimsPrincipal User, string role)
    {
        if (string.IsNullOrEmpty(role)) return false;

        if (User.Identity.IsAuthenticated && User.HasClaim("role", role)) return true;

        return false;
    }
}