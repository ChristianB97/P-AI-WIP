using System;

namespace Translation.XML
{
    [Serializable]
    public class XmlLangFileInfo : XmlLanguageDefinition
    {
        public string Title { get; set; }
        public Dict Words { get; set; }

        public XmlLangFileInfo()
        {
            Words = new Dict();
        }
    }
}
