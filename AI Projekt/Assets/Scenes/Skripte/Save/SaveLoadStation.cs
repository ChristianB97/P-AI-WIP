using System;

namespace Save
{
    public class SaveLoadStation
    {
        public EventHandler OnSave;
        public EventHandler OnLoad;

        public SaveLoadStation()
        {
            SavingEnvironment.InitEnvironment();
        }

        public void SaveObject<T>(T arg, string key)
        {
            SavingEnvironment.WriteRuntime(arg, key);
            SavingEnvironment.WriteRuntimeToFile();
        }

        public T LoadObject<T>(string key)
        {
            T myClass = SavingEnvironment.ReadValueFromRuntime<T>(key);
            if (myClass == null)
            {
                myClass = (T)Activator.CreateInstance(typeof(T));
            }
            return myClass;
        }
    }
}
