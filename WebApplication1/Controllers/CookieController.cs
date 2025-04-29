using Domain.FormModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplication1.Controllers;

public class CookieController : Controller
{

    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {
        //dont return a view

        if (consent == null)
            return BadRequest();


        if (consent.DarkMode)
        {
            Response.Cookies.Append("DarkMode", "true", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(100),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Append("DarkMode", "false", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(100),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }




        if (consent.Functional)
        {
            Response.Cookies.Append("FunctionalCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("FunctionalCookie");
        }

        if (consent.Analytics)
        {
            Response.Cookies.Append("AnalyticsCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("AnalyticsCookie");
        }

        if (consent.Marketing)
        {
            Response.Cookies.Append("MarketingCookie", "Non-Essential", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("MarketingCookie");
        }
        Response.Cookies.Append("cookieConsent", JsonSerializer.Serialize(consent), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(100),
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });
        return Ok();
    }
}
