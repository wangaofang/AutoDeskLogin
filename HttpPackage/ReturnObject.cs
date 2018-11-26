using System.Net;

namespace HttpPackage
{
    public class ReturnObject
    {
        public Cookie Cookies { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public ReturnObject(){}
    }
}