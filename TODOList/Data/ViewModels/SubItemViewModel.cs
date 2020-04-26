using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOList
{
    class SubItemViewModel:BaseViewModel
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

        public SubItemViewModel(string Title)
        {
            this.Title = Title;
            this.IsCompleted = false;
        }
    }
}
