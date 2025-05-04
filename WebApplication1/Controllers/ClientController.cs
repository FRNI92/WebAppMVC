using Business.Services;
using Domain.Dtos;
using Domain.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{

    [Authorize]
    public class ClientController(ClientService clientService) : Controller
    {
        private readonly ClientService _clientService = clientService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetAllClientsAsync();
            var cvm = new ClientViewModel
            {
                Clients = clients,
                FormModel = new ClientFormModel()
            };
            return View(cvm);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Add(ClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                var clients = await _clientService.GetAllClientsAsync();
                var cvm = new ClientViewModel
                {
                    Clients = clients,
                    FormModel = new ClientFormModel() // tom modell
                };
                return View("Index", cvm);
            }

          await _clientService.AddClientAsync(dto);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Update(ClientFormModel form)
        {

            if (!ModelState.IsValid)
            {
                var clients = await _clientService.GetAllClientsAsync();
                var cvm = new ClientViewModel
                {
                    Clients = clients,
                    FormModel = form
                };
                return View("Index", cvm);
            }

            var dto = new ClientDto
            {
                Id = form.Id,
                ClientName = form.ClientName
            };

            await _clientService.UpdateClientAsync(dto);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteClientAsync(id);
            return RedirectToAction("Index");
        }
    }
}
