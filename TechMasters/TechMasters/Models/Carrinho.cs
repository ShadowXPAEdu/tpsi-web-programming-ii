using System.Collections.Generic;
using System.Linq;

namespace TrabalhoPartico.Models
{
    public class Carrinho
    {
        public List<ProdQuant> Produtos { get; set; }

        public void AddProduto(Produto prod, int quant)
        {
            if (prod != null && quant > 0)
            {
                ProdQuant temp;
                if ((temp = Produtos.Where(p => p.Produto.Id == prod.Id).FirstOrDefault()) != null)
                {
                    if ((temp.Quantidade + quant) <= prod.Stock)
                        temp.Quantidade += quant;
                }
                else
                {
                    if (quant <= prod.Stock)
                        Produtos.Add(new ProdQuant(prod, quant));
                }
            }
        }

        public void AddProdEnc(Produto prod, int quant)
        {
            if (prod != null && quant > 0)
            {
                ProdQuant temp;
                if ((temp = Produtos.Where(p => p.Produto.Id == prod.Id).FirstOrDefault()) != null)
                {
                    temp.Quantidade += quant;
                }
                else
                {
                    Produtos.Add(new ProdQuant(prod, quant));
                }
            }
        }

        public void RemProdEnc(Produto prod, int quant)
        {
            if (prod != null && quant > 0)
            {
                ProdQuant temp;
                if ((temp = Produtos.Where(p => p.Produto.Id == prod.Id).FirstOrDefault()) != null)
                {
                    if (temp.Quantidade <= quant)
                    {
                        Produtos.Remove(temp);
                    }
                    else
                    {
                        temp.Quantidade -= quant;
                    }
                }
            }
        } 

        public void RemoveProduto(Produto prod, int quant)
        {
            if (prod != null && quant > 0)
            {
                ProdQuant temp;
                if ((temp = Produtos.Where(p => p.Produto.Id == prod.Id).FirstOrDefault()) != null)
                {
                    if (temp.Quantidade <= quant)
                    {
                        Produtos.Remove(temp);
                    }
                    else
                    {
                        temp.Quantidade -= quant;
                    }
                }
            }
        }

        public void RemoveProduto(Produto prod)
        {
            if (prod != null)
            {
                ProdQuant temp;
                if ((temp = Produtos.Where(p => p.Produto.Id == prod.Id).FirstOrDefault()) != null)
                {
                    Produtos.Remove(temp);
                }
            }
        }

        public double GetPrice()
        {
            double acum = 0;
            foreach (ProdQuant p in Produtos)
            {
                acum += (p.Produto.Preco * (1 - p.Produto.Desconto)) * p.Quantidade;
            }
            return acum;
        }

        public double GetPriceEnc()
        {
            // Preço original do produto é 15% menos que o preço que a que empresa TechMasters vende
            // 0.15
            double precoOriginal = 1 - 0.15;
            double acum = 0;
            foreach (ProdQuant p in Produtos)
            {
                acum += p.Produto.Preco * (1 - p.Produto.Desconto) * precoOriginal * p.Quantidade;
            }
            return acum;
        }

        public int GetNumProd()
        {
            int num = 0;
            foreach (ProdQuant p in Produtos)
            {
                num += p.Quantidade;
            }

            return num;
        }

        public Carrinho()
        {
            this.Produtos = new List<ProdQuant>();
        }

        /*
        public void AddProduto(Produto produto, int quant)
        {
            if (quant > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                if (!db.Carrinhos.FirstOrDefault().Produtos.Contains(produto))
                {
                    if (db.Produtos.Where(a => a.id == produto.Id).FirstOrDefault().stock >= quant)
                    {
                        db.carrinho.FirstOrDefault().produtos.Add(produto);
                        db.SaveChanges();
                        db.carrinho.FirstOrDefault().quantidade.Add(quant);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (db.Produtos.Where(a => a.id == produto.Id).FirstOrDefault().stock >= quant)
                    {
                        int index = db.carrinho.FirstOrDefault().produtos.IndexOf(produto);
                        db.carrinho.FirstOrDefault().quantidade.Insert(index, quantidade[index] + quant);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void RemoverProduto(Produto produto)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int index = db.carrinho.FirstOrDefault().produtos.IndexOf(produto);
            db.carrinho.FirstOrDefault().quantidade.Remove(quantidade[index]);
            db.SaveChanges();
            db.carrinho.FirstOrDefault().produtos.Remove(produto);
            db.SaveChanges();
        }

        public void RemoverProduto(Produto produto, int quant)
        {
            if (quant > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                int index = db.carrinho.FirstOrDefault().produtos.IndexOf(produto);
                int oldQuant = db.carrinho.FirstOrDefault().quantidade[index];
                if (oldQuant - quant > 0)
                {
                    db.carrinho.FirstOrDefault().quantidade.Insert(index, oldQuant - quant);
                    db.SaveChanges();
                }
                else
                {
                    RemoverProduto(produto);
                }
            }
        }

        public double GetPrice()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            double acum = 0.0d;
            foreach (Produto pr in db.carrinho.FirstOrDefault().produtos)
            {
                acum += pr.Preco;
            }
            return acum;
        }

        public int GetNumProds()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.carrinho.FirstOrDefault().produtos.Count;
        }

        public Carrinho()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            this.Id = db.ids.Where(a => a.nome == "Carrinho").FirstOrDefault().idNum++;
            try
            {
                this.Produtos = db.carrinho.Where(a => a.id == this.Id).FirstOrDefault().produtos;
            } catch
            {
                this.Produtos = new List<Produto>();
            }
            try
            {
                this.quantidade = db.carrinho.Where(a => a.id == this.Id).FirstOrDefault().quantidade;
            } catch
            {
                this.quantidade = new List<int>();
            }
        }
        */
    }
}