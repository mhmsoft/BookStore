using mvc3.Areas.AdminPanel.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc3.Areas.AdminPanel.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace mvc3.Controllers
{
    public class ShopController : Controller
    {
        UrunRepository repo = new UrunRepository(new kitapProjesiEntities());
        // GET: Shop
      
        public ActionResult Index(int? categoryId, string yazar =null)
        {
            if (!string.IsNullOrEmpty(yazar))
            {
                return View(repo.Listele(x => x.yazar == yazar).ToList());//ToPagedList(pageNumber, pageSize));
            }
            if (categoryId != null)
            {
                return View(repo.Listele(x => x.kategoriNo == categoryId).ToList());//ToPagedList(pageNumber, pageSize));
            }
            else
            return View(repo.Listele());
        }
        [HttpGet]
        public ActionResult productDetail(int productId)
        {
            //ViewBag.relatedProduct=repo.
            var sonuc = repo.Bul(productId);
            return View(sonuc);
        }
        public ActionResult PartialCategory()
        {
            return PartialView(repo.KategoriListesi());
        }
        public ActionResult Thumbnail(int width, int height, int Id, int _resimNo)
        {
            var photo = repo.Bul(Id).resim.FirstOrDefault(resimId => resimId.resimNo == _resimNo).resimAdi;
            var base64 = Convert.ToBase64String(photo);
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);

            using (var newImage = new Bitmap(width, height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var stream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height));
                newImage.Save(stream, ImageFormat.Png);
                return File(stream.ToArray(), "image/png");
            }

        }

    }
}