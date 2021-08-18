using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleMysql.Mysql mysql = new SimpleMysql.Mysql("10.34.11.45", "worktable", "skill", "skill");
            var query = "select  module,refdes from ( select * from worktable.netlist_module a join ( SELECT replace(Predecessor,'_','-') Predecessor, modulename, CONCAT(modulename,'(',replace(Predecessor,'_','-'),')') Premodulename FROM worktable.eedm_placement where pcbno=substring_index('213126','-',1) and predecessor <> 'n/a' and Template = ( select template from worktable.eedm_placement where pcbno=substring_index('213126','-',1) order by createdate desc LIMIT 1) group by Predecessor,modulename,Premodulename ) b on a.no=b.Predecessor and a.module=replace(b.modulename,'_','-') ) c;";
            //query = "SELECT * FROM worktable.auto_routing  limit 20;";
            DataTable result = mysql.Query(query);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("D:/temp/test.txt"))
            {
                foreach (DataRow row in result.Rows)
                {
                    file.WriteLine(string.Join("|", row.ItemArray));
                }
            }
            mysql.Dispose();


        }
    }
}
