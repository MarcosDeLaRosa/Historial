using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HistorialMedico.Models;

namespace HistorialMedico.Controllers
{
    public class LoginController : Controller
    {

        private HistorialMedicoContext db = new HistorialMedicoContext();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string user, string password)
        {

            if (user == null || password == null)
            {
                return RedirectToAction("Error", "Login");
            }

            if (user != null && password != null)
            {

                usuario usuario = db.usuario.Where(e => e.usuario1 == user).FirstOrDefault();
                string resultado = string.Empty;
                Byte[] desencriptar = Convert.FromBase64String(usuario.contrasena);

                resultado = System.Text.Encoding.Unicode.GetString(desencriptar);
                if (usuario.usuario1 == user && resultado == password)
                {
                    if (usuario.roles == "doctor")
                    {
                        return RedirectToAction("Index", "Doctor");
                    }
                    else if (usuario.roles == "asistente")
                    {
                        return RedirectToAction("Index", "Asistente");
                    }
                }
            }
            else
            {
                return View("Error");
            }

            return View();
        }

        public ActionResult Error()
        {

            return View("Login");
        }

    }
}
