using mvc3.Areas.AdminPanel.Models;
using mvc3.Areas.AdminPanel.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc3.Areas.AdminPanel.Controllers
{
    [Authorize(Roles ="admin")]
    public class KategoriController : Controller
    {
        // GET: AdminPanel/Kategori
        static kitapProjesiEntities db = new kitapProjesiEntities();
        KategoriRepository repo = new KategoriRepository(db);

        
        
        public ActionResult Index()
        {
            return View(repo.Listele());
        }
        public ActionResult Kaydet()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(kategori kategori,HttpPostedFileBase resimAdi)
        {
            if (ModelState.IsValid)
            {
                if (resimAdi!=null)
                {
                    string isim = kategori.kategoriAdi + "-Resim.jpg";
                    string adres = Server.MapPath("~/Areas/AdminPanel/images/Kategori/"+isim);
                    resimAdi.SaveAs(adres);
                    kategori.resimAdi = isim;
                }
                repo.Kaydet(kategori); 
            }
            return RedirectToAction("Index");
        }
        public ActionResult Sil(int id)
        {
            return View(repo.Bul(id));
        }
        [HttpPost,ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult kategoriSil(int id)
        {
            repo.Sil(repo.Bul(id));
            return RedirectToAction("Index");
        }
        public ActionResult Duzenle(int id)
        {
            return View(repo.Bul(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(kategori kategori, HttpPostedFileBase resimAdi)
        {
            if (ModelState.IsValid)
            {
                if (resimAdi != null)
                {
                    string isim = kategori.kategoriAdi + "-Resim.jpg";
                    string adres = Server.MapPath("~/Areas/AdminPanel/images/Kategori/" + isim);
                    resimAdi.SaveAs(adres);
                    kategori.resimAdi = isim;
                }
                repo.Guncelle(kategori);
            }
            return RedirectToAction("Index");
        }
    }
}