using System.Text;

namespace FD.Service.Http
{
    public class StringContent
    {
        public string Content { get; set; }
        public Encoding Encoding { get; set; }

        public StringContent(string content)
            : this(content, null)
        {
        }
        public StringContent(string content, Encoding encoding)
        {
            Content = content;
            Encoding = encoding;
        }
    }
}
