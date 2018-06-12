using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_Management_System
{
    public class LibraryUI
    {
        public static void AddNewMenuAndDisable(MenuStrip menuStrip1)
        {
            // bindding menu            
            List<ToolStripMenuItem> allItems = new List<ToolStripMenuItem>();
            using (SSLsEntities db = new SSLsEntities())
            {
                //int i = 1;
                foreach (ToolStripMenuItem toolItem in menuStrip1.Items)
                {
                    allItems.Add(toolItem);
                    //add sub items
                    allItems.AddRange(GetItems(toolItem));
                }
                //foreach (var item in allItems)
                //{
                //    item.Enabled = false;
                //}
                var menus = Singleton.SingletonMenu.Instance().Menu;
                int i = menus.Count() + 1;
                // จัดการ menus ที่ add ลง แล้ว ก็ข้ามไป
                foreach (var item in allItems)
                {
                    Console.WriteLine(item.Name + " " + item.Text);
                    if (menus.FirstOrDefault(w => w.Enable == true && w.Name == item.Name) != null)
                    {
                        continue;
                    }
                    Menu m = new Menu();
                    m.Code = i + "";
                    m.CreateBy = "admin";
                    m.CreateDate = DateTime.Now;
                    m.UpdateDate = DateTime.Now;
                    m.UpdateBy = "admin";
                    m.Name = item.Name;
                    m.Description = item.Text;
                    m.Enable = true;
                    i++;
                    db.Menu.Add(m);
                }
                db.SaveChanges();
                //foreach (var item in db.Menu)
                //{
                //    MenuAccess ms = new MenuAccess();
                //    ms.CreateBy = "admin";
                //    ms.CreateDate = DateTime.Now;
                //    ms.Description = "admin able";
                //    ms.Enable = true;
                //    ms.FKMenu = item.Id;
                //    ms.UpdateDate = DateTime.Now;
                //    ms.UpdateBy = "admin";
                //    ms.FKRole = 1;
                //    db.MenuAccess.Add(ms);
                //}
                //db.SaveChanges();

                //var menus = SingletonAuthen.Instance().Users.Role.MenuAccess.Where(w => w.Enable == true).ToList();
                //foreach (var item in menus)
                //{
                //    var menu = allItems.SingleOrDefault(w => w.Name == item.Menu.Name);
                //    if (menu != null)
                //    {
                //        menu.Enabled = true;
                //    }
                //}
            }
        }
        private static IEnumerable<ToolStripMenuItem> GetItems(ToolStripMenuItem item)
        {
            foreach (ToolStripMenuItem dropDownItem in item.DropDownItems)
            {
                if (dropDownItem.HasDropDownItems)
                {
                    foreach (ToolStripMenuItem subItem in GetItems(dropDownItem))
                        yield return subItem;
                }
                yield return dropDownItem;
            }
        }
    }
}
