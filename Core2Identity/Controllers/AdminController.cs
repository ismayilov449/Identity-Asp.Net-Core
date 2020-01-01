using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core2Identity.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private IPasswordHasher<ApplicationUser> passwordHasher;


        public AdminController(UserManager<ApplicationUser> _userManager, IPasswordValidator<ApplicationUser> _passwordValidator, IPasswordHasher<ApplicationUser> _passwordHasher)
        {
            userManager = _userManager;
            passwordValidator = _passwordValidator;
            passwordHasher = _passwordHasher;
        }


        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email

                };

                var result = await userManager.CreateAsync(applicationUser, model.Password);


                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var crntuser = await userManager.FindByIdAsync(Id);

            if (crntuser != null)
            {
                var result = await userManager.DeleteAsync(crntuser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }


            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            return View("Index", userManager.Users);

        }


        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);


            if (user != null)
            {

                return View(user);

            }
            else
            {
                return RedirectToAction("Index");
            }


        }



        [HttpPost]
        public async Task<IActionResult> Update(string Id, string Password, string Email)
        {
            var user = await userManager.FindByIdAsync(Id);


            if (user != null)
            {

                user.Email = Email;

                IdentityResult validPass = null;

                if (!String.IsNullOrEmpty(Password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, Password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, Password);
                    }
                    else
                    {
                        foreach (var error in validPass.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Errroror");
                }

                if (validPass.Succeeded)
                {
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in validPass.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }



            }
            else
            {
                ModelState.AddModelError("", "User not found");
            }

            return View(user);
        }
    }
}