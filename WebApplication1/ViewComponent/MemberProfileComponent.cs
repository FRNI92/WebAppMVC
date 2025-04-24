// Under ~/ViewComponents/ProfileViewComponent.cs
using Business.Services;
using Domain.Dtos;
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
    //get memberservice and userservice to check whom is logged in
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (HttpContext?.User == null)
            return View(new MemberDto { FirstName = "Not logged in", LastName = "", Image = "/Images/Avatar.svg" });

        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user?.MemberId != null)
        {
            var member = await _memberService.GetByIdAsync(user.MemberId.Value);
            return View(member);
        }
        return View(new MemberDto { FirstName = "logged in not member - Guest", LastName = "", Image = "/Images/Avatar.svg" });
    }
}
