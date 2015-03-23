using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesseract;


namespace tesseract_aspnet.Models
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.ShowResult = false;
            ViewBag.Title = "Tesseract ASP.NET MVC Demo";
            return View("Index");
        }

        [ValidateAntiForgeryToken]
        public ActionResult Submit(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.ShowResult = true;
                ViewBag.Result = "File not found";
                return View("Index");
            }

         

            using (var engine = new TesseractEngine(Server.MapPath(@"~/tessdata"), "eng", EngineMode.Default))
            {
                // have to load Pix via a bitmap since Pix doesn't support loading a stream.
                using (var image = new System.Drawing.Bitmap(file.InputStream))
                {
                    using (var pix = PixConverter.ToPix(image))
                    {

                        using (var page = engine.Process(pix))
                        {
                            ViewBag.ShowResult = true;
                            ViewBag.MeansConfidence = String.Format("{0:P}", page.GetMeanConfidence());
                            ViewBag.Result = page.GetText();
                        }
                    }
                }
            }

            return View("Index");
         
        }
    }
}
