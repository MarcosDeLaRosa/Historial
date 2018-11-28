using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HistorialMedico.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

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

            if (ModelState.IsValid)
            {
                db.doctor.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctor);

        }

        [HttpGet]
        public ActionResult CreateVisita()
        {

            return View();
        }


        [HttpGet]

        public ActionResult CrearVisita()
        {
            var getname = db.paciente.ToList();
            SelectList list = new SelectList(getname, "id_paciente", "nombre");
            ViewBag.NombrePaciente = list;
            var getnameDoctor = db.doctor.ToList();
            SelectList lista = new SelectList(getnameDoctor, "id_doctor", "nombre");
            ViewBag.NombreDoctor = lista;
            return View();
        }

        [HttpPost]
        public ActionResult CreateVisita([Bind(Include = "id_paciente,id_doctor,motivo,comentario,recetaDeMedicmentos,fechaProximaVisita")]visita visita)
        {
            if (ModelState.IsValid)
            {
                db.visita.Add(visita);
                db.SaveChanges();
                return RedirectToAction("CrearVisita", "Doctor");
            }

            return View(visita);
        }

        public ActionResult visitaPaciente()
        {
            var getname = db.paciente.ToList();
            SelectList list = new SelectList(getname, "id_paciente", "nombre");
            ViewBag.NombrePaciente = list;
            return View(db.visita.ToList());
        }
        [HttpGet]
        public ActionResult ImprimirReceta()
        {
            return View(db.visita.ToList());
        }


        [HttpGet]
        public ActionResult ImprimirRecetas(int id)
        {
            visita visita = db.visita.Find(id);
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                pdfDoc.Open();
                string cadenaFinal = "";
                cadenaFinal += "<br><center><h2>Historial Medico</h2></center><br><br><div>" +
                    "<h4> Cedula:  " + visita.paciente.cedula + "</h4><br>" +
                    "<h4> Nombre: " + visita.paciente.nombre + "</h4><br>" +
                    "<h4> Correo:  " + visita.paciente.correo + "</h4><br>" +
                    "<h4> Telefono: " + visita.paciente.telefono + "</h4><br>" +
                    "<h4> Tipo de Sangre: " + visita.paciente.tipodeSangre + "</h4><br>" +
                    "<h4> Receta: " + visita.recetaDeMedicmentos + "</h4>" + "</div>";

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

            return View();
        }



        public ActionResult crearCita()
        {
            var getname = db.paciente.ToList();
            SelectList list = new SelectList(getname, "id_paciente", "nombre");
            ViewBag.NombrePaciente = list;
            var getnameDoctor = db.doctor.ToList();
            SelectList lista = new SelectList(getnameDoctor, "id_doctor", "nombre");
            ViewBag.NombreDoctor = lista;

            return View();
        }

        [HttpPost]
        public ActionResult crearCita([Bind(Include = "id_pasiente,id_doctor,fechaDeConsulta,fechaDeCita,hora,duracion")]cita cita)
        {


            if (ModelState.IsValid)
            {
                db.cita.Add(cita);
                db.SaveChanges();
                return RedirectToAction("crearCita", "Doctor");
            }

            return View(cita);
        }


        public ActionResult citaPaciente()
        {

            return View(db.cita.ToList());
        }

        public ActionResult PagoConsulta()
        {

            return View(db.pagoConsulta.ToList());
        }

        public ActionResult TotalFecha(string id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "";
            var total = from p in db.pagoConsulta
                        where p.fecha.Equals(id)
                        group p by p.total;

            return View(total.ToList());
        }
    }
}