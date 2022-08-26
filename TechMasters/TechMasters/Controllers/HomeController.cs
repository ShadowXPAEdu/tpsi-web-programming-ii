using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TrabalhoPartico.Models;

namespace TrabalhoPartico.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<Produto> prods = db.Produtos.OrderByDescending(m => m.Desconto).ToList();

            return View(prods);
        }

        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }

        public ActionResult Inventario()
        {
            // OrderBy Stock * Preco
            List<Produto> prods = db.Produtos.OrderBy(m => m.Stock * m.Preco).ToList();
            return View(prods);
        }

        public ActionResult Detalhes(int? id)
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

        public ActionResult Carrinho()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        public ActionResult AddCarrinho(int? id, string str)
        {
            Produto prod = db.Produtos.Find(id);
            prod.Categoria = db.Categorias.SqlQuery("SELECT [Cat1].[Id] AS[Id], [Cat1].[Nome] AS[Nome], [Cat1].[Descricao] AS[Descricao]  FROM[dbo].[Produtoes] AS[Pro1], [dbo].[Categorias] AS[Cat1] WHERE[Pro1].[Categoria_Id] = [Cat1].[Id] and [Pro1].[Id] = @idProd", new SqlParameter("@idProd", id)).FirstOrDefault();

            switch (str)
            {
                case "AddFromInv":
                    ((Carrinho)Session["Carrinho"]).AddProduto(prod, 1);
                    return RedirectToAction("Inventario");
                case "AddFromDet":
                    ((Carrinho)Session["Carrinho"]).AddProduto(prod, 1);
                    return RedirectToAction("Detalhes/" + id);
                case "AddFromCar":
                    ((Carrinho)Session["Carrinho"]).AddProduto(prod, 1);
                    return RedirectToAction("Carrinho");
                case "DecFromCar":
                    ((Carrinho)Session["Carrinho"]).RemoveProduto(prod, 1);
                    return RedirectToAction("Carrinho");
                case "RemFromCar":
                    ((Carrinho)Session["Carrinho"]).RemoveProduto(prod);
                    return RedirectToAction("Carrinho");
                default:
                    return RedirectToAction("Index");
            }
        }

        public ActionResult EfetuarCompra()
        {
            if (User.Identity.IsAuthenticated)
            {
                Cliente c = db.Clientes.Where(c1 => c1.Id == db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault().Id).FirstOrDefault();

                if (c != null && ((Carrinho)Session["Carrinho"]).Produtos.Count > 0)
                {
                    // Adicionar a lista antes de fazer a compra... se não, cria novas categorias e produtos... espetáculo.....
                    foreach (ProdQuant pq in ((Carrinho)Session["Carrinho"]).Produtos)
                    {

                        // Dizer á framework que isto não mudou os dados....
                        db.Entry((pq.Produto.Categoria = db.Categorias.Find(pq.Produto.Categoria.Id))).State = EntityState.Unchanged;
                        db.Entry((pq.Produto = db.Produtos.Find(pq.Produto.Id))).State = EntityState.Unchanged;
                        // Adicionar...
                        db.ProdQuants.Add(pq);
                        db.SaveChanges();
                    }

                    Venda v = new Venda(((Carrinho)Session["Carrinho"]).Produtos, c);
                    db.Vendas.Add(v);
                    db.SaveChanges();
                    Produto.AtualizarStock((Carrinho)Session["Carrinho"]);
                    Session["Carrinho"] = new Carrinho();
                    return RedirectToAction("Carrinho");
                }
            }

            return RedirectToAction("Index");
        }
    }
}