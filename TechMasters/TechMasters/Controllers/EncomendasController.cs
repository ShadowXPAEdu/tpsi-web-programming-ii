using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrabalhoPartico.Models;

namespace TrabalhoPartico.Controllers
{
    public class EncomendasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Entregue(int? id)
        {
            if (id != null)
            {
                Encomendas enc = db.Encomendas.Find(id);
                enc.Id = enc.Id;
                enc.PrecoTotal = enc.PrecoTotal;
                enc.DataEncomenda = enc.DataEncomenda;
                enc.Estado = "Entregue";
                db.Entry(enc).State = EntityState.Modified;
                db.SaveChanges();
                int c = db.ProdQuants.SqlQuery("SELECT [PQ1].[Id] AS[Id], [PQ1].[Quantidade] AS[Quantidade]  FROM[dbo].[Encomendas] AS[Enc1], [dbo].[ProdQuants] AS[PQ1] WHERE[PQ1].[Encomendas_Id] = [Enc1].[Id] and [Enc1].[Id] = @idEncomenda", new SqlParameter("@idEncomenda", id)).Count();
                for (int i = 0; i < c; i++)
                {
                    var prods = db.Produtos.SqlQuery("SELECT [Pro1].[Id] AS[Id], [Pro1].[Nome] AS[Nome], [Pro1].[Preco] AS[Preco], [Pro1].[Stock] AS[Stock], [Pro1].[Descricao] AS[Descricao], [Pro1].[UrlImg] AS[UrlImg], [Pro1].[Desconto] AS[Desconto]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Encomendas] AS[Enc1], [dbo].[ProdQuants] AS[PQ1] WHERE[PQ1].[Encomendas_Id] = [Enc1].[Id] and [Enc1].[Id] = @idEncomenda and [PQ1].[Produto_Id] = [Pro1].[Id]", new SqlParameter("@idEncomenda", id)).ElementAt(i);
                    var prodQuants = db.ProdQuants.SqlQuery("SELECT [PQ1].[Id] AS[Id], [PQ1].[Quantidade] AS[Quantidade]  FROM[dbo].[Encomendas] AS[Enc1], [dbo].[ProdQuants] AS[PQ1] WHERE[PQ1].[Encomendas_Id] = [Enc1].[Id] and [Enc1].[Id] = @idEncomenda", new SqlParameter("@idEncomenda", id)).ElementAt(i);

                    // atualizar stock...
                    Produto.AtualizarStockEnc(prods, prodQuants.Quantidade);
                }
            }
            return RedirectToAction("Index");
        }

        /*
        public ActionResult AddProd(int? id, string str2)
        {
            if (id != null)
            {
                Produto prod = db.Produtos.Find(id);
                prod.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", id)).FirstOrDefault();

                switch (str2)
                {
                    case "Prods":
                        ((Carrinho)Session["CarEnc"]).AddProdEnc(prod, 1);
                        break;
                    case "ProdCar":
                        ((Carrinho)Session["CarEnc"]).RemProdEnc(prod, 1);
                        break;
                    default:
                        break;
                }
            }

            return RedirectToAction("Prods", new { str = str2 });
        }

        public ActionResult Prods(string str)
        {
            if (str == "Prods")
            {
                ViewBag.modelo = "Prods";
                return View(db.Produtos.ToList());
            }
            else if (str == "ProdCar")
            {
                ViewBag.modelo = "ProdCar";
                List<Produto> temp = new List<Produto>();
                foreach (var p in ((Carrinho)Session["CarEnc"]).Produtos)
                {
                    temp.Add(p.Produto);
                }
                return View(temp);
            }
            else
                return RedirectToAction("~/Shared/Error");
        }
        */

        public ActionResult ProdsV2()
        {
            return View(db.Produtos.ToList());
        }

        public ActionResult AddProd2(int? id, string str2)
        {
            if (id != null)
            {
                Produto prod = db.Produtos.Find(id);
                prod.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", id)).FirstOrDefault();

                switch (str2)
                {
                    case "Prods":
                        ((Carrinho)Session["CarEnc"]).AddProdEnc(prod, 1);
                        break;
                    case "ProdCar":
                        ((Carrinho)Session["CarEnc"]).RemProdEnc(prod, 1);
                        break;
                    default:
                        break;
                }
            }

            return RedirectToAction("ProdsV2");
        }

        // GET: Encomendas
        public ActionResult Index()
        {
            return View(db.Encomendas.ToList());
        }

        // GET: Encomendas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomendas encomendas = db.Encomendas.Find(id);
            if (encomendas == null)
            {
                return HttpNotFound();
            }
            return View(encomendas);
        }

        // GET: Encomendas/Create
        public ActionResult Create()
        {
            Session["CarEnc"] = new Carrinho();
            return View();
        }

        // POST: Encomendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PrecoTotal,Estado,DataEncomenda")] Encomendas encomendas)
        {
            if (((Carrinho)Session["CarEnc"]).Produtos.Count > 0)
            {
                // adicionar encomenda
                foreach (ProdQuant pq in ((Carrinho)Session["CarEnc"]).Produtos)
                {

                    db.Entry((pq.Produto.Categoria = db.Categorias.Find(pq.Produto.Categoria.Id))).State = EntityState.Unchanged;
                    db.Entry((pq.Produto = db.Produtos.Find(pq.Produto.Id))).State = EntityState.Unchanged;

                    db.ProdQuants.Add(pq);
                    db.SaveChanges();
                }

                Encomendas e = new Encomendas(((Carrinho)Session["CarEnc"]).Produtos, ((Carrinho)Session["CarEnc"]).GetPriceEnc());
                db.Encomendas.Add(e);
                db.SaveChanges();
                Session["CarEnc"] = new Carrinho();
                return RedirectToAction("Index");
            }

            return View(encomendas);
        }

        // GET: Encomendas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomendas encomendas = db.Encomendas.Find(id);
            if (encomendas == null)
            {
                return HttpNotFound();
            }

            if (encomendas.Estado == "Pendente")
            {
                // cancelar a encomenda
                /*
                db.ProdQuants.RemoveRange(db.ProdQuants.SqlQuery("SELECT [PQ1].[Id] AS[Id], [PQ1].[Quantidade] AS[Quantidade]  FROM[dbo].[Encomendas] AS[Enc1], [dbo].[ProdQuants] AS[PQ1] WHERE[PQ1].[Encomendas_Id] = [Enc1].[Id] and [Enc1].[Id] = @idEncomenda", new SqlParameter("@idEncomenda", id)));
                db.SaveChanges();
                db.Encomendas.Remove(encomendas);
                db.SaveChanges();
                */
                encomendas.Id = encomendas.Id;
                encomendas.PrecoTotal = encomendas.PrecoTotal;
                encomendas.DataEncomenda = encomendas.DataEncomenda;
                encomendas.Estado = "Cancelado";
                db.Entry(encomendas).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        /*
        // POST: Encomendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Encomendas encomendas = db.Encomendas.Find(id);
            db.Encomendas.Remove(encomendas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
