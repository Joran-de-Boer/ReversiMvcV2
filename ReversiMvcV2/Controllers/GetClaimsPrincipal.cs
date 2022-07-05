using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ReversiMvcV2.Controllers
{
    public class GetClaimsPrincipal : Controller, IDisposable
    {
        public string? GetUserId(ClaimsPrincipal currentUser)
        {
            return currentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
