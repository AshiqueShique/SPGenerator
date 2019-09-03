using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GenerateStoredProcedure
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SqlConnection conn;
        public MainWindow()
        {
            InitializeComponent();
            BtnCpoy.Visibility = Visibility.Hidden;
            Insert.IsChecked = true;
        }

        private void tbMultiLine_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            if (tbMultiLine.Text != "")
            {
                BtnCpoy.Visibility = Visibility.Visible;
            }
            else
            {
                BtnCpoy.Visibility = Visibility.Hidden;
            }
        }
        private SqlConnection connect()
        {
            conn = new SqlConnection();



            conn.ConnectionString = "Server=" + Server.Text + ";Database=" + DB.Text + ";User ID=" + UserName.Text + ";Password=" + Password.Password + ";Trusted_Connection=true;Integrated Security=false";
            return conn;

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try {
                string Mode=Insert.IsChecked==true?"1":"2";
                connect();
                conn.Open();
                string Q = "SET NOCOUNT ON;                                                                                                                                                           \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "declare @TableName varchar(5000)='"+Table.Text+"'                                                                                                                                                  \r\n"
+ "declare @ProcName varchar(5000)='"+"USP_Manage_"+Table.Text+"'                                                                                                                                            \r\n"
+ "declare @Mode int =1                                                                                                                                                                               \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "declare @SLNO int =1                                                                                                                                                                               \r\n"
+ "declare @Print varchar(max)='-- =============================================                                                                                                                      \r\n"
+ "-- Author:	Ashique C                                                                                                                                                                        \r\n"
+ "-- Contact:	ashikpadannotte@gmail.com                                                                                                                                                                        \r\n"
+ "-- Create date: <Create Date,,>                                                                                                                                                                    \r\n"
+ "-- Description:	<Description,,>                                                                                                                                                                   \r\n"
+ "-- =============================================                                                                                                                                                   \r\n"
+ "CREATE PROCEDURE '+@ProcName+char(10)                                                                                                                                                              \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) as SLNO,                                                                                                                                             \r\n"
+ "c.name 'Column Name',                                                                                                                                                                              \r\n"
+ "t.Name 'Data type'                                                                                                                                                                                 \r\n"
+ "into #TEMP                                                                                                                                                                                         \r\n"
+ "FROM                                                                                                                                                                                               \r\n"
+ "sys.columns c                                                                                                                                                                                      \r\n"
+ "INNER JOIN                                                                                                                                                                                         \r\n"
+ "sys.types t ON c.user_type_id = t.user_type_id                                                                                                                                                     \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "WHERE                                                                                                                                                                                              \r\n"
+ "c.object_id = OBJECT_ID(@TableName)                                                                                                                                                                \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "--select distinct [Data type] from #TEMP                                                                                                                                                           \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "while (@SLNO <= (select max(SLNO) from #TEMP))                                                                                                                                                     \r\n"
+ "BEGIN                                                                                                                                                                                              \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Print=@Print + '@'+(select [Column Name] from #TEMP where SLNO=@SLNO)+'	' +                                                                                                                   \r\n"
+ "case                                                                                                                                                                                               \r\n"
+ "when (select [Data type] from #TEMP where SLNO=@SLNO)='int' then 'int'                                                                                                                             \r\n"
+ "when (select [Data type] from #TEMP where SLNO=@SLNO)='varchar' then 'varchar(max)'                                                                                                                \r\n"
+ "when (select [Data type] from #TEMP where SLNO=@SLNO)='datetime' then 'datetime'                                                                                                                   \r\n"
+ "when (select [Data type] from #TEMP where SLNO=@SLNO)='numeric' then 'numeric(18,2)'                                                                                                               \r\n"
+ "when (select [Data type] from #TEMP where SLNO=@SLNO)='nvarchar' then 'varchar(max)'                                                                                                               \r\n"
+ "when (select [Data type] from #TEMP where SLNO=@SLNO)='decimal' then 'numeric(18,2)'                                                                                                               \r\n"
+ "else '' end + ',' + char(10)                                                                                                                                                                       \r\n"
+ "set @SLNO=@SLNO+1                                                                                                                                                                                  \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "END                                                                                                                                                                                                \r\n"
+ "set @Print=@Print+'@Mode int'+char(10)+'AS'+char(10)+'BEGIN'+char(10)                                                                                                                              \r\n"
+ "++'SET NOCOUNT ON;'+char(10)+'if(@Mode=1)'+char(10)+'BEGIN' +char(10)+'set @'+(select [Column Name] from #TEMP where SLNO=1)+'=(select isnull(Max('+(select [Column Name] from #TEMP where SLNO=1)+'),0)+1 from '+@TableName+')'    + char(10)                                                                                                                                                                                                                                                                            \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "declare @Additions varchar(max)                                                                                                                                                                    \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Additions='Insert into '+@TableName +' ( '+char(10)                                                                                                                                           \r\n"
+ "set @SLNO=1                                                                                                                                                                                        \r\n"
+ "while (@SLNO <= (select max(SLNO) from #TEMP))                                                                                                                                                     \r\n"
+ "BEGIN                                                                                                                                                                                              \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Additions=@Additions +(select [Column Name] from #TEMP where SLNO=@SLNO)                                                                                                                      \r\n"
+ "+ case when (select max(SLNO) from #TEMP)=@SLNO then '' else ',' end+ char(10)                                                                                                                     \r\n"
+ "set @SLNO=@SLNO+1                                                                                                                                                                                  \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "END                                                                                                                                                                                                \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Additions=@Additions+')'+char(10) +'values ('                                                                                                                                                 \r\n"
+ "set @SLNO=1                                                                                                                                                                                        \r\n"
+ "while (@SLNO <= (select max(SLNO) from #TEMP))                                                                                                                                                     \r\n"
+ "BEGIN                                                                                                                                                                                              \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Additions=@Additions +'@'+(select [Column Name] from #TEMP where SLNO=@SLNO)                                                                                                                  \r\n"
+ "+ case when (select max(SLNO) from #TEMP)=@SLNO then '' else ',' end+ char(10)                                                                                                                     \r\n"
+ "set @SLNO=@SLNO+1                                                                                                                                                                                  \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "END                                                                                                                                                                                                \r\n"
+ "set @Additions=@Additions+' )'+char(10)+'END'                                                                                                                                                      \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "declare @Additions2 varchar(max)='ELSE'+char(10)+'BEGIN'                                                                                                                                           \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Additions2='if(@Mode=2)'+char(10)+'BEGIN'+char(10)+'UPDATE '+@TableName +' SET '+char(10)                                                                                                     \r\n"
+ "set @SLNO=1                                                                                                                                                                                        \r\n"
+ "while (@SLNO <= (select max(SLNO) from #TEMP))                                                                                                                                                     \r\n"
+ "BEGIN                                                                                                                                                                                              \r\n"
+ "set @Additions2=@Additions2 +(select [Column Name] from #TEMP where SLNO=@SLNO) +'=	'++'@'+(select [Column Name] from #TEMP where SLNO=@SLNO)                                                     \r\n"
+ "+ case when (select max(SLNO) from #TEMP)=@SLNO then '' else ',' end+ char(10)                                                                                                                     \r\n"
+ "set @SLNO=@SLNO+1                                                                                                                                                                                  \r\n"
+ "END                                                                                                                                                                                                \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Additions2=@Additions2+'where '+(select [Column Name] from #TEMP where SLNO=1)+'='+'@'+(select [Column Name] from #TEMP where SLNO=1)+char(10)+'END'                                          \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "declare @Additions3 varchar(max)                                                                                                                                                                   \r\n"
+ "set @Additions3='ELSE'+char(10)+'BEGIN'+char(10)+'DELETE FROM '+@TableName+' WHERE '+(select top 1 [Column Name] from #TEMP)+' = '+'@'+(select top 1 [Column Name] from #TEMP)+char(10)+'END'      \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "set @Print=@print+char(10)+@Additions+char(10)+@Additions2+char(10)+@Additions3+char(10)+'END'                                                                                                     \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "select @Print     as Result                                                                                                                                                                                  \r\n"
+ "                                                                                                                                                                                                   \r\n"
+ "drop table #TEMP";                                                                                                                                                                                   
SqlCommand cmd = new SqlCommand(Q, conn);                                                                                                                                                               
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
               SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {


                    tbMultiLine.Text = reader["Result"].ToString();
                }
                conn.Close();
                
            }
            catch (Exception ae) { MessageBox.Show(ae.Message); }
        }

        private void BtnCpoy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(tbMultiLine.Text);
            MessageBox.Show("Copied");
        }
    }
}
