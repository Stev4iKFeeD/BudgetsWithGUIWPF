using System;

namespace Budgets.GUI.WPF.Navigation
{
    public interface INavigatable<TObject> where TObject : Enum
    {
        public TObject Type { get; }

        public void ClearSensitiveData();
    }
}
