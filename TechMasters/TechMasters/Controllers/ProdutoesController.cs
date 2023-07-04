using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TrabalhoPartico.Models;

namespace TrabalhoPartico.Controllers
{
    public class ProdutoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Produtoes
        public ActionResult Index()
        {
            return View(db.Produtos.ToList());
        }

        // GET: Produtoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produtos.Find(id);
            // Trabalheira só para arranjar a categoria...
            produto.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", id)).FirstOrDefault();
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Preco,Desconto,Stock,Descricao,UrlImg,Categoria")] Produto produto)
        {
            bool val = ModelState.IsValid;

            int id;
            try
            {
                id = int.Parse(ModelState.Values.ToArray()[6].Value.AttemptedValue);
            }
            catch (Exception)
            {
                id = -1;
            }

            if (id >= 0)
            {
                produto.Categoria = db.Categorias.Find(id);
                val = true;
                foreach (var v in ModelState.Values.ToArray())
                    if (v.Value.AttemptedValue == null || v.Value.AttemptedValue.Equals(""))
                    {
                        val = false;
                        break;
                    }
            }

            if (produto.Stock < 0)
                val = false;

            if (produto.Desconto < 0 || produto.Desconto > 1)
                val = false;

            if (val)
            {
                Produto prod = new Produto(produto.Nome, produto.Preco, produto.Stock, produto.Descricao, produto.UrlImg, produto.Categoria, produto.Desconto);

                db.Produtos.Add(prod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produtoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produtos.Find(id);
            produto.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", id)).FirstOrDefault();
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Preco,Desconto,Stock,Descricao,UrlImg,Categoria")] Produto produto)
        {
            bool val = ModelState.IsValid;

            Produto prod = db.Produtos.Find(produto.Id);

            prod.Nome = produto.Nome;
            prod.Preco = produto.Preco;
            prod.Stock = produto.Stock;
            prod.UrlImg = produto.UrlImg;
            prod.Id = produto.Id;
            prod.Descricao = produto.Descricao;
            prod.Desconto = produto.Desconto;

            int id;
            try
            {
                id = int.Parse(ModelState.Values.ToArray()[7].Value.AttemptedValue);
            }
            catch (Exception)
            {
                id = -1;
            }

            if (id >= 0)
            {
                prod.Categoria = db.Categorias.Find(id);
                val = true;
                try
                {
                    foreach (var v in ModelState.Values.ToArray())
                        if (v.Value.AttemptedValue == null || v.Value.AttemptedValue.Equals(""))
                        {
                            val = false;
                            break;
                        }
                }
                catch (Exception)
                {
                    if (!(ModelState.Values.ToArray()[7].Value != null && ModelState.Values.ToArray()[8].Value == null && ModelState.Values.ToArray()[9].Value == null))
                    {
                        val = false;
                    }
                }
            }

            if (prod.Stock < 0)
                val = false;

            if (prod.Desconto < 0 || prod.Desconto > 1)
                val = false;

            if (val)
            {
                db.Entry(prod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            prod.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", prod.Id)).FirstOrDefault();
            return View(prod);
        }

        // GET: Produtoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produtos.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produtos.Find(id);
            db.Produtos.Remove(produto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
