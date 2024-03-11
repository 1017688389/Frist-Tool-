using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolViewModel
{
    /// <summary>
    /// 简单工厂
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// 工厂模式创造
        /// </summary>
        /// <param name="enumVm"></param>
        /// <returns></returns>
        public static VMBase CreateVM(EnumVM enumVm)
        {   
            switch(enumVm)
            {
                case EnumVM.MainViewModel:
                    return new MainViewModel();
                default:
                    return null;
            }
        }
    }
    public enum EnumVM
    {
        MainViewModel,
    }
}
