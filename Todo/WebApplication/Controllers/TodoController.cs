using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Todo;
using WebApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserId();
            var todos = _repository.GetActive(userId);
            return View(todos);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(new TodoItem(model.Text, await GetCurrentUserId()));
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Completed()
        {
            var userId = await GetCurrentUserId();
            var completed = _repository.GetCompleted(userId);
            return View(completed);
        }

        private async Task<Guid> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return new Guid(user.Id);
        }


        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            var userId = await GetCurrentUserId();
            _repository.MarkAsCompleted(todoId, userId);
            return RedirectToAction("Index");
        }

    }
}