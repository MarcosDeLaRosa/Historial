using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HistorialMedico.Models;

namespace HistorialMedico.Controllers
{
    public class DoctorController : Controller
    {

        private HistorialMedicoContext db = new HistorialMedicoContext();

        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View(); 
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "id_doctor,nombre,apellido,telefono,correo,foto")]doctor doctor)
        {

            if (ModelState.IsValid) {
                db.doctor.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctor);

        }

        public ActionResult Edit()
        {

            return View();
        }


    }
}