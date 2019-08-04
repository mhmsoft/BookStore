﻿using mvc3.Areas.AdminPanel.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc3.Areas.AdminPanel.Models.Repository
{
    public class SiparisDetayRepository : IIslem<siparisDetay>
    {
        private kitapProjesiEntities _context;
        public SiparisDetayRepository(kitapProjesiEntities Context)
        {
            _context = Context;
        }
        public siparisDetay Bul(int id)
        {
            return _context.siparisDetay.Find(id);
        }

        public void Guncelle(siparisDetay entity)
        {
            if (entity != null)
            {
                siparisDetay old = Bul(entity.siparisNo);
                _context.Entry(old).State = System.Data.Entity.EntityState.Detached;
                _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void Kaydet(siparisDetay entity)
        {
            if (entity != null)
            {
                _context.siparisDetay.Add(entity);
                _context.SaveChanges();
            }

        }

        public List<siparisDetay> Listele()
        {
            return _context.siparisDetay.AsNoTracking().ToList();
        }

        public void Sil(siparisDetay entity)
        {
            if (entity != null)
            {
                _context.siparisDetay.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}