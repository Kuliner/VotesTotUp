using System;
using System.Windows.Controls;

namespace VotesTotUp.Data
{
    public class ViewModelConnection
    {
        #region Fields

        private ContentControl _viewInstance = null;
        private Type _viewModelType;
        private Type _viewType;

        #endregion Fields

        #region Properties

        public ContentControl ViewInstance
        {
            get { return _viewInstance; }
            set { _viewInstance = value; }
        }
        public Type ViewModelType
        {
            get { return _viewModelType; }
        }
        public Type ViewType
        {
            get { return _viewType; }
        }

        #endregion Properties

        #region Methods

        public void Set<ViewModel, View>()
        {
            _viewType = typeof(View);
            _viewModelType = typeof(ViewModel);
        }

        public void Set<ViewModel, View>(ContentControl viewInstance)
        {
            _viewType = typeof(View);
            _viewModelType = typeof(ViewModel);
            _viewInstance = viewInstance;
        }

        #endregion Methods
    }
}