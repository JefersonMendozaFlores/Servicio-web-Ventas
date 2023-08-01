using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels.User;
using System.Data;

namespace ShoppingCart.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class UsuariosController : Controller
    {
        private readonly DataContext _context;
        private UserManager<AppUser> _userManager;

        public UsuariosController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var query = (from user in _context.Users
                         join user_role in _context.UserRoles
                         on user.Id equals user_role.UserId into user_role_list
                         from user_role_null in user_role_list.DefaultIfEmpty()
                         join role in _context.Roles
                         on user_role_null.RoleId equals role.Id into role_list
                         from role_null in role_list.DefaultIfEmpty()
                         select new
                         {
                             Id = user.Id,
                             Username = user.UserName,
                             Email = user.Email,
                             Role = role_null.Name
                         }).ToList();


            List<ListUserViewModel> users = query.Select(user => new ListUserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            }).ToList();

            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel user)
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");

            if (ModelState.IsValid)
            {
                AppUser newUser = new AppUser { UserName = user.UserName, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(newUser, user.RoleName);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = $"The user {newUser.UserName} has been created!";
                        return Redirect("Index");//Retorna a pagina principal
                    }
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(user);
        }

        // GET: User/Edit/1
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);

            EditUserViewModel editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                OldRoleName = roles.First(),
                NewRoleName = roles.First()
            };

            return View(editUserViewModel);
        }

        // POST: User/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel updatedUser)
        {
            ViewBag.Roles = new SelectList(_context.Roles, "Name", "Name");
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Actualizar las propiedades del usuario existente con las propiedades del usuario actualizado
                user.Email = updatedUser.Email;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.RemoveFromRoleAsync(user, updatedUser.OldRoleName);
                    result = await _userManager.AddToRoleAsync(user, updatedUser.NewRoleName);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = $"The user {user.UserName} has been updated!";
                        return RedirectToAction("Index", "Usuarios", new { area = "Admin" });
                    }
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // Si el modelo no es válido, puedes volver a mostrar el formulario con los mensajes de error.
            return View(updatedUser);
        }

        public async Task<IActionResult> Delete(string id)
        {
            //User user = userList.Find(u => u.Id == id);
            AppUser user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"The user {user.UserName} has been deleted!";

            return RedirectToAction("Index");
        }

    }
}
