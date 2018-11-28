using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HistorialMedico.Models;

namespace HistorialMedico.Controllers
{
    public class AsistenteController : Controller
    {
        private HistorialMedicoContext db = new HistorialMedicoContext();

        // GET: Asistente
        public ActionResult Index()
        {
            return View(db.asistente.ToList());
        }

        // GET: Asistente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asistente asistente = db.asistente.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // GET: Asistente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Asistente/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_asistente,nombre,apellido,telefono,correo,foto")] asistente asistente)
        {
            if (ModelState.IsValid)
            {
                db.asistente.Add(asistente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(asistente);
        }

        // GET: Asistente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asistente asistente = db.asistente.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistente/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_asistente,nombre,apellido,telefono,correo,foto")] asistente asistente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asistente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asistente);
        }

        // GET: Asistente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asistente asistente = db.asistente.Find(id);
            if (asistente == null)
            {
                return HttpNotFound();
            }
            return View(asistente);
        }

        // POST: Asistente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            asistente asistente = db.asistente.Find(id);
            db.asistente.Remove(asistente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CrearPaciente()
        {
            return View();
        }
        
    }
}
