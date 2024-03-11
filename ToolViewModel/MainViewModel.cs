using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ToolViewModel;

namespace ToolViewModel
{
    public class MainViewModel : VMBase
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                IsTopMost = value;
                OnPropertyChanged("IsChecked");
            }
        }

        private bool _isTopMost;
        /// <summary>
        /// 是否将当前窗口置于顶层
        /// </summary>
        public bool IsTopMost
        {
            get => _isTopMost;
            set
            {
                _isTopMost = value;
                OnPropertyChanged("IsTopMost");
            }
        }

        private List<string> _typeItems;
        /// <summary>
        /// 绑定comboboxi
        /// </summary>
        public List<string> TypeItems
        {
            get => _typeItems;
            set
            {
                _typeItems = value;
                OnPropertyChanged("ItemSorce");
            }
        }


        internal MainViewModel()
        {
            InitData();
        }

        public virtual void InitData()
        {
            //最上层按钮
            IsChecked = true;
            
            TypeItems = new List<string> { "需求","bug修复","沟通事项","需求延长" };
        }
    }
}
