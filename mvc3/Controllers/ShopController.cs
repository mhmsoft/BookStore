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
using System.Net.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace mvc3.Controllers
{
    public class ShopController : Controller
    {
        UrunRepository repo = new UrunRepository(new kitapProjesiEntities());
        UserRepository repoUser = new UserRepository(new kitapProjesiEntities());
        yorumRepository repoYorum = new yorumRepository(new kitapProjesiEntities());
        SiparisRepository repoSiparis = new SiparisRepository(new kitapProjesiEntities());
        SiparisDetayRepository repoSiparisDetay = new SiparisDetayRepository(new kitapProjesiEntities());
        IndirimRepository repoIndirim = new IndirimRepository(new kitapProjesiEntities());
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

        public ActionResult Basket()
        {
            return View();
        }
        [HttpPost]
        public void UpdateCard(int productId,int quantity)
        {
            if (Session["card"] != null)
            {
                List<BasketItem> card = (List<BasketItem>)Session["card"];
                int index = isExistInCard(productId);
                if (index != -1)
                {
                    // sadece adedini gelen quantity kadar arttıracak.
                    card[index].quantity = quantity;
                }
            }
        }
        [HttpPost]
        public void ClearCard()
        {
            Session["card"] = null;
            
        }
        // sepetteki elemanı silme
        public void deleteItemInCard(int productId)
        {
            List<BasketItem> card = (List<BasketItem>)Session["card"];
            if (card.Exists(x => x.product.urunNo == productId))
            {
                int index = isExistInCard(productId);

                card.RemoveAt(index);
                Session["card"] = card;
               
            }
            
        }
        [Authorize(Roles = "User")]
        public ActionResult Checkout()
        {
            if(User.Identity.IsAuthenticated)
            { 
                user _user = repoUser.Listele().Where(x => x.email == User.Identity.Name).FirstOrDefault();
                return View(_user);
            }
            return RedirectToAction("Login", "User");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Checkout(user _user, bool?shipbox ,string bilgi,string ad, string soyad, string sirket,string adres,string sehir, int?postakodu,string eposta, string telefon)
        {
            string message = "";
            bool status = false;
            bool satisTamamlandi = false;
            siparis newOrder = new siparis();
            // farklı adres seçilmişse(user başkası adına alışveriş yaparsa)
             if (shipbox==true)
             {
                Session["note"] = bilgi;
                Session["ad"] = ad;
                Session["soyad"] = soyad;
                Session["adres"] = adres;
                Session["sehir"] = sehir;
                Session["sirket"] = sirket;
                Session["telefon"] = telefon;
                Session["postakodu"] = postakodu ?? 0;
                Session["isGuest"] = true;
                
                if (string.IsNullOrEmpty(ad))
                {
                    message = "isim alanı boş bıraktınız";
                    ViewBag.message = message;
                    return View();
                }
                else
                    newOrder.firstname = ad;
                if (string.IsNullOrEmpty(soyad))
                {
                    message = "Soyisim alanı boş bıraktınız";
                    ViewBag.message = message;
                    return View();
                }
                else
                    newOrder.lastname = soyad;
                if (!string.IsNullOrEmpty(sirket))
                    newOrder.company = sirket;
                if (string.IsNullOrEmpty(adres))
                {
                    message = "Adres alanı boş bıraktınız";
                    ViewBag.message = message;
                    return View();
                }
                   
                else
                    newOrder.address = adres;
                if (string.IsNullOrEmpty(sehir))
                {
                    message = "Şehir alanı boş bıraktınız";
                    ViewBag.message = message;
                    return View();
                }  
                else
                    newOrder.city = sehir;
                if (postakodu!=null)
                    newOrder.postakodu = postakodu;
                if (!string.IsNullOrEmpty(eposta))
                    newOrder.email = eposta;
                if (!string.IsNullOrEmpty(bilgi))
                    newOrder.note = bilgi;
                if (string.IsNullOrEmpty(telefon))
                {
                    message = "Telefon alanı boş bıraktınız";
                    ViewBag.message = message;
                    return View();
                }
                    
                else
                    newOrder.phone = telefon;

                newOrder.siparisTarihi = DateTime.Now;
                newOrder.musteriNo = _user.userId;
                // siparisi kaydet
                repoSiparis.Kaydet(newOrder);
            }
            // farklı adress seçilmemişse(normal user alışveriş yaparsa)
            else
            {
                Session.Remove("ad");
                Session.Remove("soyad");
                Session.Remove("adres");
                Session.Remove("sehir");
                Session.Remove("sirket");
                Session.Remove("telefon");
                Session.Remove("postakodu");
                Session.Remove("note");
                Session.Remove("isGuest");
                if (_user!=null)
                {
                    if (string.IsNullOrEmpty(_user.firstname))
                    {
                        message = "ad alanı boş bırakmayınız";
                        ViewBag.message = message;
                        return View();
                    }
                      
                    if (string.IsNullOrEmpty(_user.lastname))
                    {
                        message = "soyad alanı boş bırakmayınız";
                        ViewBag.message = message;
                        return View();
                    }
                    if (string.IsNullOrEmpty(_user.address))
                    {
                        message = "adres alanı boş bırakmayınız";
                        ViewBag.message = message;
                        return View();
                    }
                    if (string.IsNullOrEmpty(_user.phone))
                    {
                        message = "Telefon alanı boş bırakmayınız";
                        ViewBag.message = message;
                        return View();
                    }
                    if (string.IsNullOrEmpty(_user.city))
                    {
                        message = "sehir alanı boş bırakmayınız";
                        ViewBag.message = message;
                        return View();
                    }
                       
                    newOrder.siparisTarihi = DateTime.Now;
                    newOrder.user = _user;

                    repoSiparis.Kaydet(newOrder);
                    
                }
            }
            if (Session["card"] != null)
            {
                List<BasketItem> Basket = (List<BasketItem>)Session["card"];
                siparisDetay newOrderDetail = new siparisDetay();
               
                foreach (var item in Basket)
                {
                    
                    newOrderDetail.miktar = item.quantity;
                    newOrderDetail.siparisNo = newOrder.siparisNo;
                    newOrderDetail.urunNo = item.product.urunNo;
                    repoSiparisDetay.Kaydet(newOrderDetail);
                }
                // 5 lira kargo üzreti

                var sepetTutari = Basket.Sum(x => x.quantity * x.product.fiyat)+5m;
                if(Session["discount"]!=null)
                {
                    indirim _indirim = (indirim)Session["discount"];
                    sepetTutari -= (decimal)_indirim.indirimTutar;
                    newOrder.indirimtutar = _indirim.indirimTutar;
                    // indirim kullanıldığı için indirimi pasif et
                    indirim kullanilanIndirim = repoIndirim.Listele().FirstOrDefault(x => x.indirimKodu == _indirim.indirimKodu);

                    kullanilanIndirim.kullanidiMi = true;
                    kullanilanIndirim.indirimDurum = false;
                    repoIndirim.Guncelle(kullanilanIndirim);
                   
                }

                // indirim uygulansn yada uygulanmasın. siparişi güncelliyoruz.
                newOrder.siparistutar = sepetTutari;
                repoSiparis.Guncelle(newOrder);
                satisTamamlandi = true;
                status = true;
                // hediye kupon oluştur
                if (Basket.Sum(x => x.quantity * x.product.fiyat) > 150)
                {
                    string couponCode = createCoupon();
                    string subject = " Bookstore iİndirim Kuponu";
                    string body = "Tebrikler! 150 TL alışveriş yaptığınız için % 5 indirim kuponu kazandınız." +
                                  "İndirim kuponunuzu kullanmak için son gün:" + DateTime.Now.AddDays(10);
                    indirim newCoupon = new indirim()
                    {
                        musteriNo = _user.userId,
                        indirimDurum = true,
                        indirimBaslangic = DateTime.Now,
                        indirimBitis = DateTime.Now.AddDays(10),
                        indirimKodu = couponCode,
                        aciklama = "%5 Hediye kuponu",
                        indirimTutar = Basket.Sum(x => x.quantity * x.product.fiyat) * 0.05m,
                        kullanidiMi = false
                    };
                    repoIndirim.Kaydet(newCoupon);
                    // kupon haketmişse mail gönderiliyor.
                    SendCouponMail(User.Identity.Name, couponCode, subject, body);
                }

                if (satisTamamlandi)
                {
                    // sepeti sil
                    Session.Remove("card");
                    //indirim sil
                    Session.Remove("discount");
                }

                // sipariş maili gönderiliyor.
                SendOrderInfo(repoUser.Listele().Where(x=>x.email==User.Identity.Name).FirstOrDefault().email);
                message = " Sipariş işlemi tamamlandı. siparişiniz ile ilgili bilgi mailinize gönderilmiştir." +
                           "Bookstore hesabım sayfasında sipariş detaylarını görebilirisiniz.";
               
               
            }
            ViewBag.status = status;
            ViewBag.message = message;
            return View();
        }
        public string createCoupon()
        {
            Random N = new Random();
            string result = "";
            char[] expression = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'V', 'Y', 'X', 'W', 'Z', '1', '2', '3', '4', '5' };
            for (int i = 0; i < 8; i++)
            {
                result += expression[N.Next(expression.Length)].ToString();
            }
            return result;
        }

        [NonAction]
        public void SendCouponMail(string _email,string _couponCode,string _subject,string _message)
        {
            SmtpSection network = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                var url = "/Account/MyCoupons";
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
                var fromEmail = new MailAddress(network.Network.UserName, _subject);
                var toEmail = new MailAddress(_email);

                string subject = _subject;
                string body = "<br/><br/>"+_message +
                      " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                var smtp = new SmtpClient
                {
                    Host = network.Network.Host,
                    Port = network.Network.Port,
                    EnableSsl = network.Network.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = network.Network.DefaultCredentials,
                    Credentials = new NetworkCredential(network.Network.UserName, network.Network.Password)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [NonAction]
        public void SendOrderInfo(string emailID)
        {
            SmtpSection network = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            try
            {
                var url = "/Account/MyOrders";
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
                var fromEmail = new MailAddress(network.Network.UserName, "Bookstore Sipariş Bilgisi");
                var toEmail = new MailAddress(emailID);

                string subject = "Bookstore Sipariş Bilgisi";
                string body = "<br/><br/>Bookstore sayfanızda sipariş detaylarını görebilirisiniz. Detay için aşağıdaki linke tıklayınız" +
                      " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                var smtp = new SmtpClient
                {
                    Host = network.Network.Host,
                    Port = network.Network.Port,
                    EnableSsl = network.Network.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = network.Network.DefaultCredentials,
                    Credentials = new NetworkCredential(network.Network.UserName, network.Network.Password)
                };
                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}