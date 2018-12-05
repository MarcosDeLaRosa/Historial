﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HistorialMedico.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

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

        [HttpPost]
        public ActionResult AsignarCita([Bind(Include = "id_pasiente,id_doctor,fechaDeConsulta,fechaDeCita,hora,duracion")]cita cita)
        {

            if (ModelState.IsValid)
            {
                db.cita.Add(cita);
                db.SaveChanges();
                return RedirectToAction("AsignarCita", "Asistente");

            }
            return View(cita);
        }

        [HttpGet]
        public ActionResult AsignarCita()
        {
            return View();
        }

        public ActionResult ConsultarCalendarioCita()
        {
            return View(db.cita.ToList());
        }

        public ActionResult Cumpleaños()
        {
            return View(db.paciente.ToList());
        }

        public ActionResult PagoConsulta()
        {
            return View();
        }

        public ActionResult VerificarCedula()
        {


            return View();
        }

        [HttpPost]

        public ActionResult VerificarCedula(string cedulaP)
        {
            paciente paciente = db.paciente.Where(e => e.cedula == cedulaP).FirstOrDefault();

            if (paciente != null)
            {
                return PacienteEncontrado(cedulaP);
            }
            else
            {
                return View("CrearPaciente");
            }


            return View();
        }

        public ActionResult PacienteEncontrado(string cedula)
        {
            paciente paciente = db.paciente.Where(e => e.cedula == cedula).FirstOrDefault();
            List<paciente> pacientes = new List<paciente>();
            pacientes.Add(paciente);
            return View("PacienteEncontrado", pacientes.ToList());
        }


        public ActionResult PagoDeConsulta()
        {
            var getname = db.paciente.ToList();
            SelectList list = new SelectList(getname, "id_paciente", "nombre");
            ViewBag.NombrePaciente = list;
            var getnameDoctor = db.precioConsulta.ToList();
            SelectList lista = new SelectList(getnameDoctor, "id_precioConsulta", "costo");
            ViewBag.NombreDoctor = lista;
            return View();
        }

        [HttpPost]

        public ActionResult PagoDeConsulta([Bind(Include = "id_Paciente,id_PrecioConsulta,pago,total,fecha")] pagoConsulta pagoConsulta)
        {
            if (ModelState.IsValid)
            {
                db.pagoConsulta.Add(pagoConsulta);
                db.SaveChanges();

            }
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                pdfDoc.Open();
                string cadenaFinal = "";
                paciente asistente = db.paciente.Find(pagoConsulta.id_Paciente);
                precioConsulta precioConsulta = db.precioConsulta.Find(pagoConsulta.id_PrecioConsulta);
                cadenaFinal += "<br><center><h2>Factura</h2></center><br><br><div>" +
                    "<h4> Cedula:  " + asistente.cedula + "</h4><br>" +
                    "<h4> Nombre:  " + asistente.nombre + "</h4><br>" +
                    "<h4> Telefono:  " + asistente.telefono + "</h4><br>" +
                    "<h4> Telefono:  " + asistente.correo + "</h4><br>" +
                    "<h4> Costo:  " + precioConsulta.costo + "</h4><br>" +
                    "<h4> Pago:  " + pagoConsulta.pago + "</h4><br>" +
                    "<h4> Total:  " + pagoConsulta.total + "</h4><br>" +
                    "<h4> Fecha: " + pagoConsulta.fecha + "</h4>" + "</div>";

                string strContent = cadenaFinal;

                var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(strContent), null);

                foreach (var htmlElement in parsedHtmlElements)
                {
                    pdfDoc.Add(htmlElement as IElement);
                }

                pdfDoc.Close();
                Response.ContentType = "application/pdf";

                Response.AddHeader("content-disposition", "attachment; filename=Receta.pdf");
                System.Web.HttpContext.Current.Response.Write(pdfDoc);
                Response.Flush();
                Response.End();

            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

            return View(pagoConsulta);

        }

    }

}
