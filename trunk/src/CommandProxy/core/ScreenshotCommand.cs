using System;using System.Collections.Generic;using System.Text;using System.Drawing;using System.Windows.Forms;using System.Drawing.Imaging;using System.Xml;using System.IO;namespace CommandProxy.Commands{    class ScreenshotCommand : IProxyCommand    {/*<screenshot format="">	<path></path></screenshot>*/
        public XmlDocument Exec(XmlDocument requestDocument, XmlDocument responseDocument)        {            ImageFormat format = ImageFormat.Png;

            XmlNode commandNode = requestDocument.FirstChild.SelectSingleNode("screenshot");            XmlAttribute formatAt = commandNode.Attributes["format"];            if (formatAt != null)            {                switch (formatAt.Value)                {                    case "png":                    {                        format = ImageFormat.Png;                        break;                    }                    case "icon":                    {                        format = ImageFormat.Icon;                        break;                    }                    case "jpg":                    {                        format = ImageFormat.Jpeg;                        break;                    }                    case "gif":                    {                        format = ImageFormat.Gif;                        break;                    }                }            }            XmlNode pathNode = commandNode.SelectSingleNode("path");            string path;            if (pathNode == null)            {                path = Path.GetTempFileName();            }            else            {                path = pathNode.InnerXml;            }            try            {                TakeScreenShot(path, format);            }            catch (Exception)            {                throw new Exception("Error taking screenshot");            }

            XmlTextReader xmlReader = new XmlTextReader(new StringReader("<path>" + path + "</path>"));
            XmlNode pathElement = responseDocument.ReadNode(xmlReader);
            responseDocument.FirstChild.AppendChild(pathElement);

            return responseDocument;        }        private void TakeScreenShot(string path, ImageFormat format)        {            try            {                Rectangle bounds = Screen.GetBounds(Point.Empty);                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))                {                    using (Graphics g = Graphics.FromImage(bitmap))                    {                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);                    }                    bitmap.Save(path, format);                }            }            catch (Exception e)            {                throw e;            }        }    }}