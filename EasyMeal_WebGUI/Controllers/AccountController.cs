using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EasyMeal_WebGUI.Models.ViewModels;
using EasyMeal_Core.DomainServices;
using EasyMeal_Core.DomainModel;

namespace EasyMeal_WebGUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public readonly ICustomerRepository customerRepository;
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public AccountController(ICustomerRepository customerRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.customerRepository = customerRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(customerRepository.Customers);
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                return View(new LoginViewModel { ReturnUrl = returnUrl });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginViewModel.Username);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginViewModel
                        .Password, false, false)).Succeeded)
                    {
                        return Redirect(loginViewModel?.ReturnUrl ?? "/");
                    }
                }
            }

            ModelState.AddModelError("", "Onjuiste gebruikersnaam of wachtwoord");
            return View(loginViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                return View(new RegisterViewModel { ReturnUrl = returnUrl });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    StreetName = registerViewModel.StreetName,
                    HouseNumber = registerViewModel.HouseNumber,
                    City = registerViewModel.City,
                    PostalCode = registerViewModel.PostalCode,
                    PhoneNumber = registerViewModel.PhoneNumber,
                    Email = registerViewModel.Email
                };

                customerRepository.AddCustomer(customer);

                var identityUser = new IdentityUser { UserName = registerViewModel.Email };
                var result = await userManager.CreateAsync(identityUser, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(identityUser, false);
                    return Redirect("/");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
