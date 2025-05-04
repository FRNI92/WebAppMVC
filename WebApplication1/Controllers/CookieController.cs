using Domain.FormModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApplication1.Controllers;

// dont put any authorize here. it must be accessable before creating and/or logging in 
public class CookieController : Controller
{

    [HttpPost]
    [Route("cookie-consent")]
    [Route("cookie/setcookies")]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {

        Response.Cookies.Append("SessionCookie", "Essential", new CookieOptions
        {
            IsEssential = true,
            Expires = DateTime.UtcNow.AddYears(1)
        });
        //dont return a view

        if (consent == null)
            return BadRequest();


        if (consent.DarkMode)
        {
            Response.Cookies.Append("DarkModeCookie", "true", new CookieOptions
            {
                IsEssential = false,
                Expires = DateTimeOffset.UtcNow.AddDays(100),
                SameSite = SameSiteMode.Lax,
                Path = "/"
            });
        }
        else
        {
            Response.Cookies.Delete("DarkModeCookie");
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
        Response.Cookies.Append("Consent", JsonSerializer.Serialize(consent), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(100),
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });
        return Ok();
    }
}
