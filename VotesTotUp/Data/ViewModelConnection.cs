using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace VotesTotUp.Data
{
    public class ViewModelConnection
    {
        #region Properties

        private Type _viewType;
        public Type ViewType
        {
            get { return _viewType; }
        }

        private Type _viewModelType;
        public Type ViewModelType
        {
            get { return _viewModelType; }
        }

        private ContentControl _viewInstance = null;
        public ContentControl ViewInstance
        {
            get { return _viewInstance; }
            set { _viewInstance = value; }
        }

        #endregion

        #region Public methods

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

        #endregion
    }
}