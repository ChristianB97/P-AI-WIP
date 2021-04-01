namespace LS.Editor
{
    public class XmlWordEditorLine
    {
        public bool IsNew { get; private set; }
        public bool Dirty
        {
            get
            {
                return savedKey != Key ||
                    savedValue != Value;
            }
        }

        private string savedKey = "";
        private string savedValue = "";

        public string Key { get; set; }
        public string Value { get; set; }

        public XmlWordEditorLine()
        {
            IsNew = true;
        }

        public XmlWordEditorLine(string key, string value)
        {
            Key = key;
            Value = value;
            Save();
        }

        public void Save()
        {
            IsNew = false;
            savedValue = Value;
            savedKey = Key;
        }
    }
}
