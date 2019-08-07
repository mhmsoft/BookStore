using mvc3.Areas.AdminPanel.Models;
using mvc3.Areas.AdminPanel.Models.Repository;
using mvc3.Areas.AdminPanel.Models.Security;
using mvc3.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc3.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
      
        UserRepository repo = new UserRepository(new Areas.AdminPanel.Models.kitapProjesiEntities());
        FavoriuteRepository repoFav = new FavoriuteRepository(new Areas.AdminPanel.Models.kitapProjesiEntities());
        KategoriRepository repoCat = new KategoriRepository(new Areas.AdminPanel.Models.kitapProjesiEntities());
        UrunRepository repoProduct = new UrunRepository(new Areas.AdminPanel.Models.kitapProjesiEntities());
        SiparisDetayRepository repoSiparisDetay = new SiparisDetayRepository(new Areas.AdminPanel.Models.kitapProjesiEntities());
        SiparisRepository repoSiparis = new SiparisRepository(new Areas.AdminPanel.Models.kitapProjesiEntities());
        public ActionResult PartialCategory()
        {
            return PartialView(repoCat.Listele());
        }

        public ActionResult PartialPrice()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var accountOwner = User.Identity.Name;
                var user = repo.Listele().Where(x => x.email == accountOwner).FirstOrDefault();
                AccountVM model = new AccountVM()
                {
                    address = user.address,
                    city = user.city,
                    newPassword = user.password,
                    gsm = user.phone,
                    email = user.email,
                    lastName = user.lastname,
                    firstName = user.firstname,
                    subscribe = user.subscribe??false
                };
                return View(model);
            }
            return Redirect("~/User/Login");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountVM account)
        {
            string message = "";
            bool status = false;
           // kullanıcı bulma
            var user = repo.Listele().Where(x => x.email == account.email).FirstOrDefault();
            if (user==null)
            {
                message = "kullanıcı bulunamadı";
                ViewBag.message = message;
                return View("Index");
            }
            //  password güncelleyeğiz
            if (!String.IsNullOrEmpty(account.newPassword) && !String.IsNullOrEmpty(account.comfirmPassword))
            {
                user.password = Crypto.Hash(account.newPassword);
                user.rePassword = Crypto.Hash(account.comfirmPassword);
            }
            //  address güncelleyeğiz
            if (!String.IsNullOrEmpty(account.address))
            {
                user.address = account.address;
            }
            //  sehir güncelleyeğiz
            if (!String.IsNullOrEmpty(account.city))
            {
                user.city = account.city;
            }
            //  gsm güncelleyeğiz
            if (!String.IsNullOrEmpty(account.gsm))
            {
                user.phone = account.gsm;
            }
            //  lastname-firstname güncelleyeğiz
            if (!String.IsNullOrEmpty(account.lastName)&& (!String.IsNullOrEmpty(account.firstName)))
            {
                user.firstname = account.firstName;
                user.lastname = account.lastName;
            }
            if (account.subscribe==true)
            {
                user.subscribe = account.subscribe;
            }
             message = "Bilgileriniz Güncellendi";
            status = true;
            ViewBag.message = message;
            ViewBag.status = status;
            repo.Guncelle(user);
            return RedirectToAction("Index");
        }
        public ActionResult MyFavourite()
        {
            var accountOwner = User.Identity.Name;
            return View(repoFav.Listele().Where(x=>x.user.email==accountOwner).ToList());
        }
        [HttpPost]
        public void DeleteFavourite(int id)
        {
            var favourite = repoFav.Bul(id);
            repoFav.Sil(favourite);
        }
            
        [HttpPost]
        public string AddFavourite(int productId)
        {
            var accountOwner = User.Identity.Name;
            //favori ekleme
            // emaili olan kullanıcının user id sini bulalım.
            int userId = repo.Listele().Where(x => x.email == accountOwner).FirstOrDefault().userId;
            // favoriler tablosunda bu kullanıcıya ait gelen kitap varmı yokmu
            int count = repoFav.Listele().Where(x => x.userId == userId && x.productId == productId).Count();
            if (count==0)
            {
                var product = repoProduct.Bul(productId);
                favorim newFavori = new favorim()
                {
                    productId = productId,
                    userId = userId
                };
                repoFav.Kaydet(newFavori);
                return "Favorilerime Eklendi";
            }
            else
                return "Favorilerime Daha önceden eklemişsiniz";
        }
        [HttpGet]
        public PartialViewResult MyOrders()
        {
            var accountOwner = User.Identity.Name;
            int customerId = repo.Listele().Where(x => x.email == accountOwner).FirstOrDefault().userId;

            var orders = repoSiparis.Listele().Where(x => x.musteriNo == customerId).ToList();

            return PartialView(orders);
        }
        public PartialViewResult MyCoupons()
        {
            var accountOwner = User.Identity.Name;
            int customerId = repo.Listele().Where(x => x.email == accountOwner).FirstOrDefault().userId;

            var coupons = repo.Bul(customerId).indirim.Where(i => i.musteriNo == customerId).ToList();

            return PartialView(coupons);
        }

        [HttpPost]
        public void applyDiscount(string discountCode)
        {
            var accountOwner = User.Identity.Name;
            int customerId = repo.Listele().Where(x => x.email == accountOwner).FirstOrDefault().userId;
            var discount = repo.Bul(customerId).indirim.FirstOrDefault(i=>i.indirimDurum==true && i.indirimKodu==discountCode && DateTime.Now<i.indirimBitis).indirimTutar;
            if (discount != null)
            {
                indirim _indirim = new indirim()
                {
                    indirimTutar = discount,
                    indirimKodu=discountCode
                };
                Session["discount"] = _indirim;
            }
        }
        
    }
}