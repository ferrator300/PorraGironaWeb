using Microsoft.AspNetCore.Mvc;
using PorraGironaWeb.Models.Entity;
using Microsoft.AspNetCore.Http;
using System;
using PorraGironaWeb.Models;
using System.Linq;

namespace PorraGironaWeb.Controllers
{
    public class LoginController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        PostDbContext _context;
        public LoginController()
        {
            _context = new PostDbContext();
        }

        //Petició de la plana
        //La carregar el fitxer Startup.cs en pattern: "{controller=Login}/{action=Index}");

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //Enviament de la plana (al polsar el botó enviar)
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Penyiste penyista = null;
                try
                {
                    penyista = _context.Penyistes.FirstOrDefault(penyista => penyista.Alias.ToLower() == model.Alias.ToLower() && penyista.Password == model.Password);
                }
                catch (Exception e) { }

                if (penyista != null)
                {
                    //Guardem el alias i rol en la sessió
                    HttpContext.Session.Set("alias", System.Text.Encoding.ASCII.GetBytes(penyista.Alias));
                    HttpContext.Session.Set("rol", System.Text.Encoding.ASCII.GetBytes(penyista.Rol));
                    HttpContext.Session.Set("idpenyista", System.Text.Encoding.ASCII.GetBytes(penyista.Idpenyista.ToString()));

                    //Then redirect to the Index Action method of Controller Puntuacions
                    return RedirectToAction("Index", "Puntuacions");
                }
                else
                {
                    ModelState.AddModelError("", "Usuari o password erroni");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        private int NivellAcces()
        {
            int nivell = 10;
            String rol;
            byte[] valor = null;
            bool existeix = HttpContext.Session.TryGetValue("rol", out valor);
            if (valor != null)
            {
                rol = System.Text.Encoding.UTF8.GetString(valor);
                if (rol == "admin")
                    nivell = 0;
                if (rol == "soci")
                    nivell = 5;
            }
            return (nivell);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("alias");
            HttpContext.Session.Remove("rol");
            HttpContext.Session.Remove("idpenyista");
            return RedirectToAction("Index");
        }

    }
}
