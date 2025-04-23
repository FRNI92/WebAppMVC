// Under ~/ViewComponents/ProfileViewComponent.cs
using Business.Services;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class MemberProfileViewComponent : ViewComponent
{
    private readonly UserManager<AppUserEntity> _userManager;
    private readonly MemberService _memberService;

    public MemberProfileViewComponent(
        UserManager<AppUserEntity> userManager,
        MemberService memberService)
    {
        _userManager = userManager;
        _memberService = memberService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user?.MemberId != null)
        {
            var member = await _memberService.GetByIdAsync(user.MemberId.Value);
            return View(member);
        }
        return View(null);
    }
}