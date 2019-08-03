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
using PagedList;
using mvc3.Models.ViewModel;

namespace mvc3.Controllers
{
    public class ShopController : Controller
    {
        UrunRepository repo = new UrunRepository(new kitapProjesiEntities());
        yorumRepository repoYorum = new yorumRepository(new kitapProjesiEntities());
        // GET: Shop

        public ActionResult Index(int? categoryId, int? page, int? PageSize, int? orderBy, int? minPrice, int? maxPrice)
        {
            ViewBag.orderBy = new List<SelectListItem>() {
                new SelectListItem { Text = "Fiyat", Value = "1", Selected = true },
                new SelectListItem { Text = "İsim", Value = "2" },
                new SelectListItem { Text = "No", Value = "3" },

            };
            ViewBag.PageSize = new List<SelectListItem>() {
                new SelectListItem { Text = "20", Value = "20", Selected = true },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "2", Value = "2" },
                new SelectListItem { Text = "1", Value = "1" }
            };

            //  actif sayfa no
            int pageNumber = page ?? 1;
            // her bir sayfada kaç ürün olacağını gösteren
            int pageSize = PageSize ?? 2;

            var result = repo.Listele();

            if (categoryId != null)
            {
                result = result.Where(x => x.kategoriNo == categoryId).ToList();
            }
            // fiyat seçilmişse
            else if (orderBy == 1)
                result = result.OrderBy(x => x.fiyat).ToList();
            // ürün adi seçilmişse
            else if (orderBy == 2)
                result = result.OrderBy(x => x.urunAdi).ToList();
            // ürün no seçilmişse
            else if (orderBy == 3)
                result = result.OrderBy(x => x.urunNo).ToList();
            // ürün minprice ve maxprice seçilmişse
            else if (minPrice != null & maxPrice != null)
                result = result.Where(x => x.fiyat >= minPrice && x.fiyat <= maxPrice).ToList();

            return View(result.ToPagedList(pageNumber, pageSize));
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

        public ActionResult PartialPrice()
        {
            return PartialView();
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


        [HttpPost]
        public string yorumKaydet(int _urunNo, string _yorumcu, string _yorum)
        {
            yorum model = new yorum()
            {
                yorumcu = _yorumcu,
                yorumAdi = _yorum,
                yorumTarihi = DateTime.Now,
                urunNo = _urunNo
            };
            return repoYorum.yorumKaydet(model);
        }

        [NonAction]
        private int isExistInCard(int id)
        {
            List<BasketItem> card = (List<BasketItem>)Session["card"];
            for (int i = 0; i < card.Count; i++)
                if (card[i].product.urunNo.Equals(id))
                    return i;
            return -1;
        }
        
        public ActionResult AddCard(int productId, int quantity)
        {
            // id si verilebn ürünü getir.
            urun _product = repo.Bul(productId);
            // eğer session içi boşsa
            if (Session["card"] == null)
            {
                List<BasketItem> Card = new List<BasketItem>();
                Card.Add(new BasketItem()
                {
                    Id = Guid.NewGuid(),
                    product = _product,
                    quantity = quantity,
                    DateCreated = DateTime.Now
                });
                Session["card"] = Card;
            }
            else
            {
                List<BasketItem> card = (List<BasketItem>)Session["card"];
                // sepette eklenen ürünün  sepetteki sıra numarasına bakılır. varsa sepetteki sıra no gönderilir, yoksa -1 değeri gönderilir.
                int index = isExistInCard(productId);
                // sepette eklenen ürün varsa
                if (index != -1)
                {
                    // sadece adedini gelen quantity kadar arttıracak.
                    card[index].quantity += quantity;
                }
                // sepette girilen ürün yoksa 
                else
                    // sepete ekle
                    card.Add(new BasketItem
                    {
                        Id = Guid.NewGuid(),
                        product = _product,
                        quantity = quantity,
                        DateCreated = DateTime.Now
                    });
                Session["card"] = card;
            }
            return RedirectToAction("Index");
           // return Json( (List<BasketItem>) Session["card"], JsonRequestBehavior.AllowGet);

        }
    }
}