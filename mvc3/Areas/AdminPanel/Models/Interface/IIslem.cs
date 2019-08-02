using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace mvc3.Areas.AdminPanel.Models.Interface
{
    interface IIslem<T>
    {
        void Kaydet(T entity);
      //  void Kaydet(T entity, IEnumerable<HttpPostedFileBase> resim);
        void Guncelle(T entity);
        void Sil(T entity);
        List<T> Listele();
        T Bul(int id);
    }
}
