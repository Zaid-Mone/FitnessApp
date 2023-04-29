using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Models;
using FitnessApp.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IWebHostEnvironment _hosting;
        public IndexModel(
            UserManager<Person> userManager,
            SignInManager<Person> signInManager, IWebHostEnvironment hosting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hosting = hosting;
        }

        public string Username { get; set; }
        public string ImageUrl { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string ImageUrl { get; set; }
        }

        private async Task LoadAsync(Person user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var getUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ImageUrl = getUser.UserAvatar
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            TempData["UserImage"] = user.UserAvatar;
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

             await CheckUserRole("");
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            UploadImage(user);
            var res = await _userManager.UpdateAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();


            
        }
        



        public async Task CheckUserRole(string image)
        {
            if (User.IsInRole(Roles.Admin))
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                user.UserAvatar = image;
                UploadImage(user);
                await _userManager.UpdateAsync(user);
            }
            else if (User.IsInRole(Roles.Trainer))
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                user.UserAvatar = image;
                UploadImage(user);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                user.UserAvatar = image;
                UploadImage(user);
                await _userManager.UpdateAsync(user);
            }
        }
        void UploadImage(Person model)
        {
            var file = HttpContext.Request.Form.Files;
            if (file.Count() > 0)
            {//@"wwwroot/"
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var filestream = new FileStream(Path.Combine(_hosting.WebRootPath, "Images", ImageName), FileMode.Create);
                file[0].CopyTo(filestream);
                model.UserAvatar = ImageName;
            }
            else if (model.UserAvatar == null)
            {
                var username = model.Email.Substring(0, model.Email.IndexOf("@"));
                var name = $"https://ui-avatars.com/api/?name={username}&bold=true";
                model.UserAvatar = name;
            }
            else
            {
                model.UserAvatar = model.UserAvatar;
            }
        }



    }
}
