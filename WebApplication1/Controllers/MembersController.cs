using Business.Services;
using Database.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Domain.FormModels;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class MembersController(MemberService memberService, IWebHostEnvironment env, AddressService addressService, UserManager<AppUserEntity> userManager, NotificationService notificationService) : Controller
    {

        private readonly IWebHostEnvironment _env = env;
        private readonly UserManager<AppUserEntity> _userManager = userManager;

        private readonly AddressService _addressService = addressService;
        private readonly NotificationService _notificationService = notificationService;
        public async Task<IActionResult> Members()
        {

            // Get all appusers
            var users = await _userManager.Users.ToListAsync();

            // Get all members
            var members = await memberService.GetAllMembersAsync()
                          ?? new List<MemberDto>();

            var model = new MemberViewModel
            {
                MemberCards = new List<MemberCardViewModel>(),
                AllUsers = users
            };

            foreach (var member in members)
            {
                var address = await _addressService
                                  .GetByIdAsync(member.AddressId)
                                  .ConfigureAwait(false)
                              ?? new AddressDto();

                // find connected appuser
                var linkedUser = users.FirstOrDefault(u => u.MemberId == member.Id);

                model.MemberCards.Add(new MemberCardViewModel
                {
                    Member = member,
                    Address = address,
                    ConnectedAppUserId = linkedUser?.Id   // här sätter vi värdet
                });
            }

            // get current member for logged in user
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser?.MemberId != null)
            {
                var cm = await memberService.GetByIdAsync(appUser.MemberId.Value);
                if (cm != null)
                    model.CurrentMember = cm;
            }

            return View(model);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Add(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ShowAddMemberModal"] = true;
                // Återvänd till Members-sidan och visa Add-member modal
                return View("Members", model);  // För att skicka tillbaka till Members-sidan med model och fel
            }
            TempData["ShowAddMemberModal"] = null;

            // use this to see what fields are failing
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new
            //    {
            //        errors = ModelState.ToDictionary(
            //            x => x.Key,
            //            x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //            )
            //    });
            //}

            if (model.FormModel.ImageFile != null && model.FormModel.ImageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder); // Skapa om den inte finns

                var originalName = Path.GetFileName(model.FormModel.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FormModel.ImageFile.CopyToAsync(stream);
                }

                model.FormModel.Image = fileName;
            }
            var addressDto = new AddressDto
            {
                StreetName = model.FormModel.Address.StreetName,
                StreetNumber = model.FormModel.Address.StreetNumber,
                PostalCode = model.FormModel.Address.PostalCode,
                City = model.FormModel.Address.City
            };

            var memberDto = new MemberDto
            {
                Image = model.FormModel.Image,
                FirstName = model.FormModel.FirstName,
                LastName = model.FormModel.LastName,
                Email = model.FormModel.Email,
                Phone = model.FormModel.Phone,
                JobTitle = model.FormModel.JobTitle,
                DateOfBirth = model.FormModel.DateOfBirth,
                AddressId = 0 // is set later when address is saved
            };

            //try adding signalR when member is created
            var notificationEntity = new NotificationEntity
            {
                Message = $"New member {memberDto.FirstName} {memberDto.LastName} was created",
                NotificationTypeId = 1, // User
                TargetGroupId = 2 // Admins
            };

            await _notificationService.AddNotificationAsync(notificationEntity);



            var addressId = await _addressService.CreateAsync(addressDto);
            memberDto.AddressId = addressId;
            await memberService.CreateMemberAsync(memberDto);
            return RedirectToAction("Members", "Members");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Edit(MemberViewModel model)
        
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Members");

            if (model.FormModel.ImageFile != null && model.FormModel.ImageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.FormModel.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadFolder, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await model.FormModel.ImageFile.CopyToAsync(stream);
                model.FormModel.Image = fileName;
            }

            // get an update address
            var member = await memberService.GetByIdAsync(model.FormModel.Id);
            if (member == null) return NotFound();
            var address = await _addressService.GetByIdAsync(member.AddressId);
            if (address == null) return NotFound();

            address.StreetName = model.FormModel.Address.StreetName;
            address.StreetNumber = model.FormModel.Address.StreetNumber;
            address.PostalCode = model.FormModel.Address.PostalCode;
            address.City = model.FormModel.Address.City;
            var addrRes = await _addressService.UpdateAsync(address);
            if (!addrRes.Succeeded)
            {
                TempData["Error"] = "Failed to update address.";
                return RedirectToAction("Members");
            }

            // update member fields
            member.Image = model.FormModel.Image;
            member.FirstName = model.FormModel.FirstName;
            member.LastName = model.FormModel.LastName;
            member.Email = model.FormModel.Email;
            member.Phone = model.FormModel.Phone;
            member.JobTitle = model.FormModel.JobTitle;
            member.DateOfBirth = model.FormModel.DateOfBirth;
            member.AddressId = addrRes.Result.Id;
            var updateRes = await memberService.UpdateMemberAsync(member);
            if (!updateRes.Succeeded)
            {
                TempData["Error"] = "Failed to update member.";
                return RedirectToAction("Members");
            }

            //connect appuser via dropdown. remove possible old user
            var oldUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.MemberId == member.Id);
            if (oldUser != null && oldUser.Id != model.FormModel.ConnectedAppUserId)
            {
                oldUser.MemberId = null;
                await _userManager.UpdateAsync(oldUser);
            }
            // set the new connection if chosen
            if (!string.IsNullOrEmpty(model.FormModel.ConnectedAppUserId))
            {
                var newUser = await _userManager.FindByIdAsync(model.FormModel.ConnectedAppUserId);
                if (newUser != null)
                {
                    newUser.MemberId = member.Id;
                    await _userManager.UpdateAsync(newUser);
                }
            }

            return RedirectToAction("Members");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await memberService.DeleteMemberAsync(id);

            if (!result.Succeeded)
            {
                TempData["Error"] = result.Error;
                return RedirectToAction("Members");
            }

            return RedirectToAction("Members");
        }
    }
}
