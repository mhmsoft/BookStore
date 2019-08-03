using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class yorumRepository
    {
        kitapProjesiEntities _context;
        public yorumRepository(kitapProjesiEntities Context)
        {
            _context = Context;
        }

        public string yorumKaydet(yorum yorum)
        {
            if (yorum != null)
            {
                _context.yorum.Add(yorum);
                _context.SaveChanges();
                return "yorumunuz kaydedildi.";
            }
            else
                return "yorum kaydedilemedi.";
        }
    }
}