using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrustruture {
    public class ShowUserInfo : IShowUserInfo {
        public string Title { get; set; } = "Knight";

        public string Description { get; set; } = " modern ";

        public string ShowInfo() {
            return Title + " - " + Description;
        }
    }
}
