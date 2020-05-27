using Common;
using Model.Dao;
using Model.EF;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ShopOnline.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private const string CartSession = "CartSession";
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<Caritem>();
            if(cart !=null)
            {
                list = (List<Caritem>)cart;
            }
            return View(list);
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new { status = true });
        }
        public JsonResult Delete(long id)
        {
            var sessioncart = (List < Caritem >) Session[CartSession];
            sessioncart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessioncart;
            return Json(new { status = true });
        }

        public JsonResult Update(string cartModel)
        {
            var jsoncart = new JavaScriptSerializer().Deserialize<List<Caritem>>(cartModel);
            var sessioncart = (List<Caritem>)Session[CartSession];
            foreach(var item in sessioncart)
            {
                var jsonItem = jsoncart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem !=null)
                {
                    item.Quantily = jsonItem.Quantily;
                }
            }
            Session[CartSession] = sessioncart;
            return Json(new { status = true });
        }
        public ActionResult AddItem(long productID, int quantily)
        {
            var product = new ProductDao().ViewDetail(productID);
            var cart = Session[CartSession];
            if(cart !=null )
            {
                
                var list = (List<Caritem>)cart;
                if (list.Exists(x=>x.Product.ID==productID))
                foreach(var item in list)
                {
                    if(item.Product.ID == productID)
                    {
                        item.Quantily += quantily;
                    }
                }
                else
                {
                    var item = new Caritem();
                    item.Product = product;
                    item.Quantily = quantily;
                    list.Add(item);

                }
                Session[CartSession] = list;

            }
            else
            {
                //tao moi doi tuong moi cart item
                var item = new Caritem();
                item.Product = product;
                item.Quantily = quantily;
                var list = new List<Caritem>();
                list.Add(item);
                //gan vao session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<Caritem>();
            if (cart != null)
            {
                list = (List<Caritem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipname,string mobile,string address,string email)
        {
            var order = new Order();
            order.CratedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipEmail = email;
            order.ShipName = shipname;
            Product pro = new Product();
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<Caritem>)Session[CartSession];
                var detaildao = new OrderDetailDao();
                decimal total = 0;
                string sp = "";
                foreach (var item in cart)
                {
                    var orderdetail = new OrderDetail();
                    orderdetail.ProductID = item.Product.ID;
                    orderdetail.OrderID = id;
                    orderdetail.Price = item.Product.Price;
                    orderdetail.Quantily = item.Quantily;
                    detaildao.Insert(orderdetail);
                    sp = string.Concat(sp, item.Product.Name);
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantily);
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));

                    content = content.Replace("{{CustomerName}}", shipname);
                    content = content.Replace("{{Phone}}", mobile);
                    content = content.Replace("{{Email}}", email);
                    content = content.Replace("{{Address}}", address);
                    content = content.Replace("{{Name}}", sp);
            
                    content = content.Replace("{{Total}}", total.ToString("N0"));
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new MailHelper().SendMail(email, "Đơn hàng mới từ ShopOnline", content);
                    new MailHelper().SendMail(toEmail, "Đơn hàng mới từ ShopOnline", content);
                }
            }
            catch (Exception)
            {
                // ghi long
                return Redirect("/loi-hoan-thanh");
            }

            return Redirect("/hoan-thanh");
        }
        public ActionResult Success()
        {
            return View();
        }

    }
}