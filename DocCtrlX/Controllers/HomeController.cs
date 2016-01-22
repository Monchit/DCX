using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using WebCommonFunction;
using DocCtrlX.Models;
using System.IO;

namespace DocCtrlX.Controllers
{
    public class HomeController : Controller
    {
        private TNCSecurity secure = new TNCSecurity();
        DocCtrlXEntities db = new DocCtrlXEntities();
        TNC_ADMINEntities dbTNC = new TNC_ADMINEntities();

        public ActionResult Index()
        {
            return View();
        }

        [Chk_Authen]
        public ActionResult Main()
        {
            return View();
        }

        [Chk_Authen]
        public ActionResult New()
        {
            ViewBag.DocType = db.tm_doc_type.OrderBy(o => o.doc_type_name);
            return View();
        }
        
        public ActionResult Login()
        {
            string username = Request.Form["Username"].ToString();
            string pass = Request.Form["Password"].ToString();
            var chklogin = secure.Login(username, pass, false);//set false to true for Real

            if (chklogin != null)
            {
                Session["DX_Authen"] = chklogin.emp_code; // Employee ID

                var pos_lv = (from lv in db.tm_level
                              where lv.pos_min <= chklogin.position_level && lv.pos_max >= chklogin.position_level
                              select lv).FirstOrDefault();

                if (pos_lv != null)
                {
                    Session["DX_ULv"] = pos_lv.lv_id;
                    if (pos_lv.lv_id <= 2)
                    {
                        Session["DX_Org"] = chklogin.group_id;
                    }
                    else if (pos_lv.lv_id == 3)
                    {
                        Session["DX_Org"] = chklogin.dept_id;
                    }
                    else if (pos_lv.lv_id >= 4)
                    {
                        Session["DX_Org"] = chklogin.plant_id;
                    }
                }

                if (chklogin.emp_lname.Length > 2)
                {
                    Session["DX_Name"] = chklogin.emp_fname + " " + chklogin.emp_lname.Substring(0, 2) + ". (" + pos_lv.lv_name + ")";
                }
                else
                {
                    Session["DX_Name"] = chklogin.emp_fname + " " + chklogin.emp_lname + ". (" + pos_lv.lv_name + ")";
                }

                return RedirectToAction("Main", "Home");
            }
            else
            {
                TempData["noty"] = "Username and/or Password is incorrect !!!";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("DX_Authen");
            Session.Remove("DX_Name");
            Session.Remove("DX_ULv");
            Session.Remove("DX_Org");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult NewDoc(HttpPostedFileBase atfile)
        {
            try
            {
                td_doc doc = new td_doc();
                doc.doc_no = Request.Form["txtDocNo"];
                doc.doc_name = Request.Form["txtDocName"];
                doc.doc_type_id = byte.Parse(Request.Form["selDocType"]);
                doc.cust_no = Request.Form["selCust"];
                doc.change_point = Request.Form["txaChange"];
                doc.doc_rev = 0;
                doc.eff_date = ParseToDate(Request.Form["txtEffDate"].ToString());
                doc.receive_date = ParseToDate(Request.Form["txtRecDate"].ToString());

                DateTime dt = DateTime.Now;
                // Add data to DB TD_File //
                string subPath = "~/files/" + dt.Year + "/" + dt.Month + "/";
                if (!Directory.Exists(Server.MapPath(subPath)))
                    Directory.CreateDirectory(Server.MapPath(subPath));

                if (atfile != null && atfile.ContentLength > 0)
                {
                    var extension = Path.GetExtension(atfile.FileName);
                    var fileName = "" + extension;
                    var path = Path.Combine(Server.MapPath(subPath), fileName);
                    atfile.SaveAs(path);
                    doc.attach = subPath + fileName;
                }

                db.td_doc.Add(doc);

                return RedirectToAction("Main", "Home");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DateTime ParseToDate(string inputDT)
        {
            char[] delimiters = new char[] { '-', '/', ' ' };
            var temp = inputDT.Split(delimiters);
            DateTime outputDT = DateTime.Parse(temp[2] + "-" + temp[1] + "-" + temp[0]);
            return outputDT;
        }

        [HttpGet]
        public ActionResult Selecte2TNCGroup(string searchTerm)
        {
            var group = dbTNC.tnc_group_master.Where(w => w.group_name.Contains(searchTerm))
                .OrderBy(o => o.group_name)
                .Select(s => new { id = s.id, text = s.group_name })
                .ToList();
            return Json(group, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MainList(string doc_no, string doc_name, string cust_no, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                byte lv = byte.Parse(Session["DX_ULv"].ToString());
                int org = int.Parse(Session["DX_Org"].ToString());
                var query = db.td_tran.Where(w => w.action_id == null);

                if (!string.IsNullOrEmpty(doc_no))
                {
                    query = query.Where(w => w.doc_no.ToUpper().Contains(doc_no.ToUpper()));
                }
                if (!string.IsNullOrEmpty(doc_name))
                {
                    query = query.Where(w => w.td_doc.doc_name.ToUpper().Contains(doc_name.ToUpper()));
                }

                //Get data from database
                int TotalRecord = query.Count();

                // Paging
                var output = query
                    .Select(s => new
                    {
                        s.doc_no,
                        s.td_doc.doc_name,
                        s.td_doc.doc_type_id,
                        s.td_doc.cust_no,
                        s.td_doc.change_point,
                        s.td_doc.eff_date,
                        s.act_dt
                    }).OrderBy(jtSorting).Skip(jtStartIndex).Take(jtPageSize);

                //Return result to jTable
                return Json(new { Result = "OK", Records = output, TotalRecordCount = TotalRecord });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
