
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateBase.Models;

namespace DateBase
{
    public class DBConnector
    {
        public UserContext db = new UserContext();
        public List<Goods> GetGoods()
        {
            return db.Goods.ToList();
        }
        public List<User> GetUsers()
        {
            return db.Users.ToList();
        }
        public void AddSell(Sell sell)
        {
            db.Sells.Add(sell);
            Goods good = db.Goods.Where(s => s.Id == sell.Product_Id).FirstOrDefault();
            good.Count -= sell.Quantity;
            db.SaveChanges();
        }

        public List<GoodItem> GoodsToGoodItems(List<Goods> g)
        {
            List<GoodItem> gItems = new List<GoodItem>();
            foreach (Goods g2 in g)
            {
                GoodItem goodItem = new GoodItem();
                goodItem.Id = g2.Id;
                goodItem.ProductCode = g2.ProductCode;
                goodItem.Quantity = 1;
                goodItem.Price = g2.Price;
                goodItem.Amount = g2.Price;
                goodItem.Count = g2.Count;
                goodItem.Name = g2.Name;
                goodItem.Discount = 0;
                gItems.Add(goodItem);
            }
            return gItems;
        }
        public List<Goods> GoodItemsToGoods(List<GoodItem> g)
        {
            List<Goods> gItems = new List<Goods>();
            foreach (GoodItem g2 in g)
            {
                Goods goodItem = new Goods();
                goodItem.Id = g2.Id;
                goodItem.ProductCode = g2.ProductCode;
                goodItem.Price = g2.Price;
                goodItem.Count = g2.Count;
                goodItem.Name = g2.Name;
                gItems.Add(goodItem);
            }
            return gItems;
        }
        public GoodItem GoodToGoodItem(Goods g)
        {

            GoodItem goodItem = new GoodItem();
            goodItem.Id = g.Id;
            goodItem.ProductCode = g.ProductCode;
            goodItem.Quantity = 1;
            goodItem.Price = g.Price;
            goodItem.Amount = g.Price;
            goodItem.Name = g.Name;
            goodItem.Count = g.Count;
            goodItem.Discount = 0;

            return goodItem;
        }
        public void AddGood(Goods goods)
        {
            db.Goods.Add(goods);
            db.SaveChanges();
        }
        public void UpdateGoods(List<Goods> goods)
        {
            foreach(Goods g in goods)
            {
                db.Goods.Update(g);
                db.SaveChanges();
            }
            
        }

        public void UpdateGood(Goods good)
        {
            db.Goods.Update(good);
            db.SaveChanges();
        }
        public void DeleteGood(Goods goods)
        {
            db.Goods.Remove(goods);
            db.SaveChanges();
        }
        public void AddDelievery(Delievery delievery)
        {
            db.Delieveries.Add(delievery);
            db.SaveChanges();
        }
        public void UpdateDelieveries(List<Delievery> delievery)
        {
            foreach (Delievery g in delievery)
            {
                db.Delieveries.Update(g);
                db.SaveChanges();
            }
        }
        public void UpdateDelievery(Delievery delievery)
        {
            db.Delieveries.Update(delievery);
            db.SaveChanges();
        }
        public void DeleteDelievery(Delievery delievery)
        {
            db.Delieveries.Remove(delievery);
            db.SaveChanges();
        }
        public List<Delievery> GetDelieveries()
        {
            return db.Delieveries.ToList();
        }
        public List<Sell> GetSells()
        {
            return db.Sells.ToList();
        }
        public void UpdateUser(User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
        }
        public void AddUser(User user)
        {
            db.Add(user);
            db.SaveChanges();
        }
        public void DeleteUser(User user)
        {
            db.Remove(user);
            db.SaveChanges();
        }
    }
}
