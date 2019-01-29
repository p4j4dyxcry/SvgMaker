using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SvgMakerCore.Core;

namespace SvgMakerCore
{
    public enum AppMode
    {
        Select,
        CreateLine,
    }

    public class AppState : NotifyPropertyChanger
    {
        private AppMode _appMode;

        public AppMode AppMode
        {
            get => _appMode;
            set => SetProperty(ref _appMode, value);
        }
    }
}
