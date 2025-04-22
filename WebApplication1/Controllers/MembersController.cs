using Business.Services;
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
    public class MembersController(MemberService memberService, IWebHostEnvironment env, AddressService addressService, UserManager<AppUserEntity> userManager) : Controller
    {

        private readonly IWebHostEnvironment _env = env;
        private readonly UserManager<AppUserEntity> _userManager = userManager;

        private readonly AddressService _addressService = addressService;
        public async Task<IActionResult> Members()
        {
            var users = await _userManager.Users.ToListAsync();
            var members = await memberService.GetAllMembersAsync();

            var model = new MemberViewModel
            {
                MemberCards = new List<MemberCardViewModel>(),
                AllUsers = users // 👈 Här får dropdownen sitt innehåll
            };

            foreach (var member in members)
            {
                var address = await _addressService.GetByIdAsync(member.AddressId) ?? new AddressDto();

                model.MemberCards.Add(new MemberCardViewModel
                {
                    Member = member,
                    Address = address
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Add(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    errors = ModelState.ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }
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
                AddressId = 0 // sätts senare i servicen efter addressen sparas
            };

            var addressId = await _addressService.CreateAsync(addressDto);
            memberDto.AddressId = addressId;
            await memberService.CreateMemberAsync(memberDto);
            return RedirectToAction("Members", "Members");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(MemberViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Members");

            // 🖼 Hantera ny bild
            if (model.FormModel.ImageFile != null && model.FormModel.ImageFile.Length > 0)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadFolder);

                var originalName = Path.GetFileName(model.FormModel.ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FormModel.ImageFile.CopyToAsync(stream);
                }

                model.FormModel.Image = fileName;  // Uppdatera FormModel.Image med det nya filnamnet
            }

            // Hämta medlem baserat på det ID som skickades i modellen
            var member = await memberService.GetByIdAsync(model.FormModel.Id);
            if (member == null)
            {
                return NotFound();
            }

            // Hämta adressen som är kopplad till medlemmen
            var address = await _addressService.GetByIdAsync(member.AddressId);
            if (address == null)
            {
                return NotFound();
            }

            // Uppdatera adressen
            var updatedAddress = new AddressDto
            {
                Id = address.Id,  // Använd det befintliga AddressId
                StreetName = model.FormModel.Address.StreetName,
                StreetNumber = model.FormModel.Address.StreetNumber,
                PostalCode = model.FormModel.Address.PostalCode,
                City = model.FormModel.Address.City
            };

            // Uppdatera adressen
            var addressUpdateResult = await _addressService.UpdateAsync(updatedAddress);

            // Kontrollera om adressen uppdaterades korrekt
            if (!addressUpdateResult.Succeeded)
            {
                TempData["Error"] = "Failed to update address.";
                return RedirectToAction("Members");
            }

            // Uppdatera medlemmen med det uppdaterade AddressId
            member.Image = model.FormModel.Image;
            member.FirstName = model.FormModel.FirstName;
            member.LastName = model.FormModel.LastName;
            member.Email = model.FormModel.Email;
            member.Phone = model.FormModel.Phone;
            member.JobTitle = model.FormModel.JobTitle;
            member.DateOfBirth = model.FormModel.DateOfBirth;
            member.AddressId = addressUpdateResult.Result.Id;  // Uppdatera AddressId med det uppdaterade ID:t

            // Uppdatera medlemmen
            var updateResult = await memberService.UpdateMemberAsync(member);
            if (!updateResult.Succeeded)
            {
                TempData["Error"] = "Failed to update member.";
                return RedirectToAction("Members");
            }

            return RedirectToAction("Members");
        }


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
