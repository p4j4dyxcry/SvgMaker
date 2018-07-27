using System.IO;
using System.Windows;
using System.Xml;

namespace SvgMakerCore
{
    public class svg
    {
        public string xmlns = "http://www.w3.org/2000/svg";
        public Thickness viewBox;
    }

    public class SvgSaver
    {
        public SvgSaver(string filePath, Geometry2D.Geometry2D[] geometrys , int w , int h)
        {
            var dir = Path.GetDirectoryName(filePath) ?? string.Empty;
            if (Directory.Exists(dir) is false)
                Directory.CreateDirectory(dir);

            var docment = new XmlDocument();

            docment.CreateAttribute("svg", "xmlns", "http://www.w3.org/2000/svg");


            docment.Save(filePath);
        }
    }
}
