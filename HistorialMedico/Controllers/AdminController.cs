using System;
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
    public class AdminController : Controller
    {
        private HistorialMedicoContext db = new HistorialMedicoContext();

        // GET: Admin
        public ActionResult Index()
        {
            var isAdmin = ViewBag.isAdmin;
            return View(db.usuario.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            ViewBag.RolSelectList = new List<SelectListItem>();

            //verificamos si esta nulo o el count es 0
            if (ViewBag.RolSelectList?.Count == 0)
            {
                var roles = new List<SelectListItem>();

                roles.Add(new SelectListItem { Value = "doctor", Text = "Doctor" });
                roles.Add(new SelectListItem { Value = "asistente", Text = "Asistente" });
                roles.Add(new SelectListItem { Value = "admin", Text = "Administrador" });

                ViewBag.RolSelectList = roles;
            }

            return View();
        }

        // POST: Admin/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_usuario,Nombre,Apellido,Correo,usuario1,contrasena,roles")] usuario usuario, string telefono, HttpPostedFileBase InputFile,
            [Bind(Include = "Nombre,Apellido,Correo")] asistente asistente, [Bind(Include = "Nombre,Apellido,Correo")] doctor doctor)
        {
            try {
                if (ModelState.IsValid)
                {

                    string resultado = string.Empty;
                    Byte[] encriptar = System.Text.Encoding.Unicode.GetBytes(usuario.contrasena);
                    resultado = Convert.ToBase64String(encriptar);
                    usuario.contrasena = resultado;
                    if (InputFile != null && InputFile.ContentLength > 0)
                    {
                        byte[] imagenData = null;
                        using (var imagen = new BinaryReader(InputFile.InputStream))
                        {
                            imagenData = imagen.ReadBytes(InputFile.ContentLength);
                        }
                        usuario.Foto = imagenData;
                        db.usuario.Add(usuario);
                        db.SaveChanges();
                    if (usuario.roles == "asistente") {
                        asistente.telefono = telefono;
                        asistente.foto = imagenData;
                        db.asistente.Add(asistente);
                        db.SaveChanges();

                    } else if (usuario.roles == "doctor") {
                        doctor.telefono = telefono;
                        doctor.foto = imagenData;
                        db.doctor.Add(doctor);
                        db.SaveChanges();
                    }
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e) {


            }

            return View(usuario);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Admin/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_usuario,Nombre,Apellido,Correo,usuario1,contrasena,roles")] usuario usuario, HttpPostedFileBase imagefile)
        {
            if (ModelState.IsValid)
            {
                try {
                    if (imagefile != null && imagefile.ContentLength > 0)
                    {
                        byte[] imagenData = null;
                        using (var imagen = new BinaryReader(imagefile.InputStream))
                        {
                            imagenData = imagen.ReadBytes(imagefile.ContentLength);
                        }
                        usuario.Foto = imagenData;
                    }
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e) {

                }
            }
            return View(usuario);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            usuario usuario = db.usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usuario usuario = db.usuario.Find(id);
            db.usuario.Remove(usuario);
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


        public ActionResult ReportesDelSistema() {


            return View();
        }

        [HttpPost]
        public ActionResult ResportesDeDoctores() {
            doctor doctor;
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                pdfDoc.Open();
                string cadenaFinal = "";
                List<doctor> visi = new List<doctor>();
                visi = db.doctor.ToList();
                foreach(doctor a in visi) {
                    cadenaFinal += "<center>" +
                        "<br />"+
                        "<table style='width: 50 % ' >" +
                        "<tr>"+
                        "<th>Nombre" + "</th>" +
                        "<th> Apellido" + "</th>" +
                        "<th> Telefono" + "</th>" +
                        "<th> Correo" + "</th>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>" + a.nombre + "</td>" +
                        "<td>" + a.apellido +"</td>" +
                        "<td>" + a.telefono +"</td>" +
                        "<td>" + a.correo +"</td>" +
                        "</tr>" +
                        "</table>" +
                        "</center>";
                }

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

            [HttpPost]
        public ActionResult ResportesDePacientes()
        {
            paciente paciente;
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                pdfDoc.Open();
                string cadenaFinal = "";
                List<paciente> visi = new List<paciente>();
                visi = db.paciente.ToList();
                foreach (paciente a in visi)
                {
                    cadenaFinal += "<center>" +
                        "<br />" +
                        "<table style="+ "width: 50 % "+">" +
                        "<tr>" +
                        "<th> Cedula" + "</th>" +
                        "<th> Nombre" + "</th>" +
                        "<th> Apellido" + "</th>" +
                        "<th> Telefono" + "</th>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>" + a.cedula + "</td>" +
                        "<td>" + a.nombre + "</td>" +
                        "<td>" + a.apellido + "</td>" +
                        "<td>" + a.telefono + "</td>" +
                        "</tr>" +
                        "</table>" +
                        "</center>";
                }

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

        [HttpPost]
        public ActionResult ResportesDeAsistentes()
        {

            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                pdfDoc.Open();
                string cadenaFinal = "";
                List<asistente> visi = new List<asistente>();
                visi = db.asistente.ToList();
                foreach (asistente a in visi)
                {
                    if ( a.nombre == "") {
                        cadenaFinal = "No hay datos";
                    }
                    else
                    {
                        cadenaFinal += "<center>" +
                      "<br />" +
                      "<table style='width: 50 % ' >" +
                      "<tr>" +
                      "<th>Nombre" + "</th>" +
                      "<th> Apellido" + "</th>" +
                      "<th> Telefono" + "</th>" +
                      "<th> Correo" + "</th>" +
                      "</tr>" +
                      "<tr>" +
                      "<td>" + a.nombre + "</td>" +
                      "<td>" + a.apellido + "</td>" +
                      "<td>" + a.telefono + "</td>" +
                      "<td>" + a.correo + "</td>" +
                      "</tr>" +
                      "</table>" +
                      "</center>";
                    }
                    
                }

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

        public ActionResult Calendario()
        {

            return View(db.cita.ToList());
        }


        public ActionResult Costo() {
            var getnameDoctor = db.doctor.ToList();
            SelectList lista = new SelectList(getnameDoctor, "id_doctor", "nombre");
            ViewBag.NombreDoctor = lista;
            return View(db.precioConsulta.ToList());
        }


        public ActionResult CrearCosto()
        {
            var getnameDoctor = db.doctor.ToList();
            SelectList lista = new SelectList(getnameDoctor, "id_doctor", "nombre");
            ViewBag.NombreDoctor = lista;
            return View();
        }

        [HttpPost]

        public ActionResult CrearCosto([Bind(Include = "id_precioConsulta,id_doctor,costo")] precioConsulta precioConsulta)
        {

            return View("Costo");
        }


        public ActionResult CostoD(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            precioConsulta precioConsulta = db.precioConsulta.Find(id);
            if (precioConsulta == null)
            {
                return HttpNotFound();
            }
            return View(precioConsulta);
        }

        [HttpPost]
        public ActionResult crearCostos([Bind(Include = "id_doctor,costo")] precioConsulta precioConsulta) {

            if (ModelState.IsValid)
            {
                db.precioConsulta.Add(precioConsulta);
                db.SaveChanges();
                return RedirectToAction("Costo", "Admin");
            }


                return View();
        }


        [HttpPost]
        public ActionResult CostoD([Bind(Include = "id_precioConsulta,id_doctor,costo")] precioConsulta precioConsulta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(precioConsulta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Costo");
            }
            return View(precioConsulta);
        }

        public ActionResult convertirImagen(int codigo)
        {
            var imagen = (from usuario in db.usuario
             where usuario.id_usuario == codigo
            select usuario.Foto).FirstOrDefault();
            return File(imagen, "Imagenes/jpg");
        }

        public ActionResult convertirImagenA(int codigo)
        {
            var imagen = (from asistente in db.asistente
                          where asistente.id_asistente == codigo
                          select asistente.foto).FirstOrDefault();
            return File(imagen, "Imagenes/jpg");
        }

        public ActionResult convertirImagenD(int codigo)
        {
            var imagen = (from doctor in db.doctor
                          where doctor.id_doctor == codigo
                          select doctor.foto).FirstOrDefault();
            return File(imagen, "Imagenes/jpg");
        }

    }
}
