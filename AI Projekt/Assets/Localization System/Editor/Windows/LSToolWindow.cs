using UnityEditor;

namespace LS.Editor
{
    public abstract class LSToolWindow : EditorWindow
    {
        public int LastExitCode { get; set; }

        public bool IsOK
        {
            get { return LastExitCode == 1; }
        }

        public bool IsCancelled
        {
            get { return LastExitCode == 2; }
        }

        protected virtual void Awake()
        {
            LastExitCode = 0;
        }

        protected void AcceptDialog()
        {
            LastExitCode = 1;
            ExecuteExitCallback();
            if (LastExitCode != 0)
                this.Close();
        }

        protected void CancelDialog()
        {
            LastExitCode = 2;
            ExecuteExitCallback();
            if (LastExitCode != 0)
                this.Close();
        }

        public void CancelClosing()
        {
            LastExitCode = 0;
        }

        protected virtual void ExecuteExitCallback()
        {
        }
    }
}
