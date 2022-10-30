using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Formatting;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using SystemReport.WebAPI.Exceptions;
using SystemReport.WebAPI.Helpers;
using SystemReport.WebAPI.Models;
using EResultResponse = SystemReport.WebAPI.Exceptions.EResultResponse;
using FileFormat = Spire.Doc.FileFormat;

namespace SystemReport.WebAPI.Services
{
    public static class SignDigitalService
    {
        public static void GeneratSignKey(ref User model)
        {
            RsaKeyPairGenerator rsaKey = new RsaKeyPairGenerator();
            rsaKey.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKey.GenerateKeyPair();

            RsaKeyParameters Private_Key = (RsaKeyParameters)keyPair.Private;
            RsaKeyParameters Public_Key = (RsaKeyParameters)keyPair.Public;

            model.PrivateKey_string = PrivateKeytoString(Private_Key);
            model.PublicKey_string = PublicKeytoString(Public_Key);
        }

        private static string PublicKeytoString(RsaKeyParameters _key)
        {
            byte[] publicKeyDer = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(_key).GetDerEncoded();
            String publicKeyDerBase64 = Convert.ToBase64String(publicKeyDer);
            return publicKeyDerBase64;
        }

        public static RsaKeyParameters StringtoPrivateKey(string keyString)
        {
            byte[] PrivateKeyDerRestored = Convert.FromBase64String(keyString);
            RsaKeyParameters PrivateKeyRestored = (RsaKeyParameters)PrivateKeyFactory.CreateKey(PrivateKeyDerRestored);
            return PrivateKeyRestored;
        }

        public static string PrivateKeytoString(RsaKeyParameters _key)
        {
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(_key);
            // Write out an RSA private key with it's asscociated information as described in PKCS8.
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            // Convert to Base64 ..
            string serializedPrivateString = Convert.ToBase64String(serializedPrivateBytes);
            return serializedPrivateString;
        }

        public static string ExtractWord(string filepath)
        {
            //Load Document
            Document document = new Document();
            //document.LoadFromFile(@"D:\Doc3.doc");
            document.LoadFromFile(filepath);

            string tempPath = Path.GetDirectoryName(filepath) + @"\toPDF.PDF";
            document.SaveToFile(tempPath, Spire.Doc.FileFormat.PDF);
            return ExtractPDF(tempPath);

            //Initialzie StringBuilder Instance
            // StringBuilder sb = new StringBuilder();
            //
            // sb.Append(document.GetText());

            //Extract Text from Word and Save to StringBuilder Instance
            //foreach (Section section in document.Sections)
            //{
            //    foreach (Paragraph paragraph in section.Paragraphs)
            //    {
            //        sb.AppendLine(paragraph.Text);
            //    }
            //}

            //Create a New TXT File to Save Extracted Text
            //File.WriteAllText(@"D:\TextFromWord.txt", sb.ToString());
            //System.Diagnostics.Process.Start("ExtractText.txt");

            // string str = sb.ToString();
            // str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            // return str;
            //Console.WriteLine("Trich xuat noi Word dung thanh cong!");
        }

        public static string ExtractPDF(string path)
        {
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(path);

            StringBuilder content = new StringBuilder();
            for (int i = 0; i < document.Pages.Count; i++)
            {
                content.Append(document.Pages[i].ExtractText());
            }

            // content.Append(document.Pages[1].ExtractText());

            //String fileName = @"D:\TextFromPDF.txt";
            //File.WriteAllText(fileName, content.ToString());

            string str = content.ToString();
            str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return str;
            //System.Diagnostics.Process.Start("TextFromPDF.txt");

            // Console.WriteLine("Trich xuat noi dung PDF thanh cong!");
        }
        public static RsaKeyParameters StringtoKey(string keyString)
        {
            byte[] publicKeyDerRestored = Convert.FromBase64String(keyString);
            RsaKeyParameters publicKeyRestored = (RsaKeyParameters)PublicKeyFactory.CreateKey(publicKeyDerRestored);
            return publicKeyRestored;

        }
        public static void ValidDsign(string pathPdf, User user)
        {
            //dinh nghia template Dsign
            string StartSignD = "startSignD";
            string EndSignD = "endSignD";


            Console.WriteLine();
            Console.WriteLine("Doc noi dung PDF...");
            string TextFromPDF = SignDigitalService.ExtractPDF(pathPdf);
            Console.WriteLine("noi dung PDF thanh cong");

            //lay vitri ket thuc text
            var indEndText = TextFromPDF.IndexOf(StartSignD);

            // var ind1Sign = TextFromPDF.IndexOf(StartSignD);
            var indEndSign = TextFromPDF.IndexOf(EndSignD);
            if (indEndSign == -1 || indEndText == -1)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Xác thực thất bại với tài khoản: " + user.UserName + ": " + user.FullName);
            }

            Console.WriteLine();
            Console.WriteLine("Dsign trich xuat tu pdf:");
            string signExtracted = TextFromPDF.Substring(indEndText + StartSignD.Length,
                indEndSign - indEndText - StartSignD.Length);
            Console.WriteLine(signExtracted);


            Console.WriteLine();
            string ContentPdf = TextFromPDF.Substring(0, indEndText);
            Console.WriteLine("Noi dung van ban lay tu pdf can xac thuc");
            Console.WriteLine(ContentPdf);
            Console.WriteLine("Do dai noi dung file pdf can xac thuc: " + ContentPdf.Length);


            var listDsign = signExtracted.Split("##");
            Console.WriteLine();
            Console.WriteLine("List Dsign:");
            bool checkExist = false;
            for (int i = 0; i < listDsign.Length; i++)
            {
                var item = listDsign[i];
                if (item.StartsWith(user.UserName))
                {
                    checkExist = true;
                    Console.WriteLine();
                    Console.WriteLine("Xac thuc chu ky cua user: " + user.UserName);
                    var UserDsign = item.Substring(user.UserName.Length);
                    Console.WriteLine("noi dung chu ky: ");
                    Console.WriteLine(UserDsign);
                    Console.WriteLine("do dai chu ky: ");
                    Console.WriteLine(UserDsign.Length);

                    //xac thuc theo user
                    Xacthuc(ContentPdf, UserDsign, user.PublicKey_string);
                    break;
                }

                if (i == listDsign.Length)
                {
                    Console.WriteLine();
                    Console.WriteLine("Khong tim thay Dsign cua: " + user.UserName);

                }

            }

            if (!checkExist)
            {
                throw new ResponseMessageException()
                    .WithCode(EResultResponse.FAIL.ToString())
                    .WithMessage("Xác thực thất bại với tài khoản: " + user.UserName + ": " + user.FullName);
            }
        }

        public static void Xacthuc(string noidungvb, string _Dsign, string _publickey)
        {
            var tmpSourePdfValid = ASCIIEncoding.ASCII.GetBytes(noidungvb);
            var Public_Key = StringtoKey(_publickey);
            // var SignD = Hex.Decode(_Dsign);
            var SignD = Convert.FromBase64String(_Dsign);

            // Xac nhan voi khoa Public cho file pdf dung
            Console.WriteLine();
            Console.WriteLine("Xac nhan file PDF voi Public_Key ");
            ISigner signCheckforPDF = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id);
            signCheckforPDF.Init(false, Public_Key);
            signCheckforPDF.BlockUpdate(tmpSourePdfValid, 0, tmpSourePdfValid.Length);
            bool statusforpdf = signCheckforPDF.VerifySignature(SignD);


            Console.WriteLine();
            Console.WriteLine("Ket qua Xac nhan van ban:");
            if (statusforpdf == true)
            {
                Console.WriteLine("File PDF dung");
            }
            else
            {
                Console.WriteLine("File PDF da duoc chinh sua");
            }
        }
    }

    public class KySoNoiBoService
    {


        public void TienTrinhKySo(string pathWord, string fileName, string pathPDF, List<User> users)
        {
            Console.WriteLine(".............................");
            Console.WriteLine(".......Demo Sign Digital.....");
            Console.WriteLine(".............................");

            //So luong User
            int Num_user = users.Count;

            //document File(

            byte[] tmpSource;


            //Lay noi dung file word
            string TextFromWord = SignDigitalService.ExtractWord(pathWord);
            Console.WriteLine();
            Console.WriteLine("Noi dung file word...");
            Console.WriteLine();
            Console.WriteLine(TextFromWord);
            Console.WriteLine();
            Console.WriteLine("Do dai noi dung file word: " + TextFromWord.Length);
            tmpSource = ASCIIEncoding.ASCII.GetBytes(TextFromWord);


            // Tao List chua cac ket qua ma hoa noi dung va chu ky

            List<string> listDSign = new List<string>();
            for (int i = 0; i < Num_user; i++)
            {
                ISigner signP = SignerUtilities.GetSigner(PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id);
                signP.Init(true, SignDigitalService.StringtoPrivateKey(users[i].PrivateKey_string));
                signP.BlockUpdate(tmpSource, 0, tmpSource.Length);
                byte[] SignD = signP.GenerateSignature();

                //listDSign.Add(listUser[i].name + Hex.ToHexString(SignD)); // '\u00cb'.ToString()
                listDSign.Add(users[i].UserName + Convert.ToBase64String(SignD)); // '\u00cb'.ToString()
            }

            string StartSignD = "startSignD";
            string EndSignD = "endSignD";
            var chukyso = StartSignD + string.Join("##", listDSign) + EndSignD;

            // chen vao word và xuất ra file pdf
            MultiInsertToDoc(chukyso, pathWord, pathPDF, fileName, users);

            // //xac thu chu ky
            // ValidDsign(pathFilePDF, listUser[0]);
        }

        public void MultiInsertToDoc(string p, string pathWord, string pathPDF, string fileName, List<User> listuser)
        {
            Document doc = new Document();
            doc.LoadFromFile(pathWord);
            int i;

            //Section sec = doc.AddSection();// them moi section
            Section sec = doc.LastSection; // khong them moi sextion

            //dinh dang chu ky so
            CharacterFormat formatDSign = new CharacterFormat(doc);
            formatDSign.FontName = "Calibri";
            formatDSign.FontSize = 2;
            formatDSign.Bold = false;
            formatDSign.TextColor = Color.White; // chu~ Trang se~ an chu ky so


            //danh dau ket thuc noi dung vb
            Paragraph parEnd = sec.AddParagraph();
            // parEnd.AppendBreak(BreakType.PageBreak);
            //TextBox textBoxEnd = parEnd.AppendTextBox(400, 20);
            //Paragraph EndText = textBoxEnd.Body.AddParagraph();
            // EndText.AppendText('\u00cb'.ToString()).ApplyCharacterFormat(formatSign);
            parEnd.AppendText(p).ApplyCharacterFormat(formatDSign);
            //textBoxEnd.Format.NoLine = true;

            //chen cac o ky ten
            //Paragraph par = sec.AddParagraph();
            //Dinh dang thong tin nguoi ky
            CharacterFormat formatHSign = new CharacterFormat(doc);
            formatHSign.FontSize = 12;
            formatHSign.Bold = false;


            int num_row = 0;

            //them chu ky vao vb
            if (listuser.Count % 2 == 0)
            {
                num_row = listuser.Count / 2;
            }
            else
            {
                num_row = listuser.Count / 2 + 1;
            }


            Table table = sec.AddTable(true);
            table.TableFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.None;
            table.ResetCells(num_row, 2); // set so dong/cot

            //table.ApplyHorizontalMerge(1,0, 1); //merge cell

            i = 0;
            for (int r = 0; r < num_row; r++)
            {
                TableRow DataRow = table.Rows[r];


                //C Represents Column 1.
                //Cell Alignment

                //Fill Data in Rows
                Paragraph p2 = DataRow.Cells[0].AddParagraph();
                p2.Format.HorizontalAlignment = HorizontalAlignment.Center;
                CharacterFormat formatHSignp2 = new CharacterFormat(doc);
                CharacterFormat formatHSignp21 = new CharacterFormat(doc);
                formatHSignp2.Bold = true;
                formatHSignp2.FontSize = 12;
                formatHSignp21.FontSize = 10;
                formatHSignp21.Italic = true;
                // p2.AppendText("CHỮ KÝ HỢP LỆ").ApplyCharacterFormat(formatHSignp2);
                p2.AppendText(listuser[i].ChucVu?.Ten).ApplyCharacterFormat(formatHSignp2);
                p2.AppendBreak(BreakType.LineBreak);
                p2.AppendBreak(BreakType.LineBreak);
                if (listuser[i].FilePath != default)
                {
                    Image image = null;
                    DocPicture pic2 = null;
                    try
                    {
                        image = Image.FromFile(listuser[i].FilePath);
                        pic2 = p2.AppendPicture(image);
                    }
                    catch (Exception e)
                    {
                        image = Image.FromFile(Constants.DEFAULT_LOGO);
                        pic2 = p2.AppendPicture(image);
                    }

                    pic2.Height = 80f;
                    pic2.Width = 150f;
                }
                p2.AppendBreak(BreakType.LineBreak);
                p2.AppendBreak(BreakType.LineBreak);

                p2.AppendText(listuser[i].FullName).ApplyCharacterFormat(formatHSignp2);
                p2.AppendBreak(BreakType.LineBreak);
                p2.AppendText("Ký số tại DThU").ApplyCharacterFormat(formatHSignp21);
                p2.AppendBreak(BreakType.LineBreak);
                p2.AppendText(listuser[i].NgayKy).ApplyCharacterFormat(formatHSignp21);
                p2.AppendBreak(BreakType.LineBreak);
                // CharacterFormat formatHSignp21 = new CharacterFormat(doc);
                // formatHSignp21.FontSize = 10;
                // p2.AppendText(listuser[i].DonVi?.Ten).ApplyCharacterFormat(formatHSignp21);
                // p2.AppendBreak(BreakType.LineBreak);
                // p2.AppendText(listuser[i].NgayKy).ApplyCharacterFormat(formatHSignp21);

                // DataRow.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle ;

                i = i + 1;

                //if (listuser.Count%2!=0 && r== listuser.Count-2)
                if (i > listuser.Count - 1 && listuser.Count % 2 != 0)
                {
                    table.ApplyHorizontalMerge(r, 0, 1); //merge cell
                }
                else
                {
                    //C Represents Column 2.
                    //DataRow.Cells[1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    //Fill Data in Rows
                    Paragraph p3 = DataRow.Cells[1].AddParagraph();
                    p3.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    // p3.AppendText("CHỮ KÝ HỢP LỆ").ApplyCharacterFormat(formatHSign);
                    CharacterFormat formatHSignp3 = new CharacterFormat(doc);
                    CharacterFormat formatHSignp31 = new CharacterFormat(doc);
                    formatHSignp3.Bold = true;
                    formatHSignp3.FontSize = 12;
                    formatHSignp31.FontSize = 10;
                    formatHSignp31.Italic = true;
                    p3.AppendText(listuser[i].ChucVu?.Ten).ApplyCharacterFormat(formatHSignp3);
                    p3.AppendBreak(BreakType.LineBreak);
                    if (listuser[i].FilePath != default)
                    {
                        Image image = null;
                        DocPicture pic3 = null;
                        try
                        {
                            image = Image.FromFile(listuser[i].FilePath);
                            pic3 = p3.AppendPicture(image);
                        }
                        catch (Exception e)
                        {
                            image = Image.FromFile(Constants.DEFAULT_LOGO);
                            pic3 = p3.AppendPicture(image);
                        }


                        pic3.Height = 80f;
                        pic3.Width = 150f;
                    }
                    p3.AppendBreak(BreakType.LineBreak);
                    p3.AppendBreak(BreakType.LineBreak);
                    p3.AppendText(listuser[i].FullName).ApplyCharacterFormat(formatHSignp3);
                    p3.AppendBreak(BreakType.LineBreak);
                    p3.AppendText("Ký số tại DThU").ApplyCharacterFormat(formatHSignp31);
                    p3.AppendBreak(BreakType.LineBreak);
                    p3.AppendText(listuser[i].NgayKy).ApplyCharacterFormat(formatHSignp31);
                    p3.AppendBreak(BreakType.LineBreak);
                    // CharacterFormat formatHSignp31 = new CharacterFormat(doc);
                    // formatHSignp31.FontSize = 10;
                    // p3.AppendText(listuser[i].DonVi?.Ten).ApplyCharacterFormat(formatHSignp31);
                    // p3.AppendBreak(BreakType.LineBreak);
                    // p3.AppendText(listuser[i].NgayKy).ApplyCharacterFormat(formatHSignp31);

                    i = i + 1;
                }
            }

            // doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
            //
            try
            {
                doc.SaveToFile(pathPDF, FileFormat.PDF);
                // using (MemoryStream ms = new MemoryStream())
                // {
                //
                //     doc.SaveToFile(ms, FileFormat.Doc);
                //     doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                //     File.WriteAllBytes(pathPDF, ms.ToArray());
                // }
                // doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                // using (var strem = System.IO.File.Create(pathPDF))
                // {
                //     // strem.Position = 0;
                //     doc.SaveToStream(strem, Spire.Doc.FileFormat.PDF);
                // }
                // doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);
                // using (var stream = new FileStream
                //            (pathPDF, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                // {
                //     stream.Position = 0;
                //     doc.SaveToStream(stream, Spire.Doc.FileFormat.PDF);
                //     stream.CopyToAsync()
                // }
                // doc.SaveToFile(pathPDF);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("Ghi File thanh cong!");

            //mo file
            // Process pr = new Process();
            // ProcessStartInfo pi = new ProcessStartInfo();
            // pi.UseShellExecute = true;
            // pi.FileName = pathPDF;
            // pr.StartInfo = pi;
            //
            // try
            // {
            //     pr.Start();
            // }
            // catch (Exception Ex)
            // {
            //     //MessageBox.Show(Ex.Message);
            // }
        }
    }
}