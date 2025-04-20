using Business.Services;
using Domain.Dtos;
using Domain.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class MembersController(MemberService memberService, IWebHostEnvironment env) : Controller
    {
        public async Task <IActionResult> Members()
        {

            var members = await memberService.GetAllMembersAsync();

            var model = new MemberViewModel
            {
                FormModel = new MemberFormModel(),
                MemberList = members.ToList()
            };

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
            await memberService.CreateMemberAsync(memberDto, addressDto);
            return RedirectToAction("Members", "Members");
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
