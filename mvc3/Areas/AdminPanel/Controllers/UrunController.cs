using mvc3.Areas.AdminPanel.Models;
using mvc3.Areas.AdminPanel.Models.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc3.Areas.AdminPanel.Controllers
{
    [Authorize(Roles = "admin")]
    public class UrunController : Controller
    {
        // GET: AdminPanel/Urun
        static kitapProjesiEntities db = new kitapProjesiEntities();
        UrunRepository repo = new UrunRepository(db);
        public ActionResult Index()
        {
            return View(repo.Listele());
        }

        public ActionResult Kaydet()
        {
            ViewBag.Kategoriler = repo.KategoriListesi();
           // ViewData["Kategoriler"] = repo.KategoriListesi();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet( urun urun,IEnumerable<HttpPostedFileBase> resim)
        {
            repo.Kaydet(urun);
            if (resim.First() != null)
            {
                resim res = new resim();
                res.urunNo = urun.urunNo;
                foreach (var item in resim)
                {
                    using (var br = new BinaryReader(item.InputStream))
                    {
                        var data = br.ReadBytes(item.ContentLength);
                        res.resimAdi = data;
                    }
                    repo.ResimKaydet(res);
                }
            }

            return RedirectToAction("Index");

        }
        //Sil
        public ActionResult Sil(int id)
        {
            return View(repo.Bul(id));
        }
        // sil HttpPost
        [HttpPost,ActionName("Sil")]
        public ActionResult urunSil(int id)
        {
            repo.Sil(repo.Bul(id));
            return RedirectToAction("Index");
        }
        //Değiştir
        public ActionResult Duzenle(int id)
        {
            ViewBag.Kategoriler = repo.KategoriListesi();
            return View(repo.Bul(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(urun urun,IEnumerable<HttpPostedFileBase> resim)
        {
            repo.Guncelle(urun);
            if (resim.First() != null)
            {
                resim res = new resim();
                res.urunNo = urun.urunNo;
                foreach (var item in resim)
                {
                    using (var br = new BinaryReader(item.InputStream))
                    {
                        var data = br.ReadBytes(item.ContentLength);
                        res.resimAdi = data;
                    }
                    repo.ResimKaydet(res);
                    //urun.resim.Add(res);
                }
            }

            return RedirectToAction("Index");
        }
     
        [HttpPost]
        public string resimSil(int resimId)
        {
             return repo.resimsil(resimId);
        }


        
    }
}