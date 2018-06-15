using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ExpertPdf.HtmlToPdf;
using Framework.Commons.Logging;

namespace Framework.Commons.Utilities
{
    public static class PdfUtil
    {
        public static UrlHelper Url => new UrlHelper(HttpContext.Current.Request.RequestContext);

        /// <summary>
        ///     Add a header to a pdf
        /// </summary>
        /// <param name="pdfConverter">The PDF converter class</param>
        /// <param name="headerUrl">The URL to download the header</param>
        /// <param name="filePath">The file path to store the header</param>
        /// <param name="height">The height of the header</param>
        /// <param name="width">The width of the header</param>
        public static void AddHeader(PdfConverter pdfConverter, string headerUrl, string filePath, float height,
            float width)
        {
            string path = HostingEnvironment.MapPath(filePath);
            //enable header
            pdfConverter.PdfDocumentOptions.ShowHeader = true;
            // set the header height in points
            pdfConverter.PdfHeaderOptions.HeaderHeight = height;
            pdfConverter.PdfHeaderOptions.DrawHeaderLine = false;
            pdfConverter.AvoidImageBreak = true;
            // set the header HTML area 
            try
            {
                if (!File.Exists(path)) DownloadPdfHeader(headerUrl, path);
            }
            catch (ArgumentNullException e)
            {
                Logger.LogError("Error downloading pdf header, one of the arguments was null: " + e.Message);
            }
            catch (WebException e)
            {
                Logger.LogError("Error downloading pdf header, web exception: " + e.Message);
            }
            catch (NotSupportedException e)
            {
                Logger.LogError("Error downloading pdf header, notsupportedexception: " + e.Message);
            }

            // set the header height in points
            pdfConverter.PdfFooterOptions.FooterHeight = height;
            pdfConverter.PdfHeaderOptions.ImageArea = new ImageArea(0, 0, width, path);
        }

        /// <summary>
        ///     Add a footer to a pdf
        /// </summary>
        /// <param name="pdfConverter">The PDF converter class</param>
        /// <param name="footerUrl">The URL to download the footer</param>
        /// <param name="filePath">The file path to store the footer</param>
        /// <param name="height">The height of the footer</param>
        /// <param name="width">The width of the footer</param>
        public static void AddFooter(PdfConverter pdfConverter, string footerUrl, string filePath, float height,
            float width)
        {
            string path = HostingEnvironment.MapPath(filePath);
            //enable header
            pdfConverter.PdfDocumentOptions.ShowFooter = true;
            pdfConverter.AvoidImageBreak = true;
            pdfConverter.PdfFooterOptions.DrawFooterLine = false;
            try
            {
                if (!File.Exists(path)) DownloadPdfFooter(footerUrl, path);
            }
            catch (ArgumentNullException e)
            {
                Logger.LogError("Error downloading pdf footer, one of the arguments was null: " + e.Message);
            }
            catch (WebException e)
            {
                Logger.LogError("Error downloading pdf footer, web exception: " + e.Message);
            }
            catch (NotSupportedException e)
            {
                Logger.LogError("Error downloading pdf footer, notsupportedexception: " + e.Message);
            }

            // set the footer height in points
            pdfConverter.PdfFooterOptions.FooterHeight = height;
            //write the page number
            pdfConverter.PdfFooterOptions.ImageArea = new ImageArea(0, 0, width, path);
        }

        private static void DownloadPdfHeader(string headerUrl, string filePath)
        {
            Logger.LogInfo("Downloading pdf header from: " + headerUrl + " to " + filePath);
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                       | SecurityProtocolType.Tls11
                                                       | SecurityProtocolType.Tls12
                                                       | SecurityProtocolType.Ssl3;
                wc.DownloadFile(headerUrl, filePath);
            }

            Logger.LogInfo("Pdf header download successful!");
        }

        private static void DownloadPdfFooter(string headerUrl, string filePath)
        {
            Logger.LogInfo("Downloading pdf footer from: " + headerUrl + " to " + filePath);

            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                       | SecurityProtocolType.Tls11
                                                       | SecurityProtocolType.Tls12
                                                       | SecurityProtocolType.Ssl3;
                wc.DownloadFile(headerUrl, filePath);
            }

            Logger.LogInfo("Pdf footer download successful!");
        }
    }
}