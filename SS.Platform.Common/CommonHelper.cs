using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Microsoft.International.Converters.PinYinConverter;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SS.Platform.Common
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 生成包含字母、数字的验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns>验证码字符串</returns>
        public static string CreateValidateCodeContainsLetter(int length)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(33);

                while (temp == t)
                {
                    t = rand.Next(33);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
        /// <summary>
        /// 生成数字验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns>验证码字符串</returns>
        public static string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        /// <summary>
        /// 创建验证码的图片，把验证码写到响应流里面去。
        /// </summary>
        /// <param name="validateCode">验证码</param>
        /// <param name="context">要输出到的page对象</param>
        public static void CreateValidateGraphic(string validateCode, HttpContext context)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 15.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                context.Response.Clear();//把之前Response的字节数组清空。
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(stream.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        /// <summary>
        /// 得到验证码图片的长度
        /// </summary>
        /// <param name="validateNumLength">验证码的长度</param>
        /// <returns></returns>
        public static int GetImageWidth(int validateNumLength)
        {
            return (int)(validateNumLength * 12.0);
        }
        /// <summary>
        /// 得到验证码的高度
        /// </summary>
        /// <returns></returns>
        public static double GetImageHeight()
        {
            return 22.5;
        }

        /// <summary>
        /// 生成图片缩略图
        /// </summary>
        /// <param name="originalImagePath">原图片文件路径</param>
        /// <param name="thumbnailPath">缩略图文件路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">缩略图缩放方式</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
             new Rectangle(x, y, ow, oh),
             GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        /// <summary>
        /// 根据给定参数和模板文件，产生html文件
        /// </summary>
        /// <param name="templateName">模板文件名</param>
        /// <param name="data">参数数据</param>
        /// <returns></returns>
        public static string RenderHtml(string templateName, object data)
        {
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/templates"));//模板文件所在的文件夹
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("Model", data);
            Template vltTemplate = vltEngine.GetTemplate(templateName);
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            return html;
        }

        /// <summary>
        /// 获取密文字符串
        /// </summary>
        /// <param name="strInput">输入明文字符串</param>
        /// <returns>密文字符串</returns>
        public static string GetCipherText(string strInput)
        {
            return GetMd5(GetMd5(",./" + strInput + "3.1415921"));
        }

        /// <summary>
        /// 获取字符串MD5值
        /// </summary>
        /// <param name="strIn">输入字符串</param>
        /// <returns>输入字符串MD5值</returns>
        public static string GetMd5(string strIn)
        {
            //1.创建一个用来计算MD5值的类的对象//把字符串转换为byte[]
            //注意：如果字符串中包含汉字，则这里会把汉字使用utf-8编码转换为byte[]，当其他地方
            //计算MD5值的时候，如果对汉字使用了不同的编码，则同样的汉字生成的byte[]是不一样的，所以计算出的MD5值也就不一样了。
            //这里只要保证编码和解码的编码一致就可以了
            using (MD5 md5 = MD5.Create())
            {
                byte[] bufInput = System.Text.Encoding.UTF8.GetBytes(strIn);
                //2.计算给定字符串的MD5值
                //返回值就是就算后的MD5值,如何
                //把一个长度为16的byte[]数组转换为一个长度为32的字符串：
                //就是把每个byte转成16进制同时保留2位即可。
                byte[] bufMd5 = md5.ComputeHash(bufInput);
                //由于使用了using，里面的资源可以自动释放
                //所以此句可省略
                md5.Clear();

                StringBuilder sbMd5 = new StringBuilder();
                for (int i = 0; i < bufMd5.Length; i++)
                {
                    sbMd5.Append(bufMd5[i].ToString("x2"));
                }
                return sbMd5.ToString();
            }
        }
        /// <summary>
        /// 获取参数路径下的文件的MD5值
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件的MD5值</returns>
        public static string GetMd5FromFile(string path)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream fsRead = File.OpenRead(path))
                {
                    byte[] bytes = md5.ComputeHash(fsRead);
                    //由于使用了using，里面的资源可以自动释放
                    //所以此句可省略
                    md5.Clear();
                    StringBuilder sbMd5 = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        //转换为16进制并保留两位
                        sbMd5.Append(bytes[i].ToString("X2"));
                    }
                    return sbMd5.ToString();
                }
            }
        }
        //把SQL语句的查询结果到Excel文件
        /// <summary>
        /// 把制定SQL语句的执行结果导出到制定的Excel文件中 
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="sheetName">工作表名</param>
        public static void sqlToExcel(string sql, string filePath, string fileName, string sheetName)
        {
            using (SqlDataReader reader = SqlHelper.SelectReader(sql, CommandType.Text))
            {
                if (reader.HasRows)
                {
                    //创建Excel文件
                    IWorkbook wk = new HSSFWorkbook();
                    //创建工作表
                    ISheet sheet = wk.CreateSheet(sheetName);

                    int rowIndex = 0;
                    #region 创建表头
                    IRow rowHead = sheet.CreateRow(rowIndex++);
                    for (int j = 0; j < reader.FieldCount; j++)
                    {
                        ICell celllHead = rowHead.CreateCell(j);
                        celllHead.SetCellValue(reader.GetName(j));
                    }

                    #endregion
                    #region 读取并创建每一行
                    //读取每一条数据
                    while (reader.Read())
                    {
                        IRow row = sheet.CreateRow(rowIndex++);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            String strFieldType = reader.GetDataTypeName(i);
                            //row.CreateCell(i).SetCellValue(;
                            switch (strFieldType)
                            {
                                case "int":
                                    createIntCell(reader, row, i);
                                    break;
                                case "varchar":
                                case "nvarchar":
                                    createStringCell(reader, row, i);
                                    break;
                                case "datetime":
                                    createDateTimeCell(reader, row, i, wk);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    #endregion

                    //将Excel写入文件
                    using (FileStream fsWrite = File.OpenWrite(filePath + fileName))
                    {
                        wk.Write(fsWrite);
                    }
                }
            }
        }
        private static void createIntCell(SqlDataReader reader, IRow row, int i)
        {
            int? intCol = reader.IsDBNull(i) ? null : (int?)reader.GetInt32(i);
            ICell cell = row.CreateCell(i);
            if (intCol == null)
            {
                cell.SetCellType(CellType.BLANK);
            }
            else
            {
                cell.SetCellValue((int)intCol);
            }
        }
        private static void createStringCell(SqlDataReader reader, IRow row, int i)
        {
            string strCol = reader.GetString(i);
            ICell cell = row.CreateCell(i);
            cell.SetCellValue(strCol);
        }
        private static void createDateTimeCell(SqlDataReader reader, IRow row, int i, IWorkbook wk)
        {
            DateTime? dtCol = reader.IsDBNull(i) ? null : (DateTime?)reader.GetDateTime(i);
            ICell cell = row.CreateCell(i);
            if (dtCol == null)
            {
                //设置单元格的数据类型为Blank，表示空单元格
                cell.SetCellType(CellType.BLANK);
            }
            else
            {
                cell.SetCellValue((DateTime)dtCol);//创建一个单元格格式对象
                ICellStyle cellStyle = wk.CreateCellStyle();
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("m/d/yy h:mm");
                //设置当前日期这个单元格的是CellStyle属性
                cell.CellStyle = cellStyle;
            }
        }
        /// <summary>
        /// 取得汉字的全拼
        /// </summary>
        /// <param name="strSr">要获得拼音的汉字</param>
        /// <returns>该汉字的全拼</returns>
        public static string getPinyin(string strSr)
        {
            StringBuilder sbPy = new StringBuilder();
            //遍历用户输入的字符串中的每个char
            foreach (char item in strSr)//遍历每个汉字
            {
                if (ChineseChar.IsValidChar(item))
                {
                    ChineseChar cr = new ChineseChar(item);
                    sbPy.Append(cr.Pinyins[0].Substring(0, cr.Pinyins[0].Length - 1));
                }
            }
            return sbPy.ToString().ToLower();
        }
        /// <summary>
        /// 取得汉字的拼音首字母
        /// </summary>
        /// <param name="strSr">要获得拼音的汉字</param>
        /// <returns>该汉字拼音首字母</returns>
        public static string getFirstPinyin(string strSr)
        {
            StringBuilder sbPy = new StringBuilder();
            //遍历用户输入的字符串中的每个char
            foreach (char item in strSr)//遍历每个汉字
            {
                if (ChineseChar.IsValidChar(item))
                {
                    ChineseChar cr = new ChineseChar(item);
                    //sbPy.Append(cr.Pinyins[0].Substring(0, cr.Pinyins[0].Length - 1));
                    string strPinyin = cr.Pinyins[0];
                    char cPy = strPinyin[0];
                    sbPy.Append(cPy);
                }
            }
            return sbPy.ToString().ToLower();
        }
        /// <summary>
        /// 由简体中文转换繁体中文
        /// </summary>
        /// <param name="strSc">简体中文字符串</param>
        /// <returns>繁体中文字符串</returns>
        public static string getTradChineseFromSimpChinese(string strSc)
        {
            return ChineseConverter.Convert(strSc, ChineseConversionDirection.SimplifiedToTraditional);
        }
        /// <summary>
        /// 由繁体中文转换简体中文
        /// </summary>
        /// <param name="strSc">繁体中文字符串</param>
        /// <returns>简体中文字符串</returns>
        public static string getSimpChineseFromTradChinese(string strSc)
        {
            return ChineseConverter.Convert(strSc, ChineseConversionDirection.TraditionalToSimplified);
        }
    }
}
