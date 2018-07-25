using GrapeCity.Enterprise.Data.DataSource.Excel;

//using System.Data.CData.Excel;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start ....");


            using (ExcelConnection connection = new ExcelConnection(GetConnectionString()))
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                connection.Open();
                stopwatch.Stop();
                Console.WriteLine("Total cost1: " + stopwatch.ElapsedMilliseconds + "ms");

                //var dataTable = connection.GetSchema("Columns");


                //DataTable schemalTable = connection.GetSchema("Columns");
                //Assert.Equal(3, schemalTable.Columns.Count);
                //Assert.Equal("CollectionName", schemalTable.Columns[0].ColumnName);
                //Assert.Equal("NumberOfRestrictions", schemalTable.Columns[1].ColumnName);
                //Assert.Equal("NumberOfIdentifierParts", schemalTable.Columns[2].ColumnName);

                //Assert.Equal("MetaDataCollections", schemalTable.Rows[0][0]);
                //Assert.Equal("Tables", schemalTable.Rows[1][0]);
                //Assert.Equal("Columns", schemalTable.Rows[2][0]);
                //Assert.Equal("DataTypes", schemalTable.Rows[3][0]);


                stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                //string queryString = "select [2015/06/05], [2017/12/31] from Sheet2";
                string queryString = "SELECT[T0].[2015/06/05] AS[T0C0], [T0].[2017/12/31] AS[T0C1] FROM [Sheet2] [T0] WHERE [T0].[2015/06/05] LIKE '%n%' LIMIT 100";

                //string queryString = "SELECT [T0].[Id] AS [T0C0], [T0].[product] AS [T0C1], [T0].[price] AS [T0C2], [T0].[quantity] AS [T0C3], [T0].[amount] AS [T0C4], [T0].[region] AS [T0C5], [T0].[date] AS [T0C6], [T0].[employee] AS [T0C7], [T0].[Department] AS [T0C8], [T0].[customer] AS [T0C9], [T0].[platform] AS [T0C10], [T0].[payment] AS [T0C11], [T0].[Language] AS [T0C12], [T0].[IsLeader] AS [T0C13]  FROM [Sheet1] [T0]  WHERE [T0].[product] LIKE 'Spread%'";
                //string queryString = "SELECT [T0].[年月] AS [T0C0], [T0].[投資先コード] AS [T0C1], [T0].[投資先] AS [T0C2], [T0].[略称] AS [T0C3], [T0].[投資担当] AS [T0C4], [T0].[カテゴリー] AS [T0C5], [T0].[ステージ] AS [T0C6], [T0].[売上計画] AS [T0C7], [T0].[売上実績] AS [T0C8], [T0].[売上達成率] AS [T0C9], [T0].[売上計画\n（直近12ヵ月累計）] AS [T0C10], [T0].[売上実績\n（直近12ヵ月累計）] AS [T0C11], [T0].[売上達成率\n（直近12ヵ月）] AS [T0C12], [T0].[売上計画\n（会計年度）] AS [T0C13], [T0].[売上実績\n（会計年度）] AS [T0C14], [T0].[売上達成率\n（会計年度）] AS [T0C15], [T0].[当初売上計画\n（単月）] AS [T0C16], [T0].[当初売上達成率] AS [T0C17], [T0].[当初売上計画\n（会計年度）] AS [T0C18], [T0].[当初売上達成率\n（会計年度）] AS [T0C19], [T0].[営利計画] AS [T0C20], [T0].[営利実績] AS [T0C21], [T0].[営利乖離額] AS [T0C22], [T0].[営利計画\n（直近12ヵ月累計）] AS [T0C23], [T0].[営利実績\n（直近12ヵ月累計）] AS [T0C24], [T0].[営利達成率\n（直近12ヵ月）] AS [T0C25], [T0].[営利乖離額\n（直近12ヵ月累計）] AS [T0C26], [T0].[営利計画\n（会計年度）] AS [T0C27], [T0].[営利実績\n（会計年度）] AS [T0C28], [T0].[営利達成率\n（会計年度）] AS [T0C29], [T0].[営利乖離額\n（会計年度）] AS [T0C30], [T0].[当初営利計画\n（単月）] AS [T0C31], [T0].[当初営利達成率] AS [T0C32], [T0].[当初営利計画\n（会計年度）] AS [T0C33], [T0].[当初営利達成率\n（会計年度）] AS [T0C34], [T0].[投資額] AS [T0C35], [T0].[一部売却後投資額] AS [T0C36], [T0].[持分比率] AS [T0C37], [T0].[1株単価] AS [T0C38], [T0].[総株式数\n（顕在）] AS [T0C39], [T0].[持ち株数] AS [T0C40], [T0].[時価総額] AS [T0C41], [T0].[出資時_時価総額] AS [T0C42], [T0].[増加_時価総額] AS [T0C43], [T0].[持分_時価総額] AS [T0C44], [T0].[未実現利益] AS [T0C45], [T0].[現預金] AS [T0C46], [T0].[純資産] AS [T0C47], [T0].[持分純資産] AS [T0C48], [T0].[増減率] AS [T0C49], [T0].[最大減損額] AS [T0C50], [T0].[投資額（売却分）] AS [T0C51], [T0].[売却（清算）額] AS [T0C52], [T0].[損益] AS [T0C53], [T0].[現在簿価] AS [T0C54], [T0].[減損計上額] AS [T0C55], [T0].[想定IRR計算\n_日付] AS [T0C56], [T0].[想定IRR計算\n_投資額] AS [T0C57], [T0].[想定IRR] AS [T0C58], [T0].[確定IRR計算\n_日付] AS [T0C59], [T0].[確定IRR計算\n_投資額] AS [T0C60], [T0].[確定IRR] AS [T0C61], [T0].[トータル\n想定IRR] AS [T0C62], [T0].[トータル\n確定IRR] AS [T0C63]  FROM [00_統合用] [T0]  LIMIT 100";
                ExcelCommand cmd = new ExcelCommand(queryString, connection);

                //DataTable table = new DataTable("queryTable");
                //ExcelDataAdapter dap = new ExcelDataAdapter(cmd);
                //dap.Fill(table);

                var oleExcelReader = cmd.ExecuteReader();
                int nOutputRow = 0;

                while (oleExcelReader.Read())
                {
                    for (int i = 0; i < oleExcelReader.FieldCount; i++)
                    {
                        object v = oleExcelReader.GetValue(i);
                        //if(v is DateTime)
                        //{
                        //    v = ((DateTime)v).ToString("yyyy/MM/dd HH:mm:ss");
                        //}
                        Console.Write(v + v.GetType().Name);
                        Console.Write(", ");
                    }
                    Console.WriteLine();
                    nOutputRow++;
                }

                stopwatch.Stop();
                Console.WriteLine("Total cost2: " + stopwatch.ElapsedMilliseconds + "ms");
            }


          

            Console.ReadLine();
        }

        private static string GetConnectionString()
        {
            //string file = @"C:\Users\robinhan\Desktop\Tools_Sales_Data_J (FY2013).xlsx";
            //string file = @"E:\Temp\Test-ExcelFile\AzureDB.xlsx";
            //string file = @"E:\Temp\Test-ExcelFile\2013-2013西安环境监测数据.xlsx";
            //string file = @"E:\Temp\Test-ExcelFile\SampleData\HumanResourcesEmployee - Copy.xlsx";
            //string file = @"E:\Temp\Test-ExcelFile\SampleData\HumanResources-SmallData.xlsx";
            //string file = @"E:\Temp\Test-ExcelFile\SampleData\RetailAnalysis - Small.xlsx"; //20W
            //string file = @"E:\Temp\Test-ExcelFile\Sales 2012-201505-Daisy.xlsx";
            //string file = @"C:\Users\robinhan\Desktop\StudentScore.xlsx";
            //string file = @"E:\Temp\Test-ExcelFile\★最新【OV1号F】BIツール集計用.xlsx"; 
            //string file = @"E:\Temp\Test-ExcelFile\XIRR.xlsx";
            string file = @"E:\Temp\Test-ExcelFile\XIRR_1127.xlsx";

            return "Data Source=" + file + ";Row Scan Depth=15;Type Detection Scheme=RowScan";
            //return "Excel File=" + file + ";Row Scan Depth=15;Type Detection Scheme=RowScan";
        }

    }
}
