using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using vikasAppCrud.DB_Context;
using vikasAppCrud.Models;

namespace vikasAppCrud.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public object FormsAuthentication { get; private set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            vikasContext obj = new vikasContext();
            var res = obj.VikTables.ToList();
            List<VikasModel> vm = new List<VikasModel>();
            foreach (var item in res)
            {
                vm.Add(new VikasModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Mobileno = item.Mobileno,
                    Salary = item.Salary
                });
            }

            return View(vm);
        }


        [HttpGet]
        public IActionResult AddData()
        {


            return View();
        }

        [HttpPost]
        public IActionResult AddData(VikasModel vm)
        {
            vikasContext obj = new vikasContext();

            VikTable vt = new VikTable();

            vt.Id = vm.Id;
            vt.Name = vm.Name;
            vt.Email = vm.Email;
            vt.Mobileno = vm.Mobileno;
            vt.Salary = vm.Salary;

            if (vm.Id == 0)
            {
                obj.VikTables.Add(vt);
                obj.SaveChanges();
            }

            else
            {
                obj.Entry(vt).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                obj.SaveChanges();
            }
             


            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            VikasModel vtt = new VikasModel();
            vikasContext obj = new vikasContext();
            var editi = obj.VikTables.Where(x => x.Id == id).First();
            vtt.Id = editi.Id;
            vtt.Name = editi.Name;
            vtt.Email = editi.Email;
            vtt.Mobileno = editi.Mobileno;
            vtt.Salary = editi.Salary;

            return View("AddData", vtt);
        }
        public IActionResult Delete(int id)
        {

            vikasContext obj = new vikasContext();
            var dele = obj.VikTables.Where(x => x.Id == id).First();
            obj.VikTables.Remove(dele);
            obj.SaveChanges();

            return RedirectToAction("Index", "home");
        }


        public IActionResult Login(UserModel lm)
        {
            vikasContext obj = new vikasContext();
            var res = obj.LoginData.Where(m => m.Email == lm.Email).FirstOrDefault();
            if(res==null)
            {
                TempData["invalid"] = "email is not found";
            }
            else
            {
                if(res.Email==lm.Email&&res.Password==lm.Password)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, res.Name),
                                        new Claim(ClaimTypes.Email, res.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);


                    HttpContext.Session.SetString("Name",lm.Email);

                    return RedirectToAction("Index","Home");

                }

                else
                {

                    ViewBag.Inv = "Wrong Email Id or password";

                    return View("Login");
                }


            }

            return View();
        }
    



            
         


        public IActionResult Logout()
        {


            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            return RedirectToAction("Login","Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
